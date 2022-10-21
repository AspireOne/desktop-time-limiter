using System.ComponentModel;

namespace Wellbeing
{
    partial class TimeDialog
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
            this.HoursBox = new System.Windows.Forms.NumericUpDown();
            this.HoursLbl = new System.Windows.Forms.Label();
            this.MinutesBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.HoursBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinutesBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OkButt
            // 
            this.OkButt.Location = new System.Drawing.Point(88, 141);
            this.OkButt.Name = "OkButt";
            this.OkButt.Size = new System.Drawing.Size(101, 34);
            this.OkButt.TabIndex = 0;
            this.OkButt.Text = "Ok";
            this.OkButt.UseVisualStyleBackColor = true;
            // 
            // HoursBox
            // 
            this.HoursBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HoursBox.Location = new System.Drawing.Point(33, 67);
            this.HoursBox.Maximum = new decimal(new int[] { 23, 0, 0, 0 });
            this.HoursBox.Name = "HoursBox";
            this.HoursBox.Size = new System.Drawing.Size(48, 26);
            this.HoursBox.TabIndex = 3;
            // 
            // HoursLbl
            // 
            this.HoursLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.HoursLbl.Location = new System.Drawing.Point(82, 67);
            this.HoursLbl.Name = "HoursLbl";
            this.HoursLbl.Size = new System.Drawing.Size(27, 26);
            this.HoursLbl.TabIndex = 4;
            this.HoursLbl.Text = "h";
            this.HoursLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MinutesBox
            // 
            this.MinutesBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.MinutesBox.Location = new System.Drawing.Point(154, 67);
            this.MinutesBox.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            this.MinutesBox.Name = "MinutesBox";
            this.MinutesBox.Size = new System.Drawing.Size(51, 26);
            this.MinutesBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(211, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 26);
            this.label1.TabIndex = 7;
            this.label1.Text = "min";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(207, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "min";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TimeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 186);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MinutesBox);
            this.Controls.Add(this.HoursLbl);
            this.Controls.Add(this.HoursBox);
            this.Controls.Add(this.OkButt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = Properties.Resources.Time;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.HoursBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MinutesBox)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        public System.Windows.Forms.NumericUpDown MinutesBox;

        private System.Windows.Forms.Label HoursLbl;

        public System.Windows.Forms.NumericUpDown HoursBox;

        private System.Windows.Forms.Button OkButt;

        #endregion
    }
}