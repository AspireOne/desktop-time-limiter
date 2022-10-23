using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Wellbeing;

partial class VisualNotification
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
        this.NotificationTitleLbl = new System.Windows.Forms.Label();
        this.HorizontalLine = new System.Windows.Forms.Label();
        this.NotificationTextLbl = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // NotificationTitleLbl
        // 
        this.NotificationTitleLbl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.NotificationTitleLbl.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.NotificationTitleLbl.ForeColor = System.Drawing.Color.Gainsboro;
        this.NotificationTitleLbl.Location = new System.Drawing.Point(12, 17);
        this.NotificationTitleLbl.Name = "NotificationTitleLbl";
        this.NotificationTitleLbl.Size = new System.Drawing.Size(342, 23);
        this.NotificationTitleLbl.TabIndex = 1;
        this.NotificationTitleLbl.Text = "Digital Wellbeing Notification";
        this.NotificationTitleLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // HorizontalLine
        // 
        this.HorizontalLine.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
        this.HorizontalLine.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        this.HorizontalLine.Location = new System.Drawing.Point(0, 40);
        this.HorizontalLine.Name = "HorizontalLine";
        this.HorizontalLine.Size = new System.Drawing.Size(400, 1);
        this.HorizontalLine.TabIndex = 2;
        this.HorizontalLine.Text = "label3";
        // 
        // NotificationTextLbl
        // 
        this.NotificationTextLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
        this.NotificationTextLbl.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
        this.NotificationTextLbl.ForeColor = System.Drawing.Color.Gainsboro;
        this.NotificationTextLbl.Location = new System.Drawing.Point(22, 60);
        this.NotificationTextLbl.Name = "NotificationTextLbl";
        this.NotificationTextLbl.Size = new System.Drawing.Size(320, 81);
        this.NotificationTextLbl.TabIndex = 3;
        this.NotificationTextLbl.Text = "Digital Wellbeing Notification";
        this.NotificationTextLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // VisualNotification
        // 
        this.BackColor = System.Drawing.Color.Black;
        this.ClientSize = new System.Drawing.Size(366, 162);
        this.Controls.Add(this.NotificationTextLbl);
        this.Controls.Add(this.HorizontalLine);
        this.Controls.Add(this.NotificationTitleLbl);
        this.ForeColor = System.Drawing.Color.White;
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.MaximumSize = new System.Drawing.Size(382, 187);
        this.Name = "VisualNotification";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.TopMost = true;
        this.TransparencyKey = System.Drawing.Color.Transparent;
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Label NotificationTextLbl;

    private System.Windows.Forms.Label HorizontalLine;

    private System.Windows.Forms.Label NotificationTitleLbl;

    #endregion
}