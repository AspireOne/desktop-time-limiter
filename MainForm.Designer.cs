namespace Digital_wellbeing
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.TimeLbl = new System.Windows.Forms.Label();
            this.CloseButt = new System.Windows.Forms.Button();
            this.ChangeMaxButt = new System.Windows.Forms.Button();
            this.ChangePassedButt = new System.Windows.Forms.Button();
            this.ToggleButt = new System.Windows.Forms.Button();
            this.StatusLbl = new System.Windows.Forms.Label();
            this.ChangePasswordButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TimeLbl
            // 
            this.TimeLbl.Font = new System.Drawing.Font("Microsoft YaHei UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TimeLbl.Location = new System.Drawing.Point(12, 9);
            this.TimeLbl.Name = "TimeLbl";
            this.TimeLbl.Size = new System.Drawing.Size(415, 81);
            this.TimeLbl.TabIndex = 0;
            this.TimeLbl.Text = "Čas:";
            this.TimeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CloseButt
            // 
            this.CloseButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CloseButt.Location = new System.Drawing.Point(34, 330);
            this.CloseButt.Name = "CloseButt";
            this.CloseButt.Size = new System.Drawing.Size(171, 58);
            this.CloseButt.TabIndex = 1;
            this.CloseButt.Text = "Ukončit";
            this.CloseButt.UseVisualStyleBackColor = true;
            // 
            // ChangeMaxButt
            // 
            this.ChangeMaxButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangeMaxButt.Location = new System.Drawing.Point(232, 165);
            this.ChangeMaxButt.Name = "ChangeMaxButt";
            this.ChangeMaxButt.Size = new System.Drawing.Size(171, 58);
            this.ChangeMaxButt.TabIndex = 2;
            this.ChangeMaxButt.Text = "Změnit max. čas";
            this.ChangeMaxButt.UseVisualStyleBackColor = true;
            // 
            // ChangePassedButt
            // 
            this.ChangePassedButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangePassedButt.Location = new System.Drawing.Point(34, 165);
            this.ChangePassedButt.Name = "ChangePassedButt";
            this.ChangePassedButt.Size = new System.Drawing.Size(171, 58);
            this.ChangePassedButt.TabIndex = 3;
            this.ChangePassedButt.Text = "Změnit strávený čas";
            this.ChangePassedButt.UseVisualStyleBackColor = true;
            // 
            // ToggleButt
            // 
            this.ToggleButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ToggleButt.Location = new System.Drawing.Point(34, 248);
            this.ToggleButt.Name = "ToggleButt";
            this.ToggleButt.Size = new System.Drawing.Size(171, 58);
            this.ToggleButt.TabIndex = 4;
            this.ToggleButt.Text = "Pauznout/spustit";
            this.ToggleButt.UseVisualStyleBackColor = true;
            // 
            // StatusLbl
            // 
            this.StatusLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.StatusLbl.Location = new System.Drawing.Point(18, 78);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(177, 23);
            this.StatusLbl.TabIndex = 5;
            this.StatusLbl.Text = "Zapnuto";
            this.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChangePasswordButt
            // 
            this.ChangePasswordButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangePasswordButt.Location = new System.Drawing.Point(232, 248);
            this.ChangePasswordButt.Name = "ChangePasswordButt";
            this.ChangePasswordButt.Size = new System.Drawing.Size(171, 58);
            this.ChangePasswordButt.TabIndex = 6;
            this.ChangePasswordButt.Text = "Změnit heslo";
            this.ChangePasswordButt.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 417);
            this.Controls.Add(this.ChangePasswordButt);
            this.Controls.Add(this.StatusLbl);
            this.Controls.Add(this.ToggleButt);
            this.Controls.Add(this.ChangePassedButt);
            this.Controls.Add(this.ChangeMaxButt);
            this.Controls.Add(this.CloseButt);
            this.Controls.Add(this.TimeLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "Digitální blahobyt";
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button ChangePasswordButt;

        private System.Windows.Forms.Label StatusLbl;

        private System.Windows.Forms.Button ToggleButt;

        private System.Windows.Forms.Button CloseButt;
        private System.Windows.Forms.Button ChangeMaxButt;
        private System.Windows.Forms.Button ChangePassedButt;

        private System.Windows.Forms.Label TimeLbl;

        #endregion
    }
}