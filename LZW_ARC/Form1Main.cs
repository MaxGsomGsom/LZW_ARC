using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LZW_ARC
{
    public partial class Form1Main : Form
    {
        public Form1Main()
        {
            InitializeComponent();
        }

        private void button1Encode_Click(object sender, EventArgs e)
        {
            int chainCount = 100000;
            switch (trackBar1.Value)
            {
                case 0: chainCount = 4096; break;
                case 1: chainCount = 8192; break;
                case 2: chainCount = 16384; break;
                case 3: chainCount = 32768; break;
                case 4: chainCount = 65536; break;
                case 5: chainCount = 131072; break;
                case 6: chainCount = 262144; break;
                case 7: chainCount = 524288; break;
                case 8: chainCount = 1048576; break;
                case 9: chainCount = int.MaxValue; break;
            }


            Coder c = new Coder(0, chainCount, radioButton1full.Checked);
            OpenFileDialog opnDialog = new OpenFileDialog();
            SaveFileDialog svDialog = new SaveFileDialog();
            opnDialog.Title = "Файл для сжатия";
            opnDialog.ShowDialog();
            if (opnDialog.FileName == "") return;
            svDialog.DefaultExt = "lzw";
            svDialog.Filter = "Сжатые файлы|*.lzw";
            svDialog.FilterIndex = 1;
            svDialog.Title = "Место сохранения архива";
            svDialog.ShowDialog();
            if (svDialog.FileName == "") return;
            button1Encode.Enabled = false;
            button2Decode.Enabled = false;

            label3in.Text = (opnDialog.FileName.Length > 50) ? (opnDialog.FileName.Substring(opnDialog.FileName.Length - 50, 50)) : (opnDialog.FileName);
            label4out.Text = (svDialog.FileName.Length > 50) ? (svDialog.FileName.Substring(svDialog.FileName.Length - 50, 50)) : (svDialog.FileName);

            c.PersentEvent+=c_PersentEventEncode;
            c.EncodeAsync(opnDialog.FileName, svDialog.FileName);
        }


        private void c_PersentEventEncode(object sender, PersentEventArgs e)
        {
            progressBar1.Value = e.persent;
            label1Progress.Text = "Процесс сжатия: " + e.currentBytes/1024 + " из " + e.lenghtBytes/1024 + " КБайт\nСтепень сжатия: " + e.compression + "%";
            if (e.persent == 100) 
            { 
                MessageBox.Show("Сжатие завершено!", "Завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1Encode.Enabled = true;
                button2Decode.Enabled = true;
            }
        }

        private void button1Decode_Click(object sender, EventArgs e)
        {
            Decoder d = new Decoder();
            OpenFileDialog opnDialog = new OpenFileDialog();
            opnDialog.Filter = "Сжатые файлы|*.lzw";
            opnDialog.FilterIndex = 1;
            opnDialog.Title = "Файл для распаковки";
            opnDialog.ShowDialog();
            if (opnDialog.FileName == "") return;
            FolderBrowserDialog svDialog = new FolderBrowserDialog();
            svDialog.Description = "Место сохранения распакованного файла";
            svDialog.ShowDialog();
            if (svDialog.SelectedPath == "") return;
            button1Encode.Enabled = false;
            button2Decode.Enabled = false;

            label3in.Text = (opnDialog.FileName.Length>50)?(opnDialog.FileName.Substring(opnDialog.FileName.Length - 50, 50)):(opnDialog.FileName);
            label4out.Text = (svDialog.SelectedPath.Length>50)?(svDialog.SelectedPath.Substring(svDialog.SelectedPath.Length - 50, 50)):(svDialog.SelectedPath);
            
            d.PersentEvent += c_PersentEventDecode;
            d.DecodeAsync(opnDialog.FileName, svDialog.SelectedPath);
        }

        private void c_PersentEventDecode(object sender, PersentEventArgs e)
        {
            progressBar1.Value = e.persent;
            label1Progress.Text = "Процесс распаковки: " + e.currentBytes / 1024 + " из " + e.lenghtBytes / 1024 + " КБайт\nСтепень сжатия: " + e.compression + "%";
            if (e.persent == 100)
            {
                MessageBox.Show("Распаковка завершена!", "Завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1Encode.Enabled = true;
                button2Decode.Enabled = true;
            }
        }


    }
}
