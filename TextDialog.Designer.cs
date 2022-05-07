using System.ComponentModel;

namespace Digital_wellbeing
{
    partial class TextDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.OkButt = new System.Windows.Forms.Button();
            this.TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OkButt
            // 
            this.OkButt.Location = new System.Drawing.Point(88, 141);
            this.OkButt.Name = "OK";
            this.OkButt.Size = new System.Drawing.Size(101, 34);
            this.OkButt.TabIndex = 0;
            this.OkButt.Text = "Ok";
            this.OkButt.UseVisualStyleBackColor = true;
            // 
            // TextBox
            // 
            this.TextBox.Location = new System.Drawing.Point(44, 69);
            this.TextBox.Name = "TextBox";
            this.TextBox.PasswordChar = '卐';
            this.TextBox.Size = new System.Drawing.Size(190, 20);
            this.TextBox.TabIndex = 1;
            // 
            // TextDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 186);
            this.Controls.Add(this.TextBox);
            this.Controls.Add(this.OkButt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TextDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button OkButt;
        public System.Windows.Forms.TextBox TextBox;

        #endregion
    }
}