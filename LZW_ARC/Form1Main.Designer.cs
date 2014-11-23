namespace LZW_ARC
{
    partial class Form1Main
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1Encode = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1Progress = new System.Windows.Forms.Label();
            this.button2Decode = new System.Windows.Forms.Button();
            this.groupBox1Options = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3in = new System.Windows.Forms.Label();
            this.label4out = new System.Windows.Forms.Label();
            this.radioButton1full = new System.Windows.Forms.RadioButton();
            this.radioButton2statistic = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1Options.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1Encode
            // 
            this.button1Encode.Location = new System.Drawing.Point(16, 318);
            this.button1Encode.Margin = new System.Windows.Forms.Padding(4);
            this.button1Encode.Name = "button1Encode";
            this.button1Encode.Size = new System.Drawing.Size(160, 41);
            this.button1Encode.TabIndex = 0;
            this.button1Encode.Text = "Сжать файл";
            this.button1Encode.UseVisualStyleBackColor = true;
            this.button1Encode.Click += new System.EventHandler(this.button1Encode_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 129);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(482, 28);
            this.progressBar1.TabIndex = 1;
            // 
            // label1Progress
            // 
            this.label1Progress.AutoSize = true;
            this.label1Progress.Location = new System.Drawing.Point(13, 72);
            this.label1Progress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1Progress.Name = "label1Progress";
            this.label1Progress.Size = new System.Drawing.Size(118, 34);
            this.label1Progress.TabIndex = 2;
            this.label1Progress.Text = "Процесс:\r\nСтепень сжатия:";
            // 
            // button2Decode
            // 
            this.button2Decode.Location = new System.Drawing.Point(338, 318);
            this.button2Decode.Name = "button2Decode";
            this.button2Decode.Size = new System.Drawing.Size(161, 41);
            this.button2Decode.TabIndex = 3;
            this.button2Decode.Text = "Распаковать файл";
            this.button2Decode.UseVisualStyleBackColor = true;
            this.button2Decode.Click += new System.EventHandler(this.button1Decode_Click);
            // 
            // groupBox1Options
            // 
            this.groupBox1Options.Controls.Add(this.label4);
            this.groupBox1Options.Controls.Add(this.label3);
            this.groupBox1Options.Controls.Add(this.trackBar1);
            this.groupBox1Options.Location = new System.Drawing.Point(16, 178);
            this.groupBox1Options.Name = "groupBox1Options";
            this.groupBox1Options.Size = new System.Drawing.Size(270, 122);
            this.groupBox1Options.TabIndex = 4;
            this.groupBox1Options.TabStop = false;
            this.groupBox1Options.Text = "Настройки сжатия";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Размер";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Скорость";
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(11, 57);
            this.trackBar1.Maximum = 9;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(247, 56);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.Value = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Вход:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Выход:";
            // 
            // label3in
            // 
            this.label3in.Location = new System.Drawing.Point(76, 18);
            this.label3in.Name = "label3in";
            this.label3in.Size = new System.Drawing.Size(423, 23);
            this.label3in.TabIndex = 7;
            this.label3in.Text = "-";
            // 
            // label4out
            // 
            this.label4out.Location = new System.Drawing.Point(75, 44);
            this.label4out.Name = "label4out";
            this.label4out.Size = new System.Drawing.Size(423, 23);
            this.label4out.TabIndex = 8;
            this.label4out.Text = "-";
            // 
            // radioButton1full
            // 
            this.radioButton1full.AutoSize = true;
            this.radioButton1full.Location = new System.Drawing.Point(26, 30);
            this.radioButton1full.Name = "radioButton1full";
            this.radioButton1full.Size = new System.Drawing.Size(79, 21);
            this.radioButton1full.TabIndex = 3;
            this.radioButton1full.Text = "Полная";
            this.radioButton1full.UseVisualStyleBackColor = true;
            // 
            // radioButton2statistic
            // 
            this.radioButton2statistic.AutoSize = true;
            this.radioButton2statistic.Checked = true;
            this.radioButton2statistic.Location = new System.Drawing.Point(26, 57);
            this.radioButton2statistic.Name = "radioButton2statistic";
            this.radioButton2statistic.Size = new System.Drawing.Size(125, 21);
            this.radioButton2statistic.TabIndex = 4;
            this.radioButton2statistic.TabStop = true;
            this.radioButton2statistic.Text = "По статистике";
            this.radioButton2statistic.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1full);
            this.groupBox1.Controls.Add(this.radioButton2statistic);
            this.groupBox1.Location = new System.Drawing.Point(292, 178);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(207, 122);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Очистка таблицы цепочек";
            // 
            // Form1Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 377);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4out);
            this.Controls.Add(this.label3in);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1Options);
            this.Controls.Add(this.button2Decode);
            this.Controls.Add(this.label1Progress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1Encode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LZW ARC";
            this.groupBox1Options.ResumeLayout(false);
            this.groupBox1Options.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1Encode;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1Progress;
        private System.Windows.Forms.Button button2Decode;
        private System.Windows.Forms.GroupBox groupBox1Options;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3in;
        private System.Windows.Forms.Label label4out;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.RadioButton radioButton1full;
        private System.Windows.Forms.RadioButton radioButton2statistic;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

