using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace LZW_ARC
{
    class ChainShell
    {
        public ChainShell(int index, byte[] chain, int statistic)
        {
            this.index = index;
            this.chain = chain;
            this.statistic = statistic;
        }

        public int index;
        public byte[] chain;
        public int statistic;
    }

    class PersentEventArgs: EventArgs 
    {
        public int persent = 0;
        public long lenghtBytes = 0;
        public long currentBytes = 0;
        public int compression = 0;
        public PersentEventArgs (int persent, long lenghtBytes, long currentBytes, int compression) {
            this.persent = persent;
            this.lenghtBytes = lenghtBytes;
            this.currentBytes = currentBytes;
            this.compression = compression;
        }
    }

    class Coder
    {
        int curIndexLenght = 9;
        int writeBytesBlockCount = 512;
        int readBytesBlockCount = 512;
        int delChainStatistic = 1;
        int delTableCount = 100000;
        bool usingHash = false;

        long ii = -1;
        long inFileLength = -1;
        long outFileLength = -1;

        public event EventHandler<PersentEventArgs> PersentEvent;
        
        Timer t;

        
        public Coder(int delChainStatistic, int delTableCount, bool usingHash)
        {
            this.delChainStatistic = delChainStatistic;
            this.delTableCount = delTableCount;
            this.usingHash = usingHash;
            t = new Timer();
            t.Interval=500;
            t.Tick+=t_Tick;

            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (ii>=1)
            {
                int curPersent = (int)(((double)ii / (double)inFileLength) * 100);
                int compression = (int)(((double)outFileLength / (double)ii) * 100);
                PersentEventArgs e1 = new PersentEventArgs(curPersent, inFileLength, ii, compression);
                if (curPersent == 100) t.Stop();
                PersentEvent(this, e1); 
            }
        }


        public async void EncodeAsync(string inFileName, string outFileName)
        {
            await Task.Run(() => { Encode(inFileName, outFileName); });
        }


        void Encode(string inFileName, string outFileName)
        {
            FileStream inFile;
            FileStream outFile;
            try
            {
                inFile = new FileStream(inFileName, FileMode.Open);
                outFile = new FileStream(outFileName, FileMode.Create);
            }
            catch
            {
                MessageBox.Show("Отказано в доступе к файлу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ii = 1;
                inFileLength = 1;
                outFileLength = 1;
                return;
            }
            inFileLength = inFile.Length;

            //запись атрибутов файла в начало архива
            FileInfo inFileInfo = new FileInfo(inFileName);

            outFile.WriteByte(Convert.ToByte(inFileInfo.Name.Length));
            outFile.Write(Encoding.ASCII.GetBytes(inFileInfo.Name), 0, inFileInfo.Name.Length);
            outFile.Write(BitConverter.GetBytes(inFileInfo.LastWriteTimeUtc.Ticks), 0, sizeof(long));

            //очередь бит для записи в файл
            Queue<bool> outBitStream = new Queue<bool>();
            //очередь считанных новых байт из файла
            byte[] newSymbolsArr = new byte[readBytesBlockCount];
            Queue<byte> newSymbolsStream = new Queue<byte>();

            //таблица цепочек
            int maxTableSize = (int)Math.Pow(2, curIndexLenght);
            List<ChainShell> table = new List<ChainShell>();

            List<int> statistics = new List<int>();
            //заполнение корня
            for (int i = 0; i < Math.Pow(2, 8); i++)
            {
                //помещаем индекс, цепочку, статистику в капсулу
                byte[] chainBytes = { (byte)i };
                byte[] chainHash = GetHash(chainBytes);
                ChainShell shellChain = new ChainShell(i, chainHash, int.MinValue);
                //помещаем её на место в таблице
                int chainPlace = SearchChain(table, chainHash, true);
                table.Insert(chainPlace, shellChain);

            }
            //номер последней цепочки в таблице
            int tableLastIndex = (int)Math.Pow(2, 8) - 1;
            //префикс
            byte[] prefix = { };
            //основной цикл
            for (ii = 0; ii < inFile.Length; ii++)
            {

                //чтение очередного блока байт
                if (newSymbolsStream.Count == 0)
                {
                    int readCount = readBytesBlockCount;
                    if ((inFile.Length - inFile.Position) < readBytesBlockCount) readCount = (int)(inFile.Length - inFile.Position);

                    inFile.Read(newSymbolsArr, 0, readCount);
                    for (int m = 0; m < readCount; m++)
                    {
                        newSymbolsStream.Enqueue(newSymbolsArr[m]);
                    }
                }

                byte newSymbol = newSymbolsStream.Dequeue();
                //добавляет байт к префиксу
                byte[] chainBytes = (byte[])prefix.Clone();
                Array.Resize<byte>(ref chainBytes, chainBytes.Length + 1);
                chainBytes[chainBytes.Length - 1] = newSymbol;
                //считает хэш цепочки
                byte[] chainHash = GetHash(chainBytes);

                //если эта цепь уже есть в таблице
                if (SearchChain(table, chainHash) >= 0) prefix = chainBytes;
                else
                {
                    //ищет место для цепочки в сортированном массиве
                    int placeForChain = SearchChain(table, chainHash, true);
                    //инкапсулирует индекс, цепочку и ее статистику
                    tableLastIndex++;
                    ChainShell shellChain = new ChainShell(tableLastIndex, chainHash, 0);
                    //вставляет капсулу в массив на нужное место
                    table.Insert(placeForChain, shellChain);

                    //находит префикс в таблице и увеличивает статистику
                    byte[] prefixHash = GetHash(prefix);
                    int prefixPosInTable = SearchChain(table, prefixHash);
                    table[prefixPosInTable].statistic = table[prefixPosInTable].statistic + 1;

                    //записывает номер цепочки как массив бит с поправкой на сортировку 
                    int[] prefixIndexInt = { table[prefixPosInTable].index };
                    BitArray prefixIndexBits = new BitArray(prefixIndexInt);
                    //добавляет биты в очередь
                    for (int m = 0; m < curIndexLenght; m++)
                    {
                        outBitStream.Enqueue(prefixIndexBits[m]);
                    }

                    //если очередь достигла определенного размера, записывает биты блоком из нескольких байт в файл
                    if (outBitStream.Count > 8 * writeBytesBlockCount)
                    {
                        byte[] bytesToWrite = new byte[writeBytesBlockCount];
                        BitArray bitsToBytes = new BitArray(8 * writeBytesBlockCount);
                        for (int k = 0; k < 8 * writeBytesBlockCount; k++)
                        {
                            bitsToBytes.Set(k, outBitStream.Dequeue());
                        }
                        bitsToBytes.CopyTo(bytesToWrite, 0);

                        outFile.Write(bytesToWrite, 0, writeBytesBlockCount);
                        outFileLength = outFile.Length;
                    }


                    byte[] newSymbolByte = { newSymbol };
                    prefix = newSymbolByte;


                }

                //увеличивает таблицу и длину номера цепочки при заполнении
                if (tableLastIndex == maxTableSize - 1)
                {
                    curIndexLenght += 1;
                    maxTableSize = (int)Math.Pow(2, curIndexLenght);
                }

                //очистка редко используемых цепочек при превышении ею определенного размера на основе статистики
                if (table.Count >= delTableCount)
                {
                    List<ChainShell> newTable = new List<ChainShell>(delTableCount);
                    for (int n = 0; n < table.Count; n++)
                    {
                        //если цепочка не использовалась с момента последней очистки - удалить ее
                        if (table[n].statistic > delChainStatistic || table[n].statistic < 0)
                        {
                            //если цепочка является одной из 256 первоначальных
                            if (table[n].statistic < 0) table[n].statistic = int.MinValue;
                            //если цепочка использовалась - обнулить статистику
                            else table[n].statistic = 0;
                            
                            newTable.Add(table[n]);
                        }

                    }
                    table = newTable;
                    //если удаление удаляет слишком мало цепочек увеличить минимальную длину 
                    if (table.Count > delTableCount * 0.8) delChainStatistic++;
                }

            }

            //запись номера последней цепочки
            byte[] prefixLastHash = GetHash(prefix);
            int[] prefixLastIndex = { table[SearchChain(table, prefixLastHash)].index};
            BitArray prefixLastIndexBits = new BitArray(prefixLastIndex);
            //добавляет биты в очередь
            for (int m = 0; m < curIndexLenght; m++)
            {
                outBitStream.Enqueue(prefixLastIndexBits[m]);
            }
            //записывает последние биты блоком из нескольких байт в файл и если нужно добавляет в конец пустые биты
            int lastBytesCount = (outBitStream.Count % 8 == 0) ? (outBitStream.Count / 8) : (outBitStream.Count / 8 + 1);
            byte[] lastBytes = new byte[lastBytesCount];
            BitArray lastBits = new BitArray(8 * lastBytesCount);
            for (int k = 0; k < 8 * lastBytesCount && outBitStream.Count > 0; k++)
            {
                lastBits.Set(k, outBitStream.Dequeue());
            }
            lastBits.CopyTo(lastBytes, 0);

            outFile.Write(lastBytes, 0, lastBytesCount);


            //закрытие файлов
            inFile.Close();
            outFile.Close();
        }

        //поиск цепочки в таблице или места для ее вставки бинарным поиском
        int SearchChain(List<ChainShell> table, byte[] chain, bool pasteMode = false)
        {
            //pasteMode = true двоичный поиск места цепочки для вставки
            //pasteMode = false двоичный поиск места цепочки в таблице

            int first = 0;
            int last = table.Count;

            //если массив пуст
            if (pasteMode && table.Count == 0) return 0;
            else if (BytesComparer(table[0].chain, chain) == 1)
            {
                //не найден, вставить элемент на индекс 0
                if (pasteMode) return 0;
                return -1;
            }
            else if (BytesComparer(table[last - 1].chain, chain) == -1)
            {
                //не найден, вставить элемент в конец
                if (pasteMode) return last;
                return -1;
            }

            //поиск элемента
            while (first < last)
            {
                int mid = first + (last - first) / 2;
                if (BytesComparer(chain, table[mid].chain) <= 0)
                {
                    last = mid;
                }
                else
                {
                    first = mid + 1;
                }
            }

            if (BytesComparer(table[last].chain, chain) == 0)
            {
                //найден, его индекс last
                return last;
            }
            //не найден, вставить элемент на индекс last
            else
            {
                if (pasteMode) return last;
                return -1;
            }
        }

        //сравнение двух цепочек
        int BytesComparer(byte[] one, byte[] two)
        {
            if (one.Length < two.Length) return -1;
            if (one.Length > two.Length) return 1;
            for (int i = 0; i < one.Length; i++)
            {
                if (one[i] < two[i]) return -1;
                if (one[i] > two[i]) return 1;
            }
            return 0;
        }

        //хэширование
        MD5 md5Alg = MD5Cng.Create();

        byte[] GetHash(byte[] buf)
        {
            if (!usingHash) return buf;
            return md5Alg.ComputeHash(buf);
        }

    }
}
