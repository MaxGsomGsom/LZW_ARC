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
            this.SuspendLayout();
            // 
            // button1Encode
            // 
            this.button1Encode.Location = new System.Drawing.Point(25, 200);
            this.button1Encode.Name = "button1Encode";
            this.button1Encode.Size = new System.Drawing.Size(94, 23);
            this.button1Encode.TabIndex = 0;
            this.button1Encode.Text = "Сжать файл";
            this.button1Encode.UseVisualStyleBackColor = true;
            this.button1Encode.Click += new System.EventHandler(this.button1Encode_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(25, 105);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(219, 23);
            this.progressBar1.TabIndex = 1;
            // 
            // label1Progress
            // 
            this.label1Progress.AutoSize = true;
            this.label1Progress.Location = new System.Drawing.Point(22, 58);
            this.label1Progress.Name = "label1Progress";
            this.label1Progress.Size = new System.Drawing.Size(37, 13);
            this.label1Progress.TabIndex = 2;
            this.label1Progress.Text = "          ";
            // 
            // Form1Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label1Progress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1Encode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1Main";
            this.Text = "LZW ARC";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1Encode;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1Progress;
    }
}

