using System.ComponentModel;
using System.Drawing;

namespace Wellbeing
{
    partial class LockedScreenOverlay
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
            this.SuspendLayout();
            // 
            // BlackBackground
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Location = new Point(0, 0);
            this.ShowInTaskbar = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LockedScreenOverlay";
            this.TopMost = true;
            this.BackColor = Color.DarkBlue;
            this.TransparencyKey = Color.DarkBlue;
            this.ResumeLayout(false);
        }

        #endregion
    }
}