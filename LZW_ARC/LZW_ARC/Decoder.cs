using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace LZW_ARC
{
    class Decoder
    {
        int curIndexLenght = 9;
        int readBytesBlockCount = 256;

        Timer t;
        long inFilePos = -1;
        long inFileLength = -1;
        long outFileLength = -1;

        public event EventHandler<PersentEventArgs> PersentEvent;

        public Decoder()
        {
            t = new Timer();
            t.Interval = 500;
            t.Tick += t_Tick;

            t.Start();
        }

        private void t_Tick(object sender, EventArgs e)
        {
            if (inFilePos >= 1)
            {
                int curPersent = (int)(((double)inFilePos / (double)inFileLength) * 100);
                int compression = (int)(((double)inFilePos / (double)outFileLength) * 100);
                PersentEventArgs e1 = new PersentEventArgs(curPersent, inFileLength, inFilePos, compression);
                if (curPersent == 100) t.Stop();
                PersentEvent(this, e1);
            }
        }

        public async void DecodeAsync(string inFileName, string outFolder)
        {
            await Task.Run(() => { Decode(inFileName, outFolder); });
        }

        void Decode(string inFileName, string outFolder)
        {
            FileStream inFile;
            FileStream outFile;
            try
            {
                inFile = new FileStream(inFileName, FileMode.Open);
            }
            catch
            {
                MessageBox.Show("Отказано в доступе к файлу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                inFilePos = 1;
                inFileLength = 1;
                outFileLength = 1;
                return;
            }
            inFileLength = inFile.Length;

            //чтение имени файла из архива
            int outFileNameLenght = inFile.ReadByte();
            byte[] outFileNameBytes = new byte[outFileNameLenght];
            inFile.Read(outFileNameBytes, 0, outFileNameLenght);
            string outFileName = Encoding.ASCII.GetString(outFileNameBytes);

            try
            {
                outFile = new FileStream(outFolder + "\\" + outFileName, FileMode.Create);
            }
            catch 
            {

                MessageBox.Show("Ошибочный формат файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                inFilePos = 1;
                inFileLength = 1;
                outFileLength = 1;
                return;
            }

            //очередь бит для чтения
            Queue<bool> inBitStream = new Queue<bool>();
            //макс. размер массива при текущей длине номера цепочки
            int curTableDimension = (int)Math.Pow(2, curIndexLenght);

            //чтение даты файла из архива
            byte[] outFileTimeTicksBytes = new byte[sizeof(long)];
            inFile.Read(outFileTimeTicksBytes, 0, sizeof(long));
            long outFileTimeTicks = BitConverter.ToInt64(outFileTimeTicksBytes, 0);
            DateTime outFileTime = new DateTime(outFileTimeTicks);

            //таблица цепочек
            List<byte[]> table = new List<byte[]>();
            //заполнение корня
            for (int i = 0; i < Math.Pow(2, 8); i++)
            {
                byte[] chain = { (byte)i };
                table.Add(chain);
            }

            //номер последнеей цепочки в таблице
            int tableLastIndex = (int)Math.Pow(2, 8) - 1;
            //чтение первого блока байт и помещение в очередь
            byte[] readBytesBlock = new byte[readBytesBlockCount];
            inFile.Read(readBytesBlock, 0, readBytesBlockCount);
            BitArray readBytesBits = new BitArray(readBytesBlock);

            for (int m = 0; m < readBytesBlockCount * 8; m++)
            {
                inBitStream.Enqueue(readBytesBits[m]);
            }
            //чтение первого номера из очереди и его преобразование в int
            byte[] indexBytes = new byte[4];
            BitArray indexBits = new BitArray(indexBytes);
            for (int k = 0; k < curIndexLenght; k++)
            {
                indexBits.Set(k, inBitStream.Dequeue());
            }
            indexBits.CopyTo(indexBytes, 0);

            int indexChain = BitConverter.ToInt32(indexBytes, 0);

            //запись первой цепочки
            byte[] outChain1 = table[indexChain];
            outFile.Write(outChain1, 0, outChain1.Length);

            int oldIndexChain = indexChain;

            //пока не конец файла
            while (inBitStream.Count >= curIndexLenght)
            {

                //чтение номера цепочки из очереди байт и преобразование его в int
                for (int k = 0; k < curIndexLenght; k++)
                {
                    indexBits.Set(k, inBitStream.Dequeue());
                }
                indexBits.CopyTo(indexBytes, 0);
                indexChain = BitConverter.ToInt32(indexBytes, 0);

                //если цепочка уже в таблице
                if (indexChain <= tableLastIndex)
                {
                    //запись ее в файл
                    byte[] chainForIndex = table[indexChain];
                    outFile.Write(chainForIndex, 0, chainForIndex.Length);

                    //добавление цепочки с новым символом из начала другой цепочки в таблицу
                    byte[] chain = table[oldIndexChain];
                    byte newSymbolChain = chainForIndex[0];
                    Array.Resize<byte>(ref chain, chain.Length + 1);
                    chain[chain.Length - 1] = newSymbolChain;

                    tableLastIndex++;

                    table.Add(chain);
                }
                //если цепи нет в таблице
                else
                {
                    //добавление цепочки с новым символом из этой же цепочки в таблицу и ее запись в файл
                    byte[] chain = table[oldIndexChain];
                    byte newSymbolChain = chain[0];
                    Array.Resize<byte>(ref chain, chain.Length + 1);
                    chain[chain.Length - 1] = newSymbolChain;
                    outFile.Write(chain, 0, chain.Length);

                    tableLastIndex++;

                    table.Add(chain);

                }

                oldIndexChain = indexChain;

                //увеличение размера номера цепочек при достижении предела
                if (table.Count == curTableDimension - 1)
                {
                    curIndexLenght += 1;
                    curTableDimension = (int)Math.Pow(2, curIndexLenght);
                }

                //считывание очередного блока байт при уменьшении длины очереди и добавление бит в очередь
                if (inBitStream.Count < 32 && inFile.Position != inFile.Length)
                {

                    int readBytesNumCur = readBytesBlockCount;
                    if ((inFile.Length - inFile.Position) < readBytesBlockCount) readBytesNumCur = (int)(inFile.Length - inFile.Position);

                    inFile.Read(readBytesBlock, 0, readBytesNumCur);
                    readBytesBits = new BitArray(readBytesBlock);

                    for (int m = 0; m < readBytesNumCur * 8; m++)
                    {
                        inBitStream.Enqueue(readBytesBits[m]);
                    }

                    inFilePos = inFile.Position;
                    outFileLength = outFile.Length;
                }
            }

            //закрытие фалов
            inFile.Close();
            outFile.Close();

            //запись времени файла
            FileInfo outFileInfo = new FileInfo(outFolder + "\\" + outFileName);
            outFileInfo.LastWriteTimeUtc = outFileTime;
        }


    }
}
