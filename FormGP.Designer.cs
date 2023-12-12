namespace GraphicPrimitives
{
    partial class FormGP
    {
        /// <summary>
        /// Обязательная переменная конструктора.
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGP));
            this.buttonClearLines = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonClearLines
            // 
            this.buttonClearLines.Location = new System.Drawing.Point(670, 42);
            this.buttonClearLines.Name = "buttonClearLines";
            this.buttonClearLines.Size = new System.Drawing.Size(185, 52);
            this.buttonClearLines.TabIndex = 0;
            this.buttonClearLines.Text = "Очистить связи";
            this.buttonClearLines.UseVisualStyleBackColor = true;
            this.buttonClearLines.Click += new System.EventHandler(this.buttonClearLines_Click);
            // 
            // FormGP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(882, 503);
            this.Controls.Add(this.buttonClearLines);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(900, 550);
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "FormGP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Графические примитивы";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClearLines;
    }
}

