namespace Wellbeing
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
            this.ChangeResetHourButt = new System.Windows.Forms.Button();
            this.ChangeIdleTimeButt = new System.Windows.Forms.Button();
            this.versionLbl = new System.Windows.Forms.Label();
            this.LogButt = new System.Windows.Forms.Button();
            this.AppButt = new System.Windows.Forms.Button();
            this.RestartButt = new System.Windows.Forms.Button();
            this.IdleLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TimeLbl
            // 
            this.TimeLbl.Font = new System.Drawing.Font("Microsoft YaHei UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.TimeLbl.Location = new System.Drawing.Point(12, 9);
            this.TimeLbl.Name = "TimeLbl";
            this.TimeLbl.Size = new System.Drawing.Size(415, 81);
            this.TimeLbl.TabIndex = 0;
            this.TimeLbl.Text = Properties.Resources.Time + ":";
            this.TimeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CloseButt
            // 
            this.CloseButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CloseButt.Location = new System.Drawing.Point(34, 421);
            this.CloseButt.Name = "CloseButt";
            this.CloseButt.Size = new System.Drawing.Size(171, 58);
            this.CloseButt.TabIndex = 6;
            this.CloseButt.Text = Properties.Resources.Terminate;
            this.CloseButt.UseVisualStyleBackColor = true;
            // 
            // ChangeMaxButt
            // 
            this.ChangeMaxButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangeMaxButt.Location = new System.Drawing.Point(232, 165);
            this.ChangeMaxButt.Name = "ChangeMaxButt";
            this.ChangeMaxButt.Size = new System.Drawing.Size(171, 58);
            this.ChangeMaxButt.TabIndex = 2;
            this.ChangeMaxButt.Text = Properties.Resources.ChangeMaxButt;
            this.ChangeMaxButt.UseVisualStyleBackColor = true;
            // 
            // ChangePassedButt
            // 
            this.ChangePassedButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangePassedButt.Location = new System.Drawing.Point(34, 165);
            this.ChangePassedButt.Name = "ChangePassedButt";
            this.ChangePassedButt.Size = new System.Drawing.Size(171, 58);
            this.ChangePassedButt.TabIndex = 1;
            this.ChangePassedButt.Text = Properties.Resources.ChangePassedButt;
            this.ChangePassedButt.UseVisualStyleBackColor = true;
            // 
            // ToggleButt
            // 
            this.ToggleButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ToggleButt.Location = new System.Drawing.Point(34, 333);
            this.ToggleButt.Name = "ToggleButt";
            this.ToggleButt.Size = new System.Drawing.Size(171, 58);
            this.ToggleButt.TabIndex = 5;
            this.ToggleButt.Text = Properties.Resources.Pause + "/" + Properties.Resources.Start;
            this.ToggleButt.UseVisualStyleBackColor = true;
            // 
            // StatusLbl
            // 
            this.StatusLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.StatusLbl.Location = new System.Drawing.Point(16, 73);
            this.StatusLbl.Name = "StatusLbl";
            this.StatusLbl.Size = new System.Drawing.Size(144, 23);
            this.StatusLbl.TabIndex = 0;
            this.StatusLbl.Text = Properties.Resources.Status;
            this.StatusLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChangePasswordButt
            // 
            this.ChangePasswordButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangePasswordButt.Location = new System.Drawing.Point(232, 333);
            this.ChangePasswordButt.Name = "ChangePasswordButt";
            this.ChangePasswordButt.Size = new System.Drawing.Size(171, 58);
            this.ChangePasswordButt.TabIndex = 4;
            this.ChangePasswordButt.Text = Properties.Resources.ChangePass;
            this.ChangePasswordButt.UseVisualStyleBackColor = true;
            // 
            // ChangeResetHourButt
            // 
            this.ChangeResetHourButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangeResetHourButt.Location = new System.Drawing.Point(34, 248);
            this.ChangeResetHourButt.Name = "ChangeResetHourButt";
            this.ChangeResetHourButt.Size = new System.Drawing.Size(171, 58);
            this.ChangeResetHourButt.TabIndex = 3;
            this.ChangeResetHourButt.Text = Properties.Resources.ChangeResetHour;
            this.ChangeResetHourButt.UseVisualStyleBackColor = true;
            // 
            // ChangeIdleTimeButt
            // 
            this.ChangeIdleTimeButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ChangeIdleTimeButt.Location = new System.Drawing.Point(232, 248);
            this.ChangeIdleTimeButt.Name = "ChangeIdleTimeButt";
            this.ChangeIdleTimeButt.Size = new System.Drawing.Size(171, 58);
            this.ChangeIdleTimeButt.TabIndex = 7;
            this.ChangeIdleTimeButt.Text = Properties.Resources.ChangeIdleTime;
            this.ChangeIdleTimeButt.UseVisualStyleBackColor = true;
            // 
            // versionLbl
            // 
            this.versionLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.versionLbl.Location = new System.Drawing.Point(386, 502);
            this.versionLbl.Name = "versionLbl";
            this.versionLbl.Size = new System.Drawing.Size(38, 23);
            this.versionLbl.TabIndex = 8;
            this.versionLbl.Text = "x.x.x";
            this.versionLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LogButt
            // 
            this.LogButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.LogButt.Location = new System.Drawing.Point(329, 502);
            this.LogButt.Name = "LogButt";
            this.LogButt.Size = new System.Drawing.Size(51, 23);
            this.LogButt.TabIndex = 9;
            this.LogButt.Text = "Log";
            this.LogButt.UseVisualStyleBackColor = true;
            // 
            // AppButt
            // 
            this.AppButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.AppButt.Location = new System.Drawing.Point(272, 502);
            this.AppButt.Name = "AppButt";
            this.AppButt.Size = new System.Drawing.Size(51, 23);
            this.AppButt.TabIndex = 10;
            this.AppButt.Text = "App";
            this.AppButt.UseVisualStyleBackColor = true;
            // 
            // RestartButt
            // 
            this.RestartButt.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.RestartButt.Location = new System.Drawing.Point(232, 421);
            this.RestartButt.Name = "RestartButt";
            this.RestartButt.Size = new System.Drawing.Size(171, 58);
            this.RestartButt.TabIndex = 12;
            this.RestartButt.Text = Properties.Resources.Restart;
            this.RestartButt.UseVisualStyleBackColor = true;
            // 
            // IdleLbl
            // 
            this.IdleLbl.Location = new System.Drawing.Point(105, 502);
            this.IdleLbl.Name = "IdleLbl";
            this.IdleLbl.Size = new System.Drawing.Size(161, 23);
            this.IdleLbl.TabIndex = 14;
            this.IdleLbl.Text = "-- čas mimo počítač";
            this.IdleLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 537);
            this.Controls.Add(this.IdleLbl);
            this.Controls.Add(this.RestartButt);
            this.Controls.Add(this.AppButt);
            this.Controls.Add(this.LogButt);
            this.Controls.Add(this.versionLbl);
            this.Controls.Add(this.ChangeIdleTimeButt);
            this.Controls.Add(this.ChangeResetHourButt);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = Properties.Resources.AppTitle;
            this.TopMost = true;
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label IdleLbl;

        private System.Windows.Forms.Button RestartButt;

        private System.Windows.Forms.Button AppButt;

        private System.Windows.Forms.Button LogButt;

        private System.Windows.Forms.Label versionLbl;

        private System.Windows.Forms.Button ChangeIdleTimeButt;

        private System.Windows.Forms.Button ChangeResetHourButt;

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