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
            Coder c = new Coder(0, 100000, false);
            OpenFileDialog opnDialog = new OpenFileDialog();
            SaveFileDialog svDialog = new SaveFileDialog();
            opnDialog.Title = "Файл для сжатия";
            opnDialog.ShowDialog();
            if (opnDialog.FileName == "") return;
            svDialog.DefaultExt = "lzw";
            svDialog.Title = "Место сохранения архива";
            svDialog.ShowDialog();
            button1Encode.Enabled = false;
            if (svDialog.FileName == "") return;

            c.PersentEvent+=c_PersentEventEncode;
            c.EncodeAsync(opnDialog.FileName, svDialog.FileName);
        }


        private void c_PersentEventEncode(object sender, PersentEventArgs e)
        {
            progressBar1.Value = e.persent;
            label1Progress.Text = "Процесс сжатия: " + e.currentBytes/1024 + " из " + e.lenghtBytes/1024 + " КБайт\nСтепень сжатия: " + e.compression + "%";
            if (e.persent == 100) 
            { 
                MessageBox.Show("Сжатие завершено!");
                button1Encode.Enabled = true;
            }
        }
    }
}
