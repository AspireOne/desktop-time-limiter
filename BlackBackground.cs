using System;
using System.Drawing;
using System.Windows.Forms;

namespace Wellbeing
{
    public partial class BlackBackground : Form
    {
        private readonly Timer TopmostUpdateTimer = new()
        {
            Interval = 5000,
            Enabled = false
        };
        
        public BlackBackground()
        {
            InitializeComponent();
            TopmostUpdateTimer.Tick += OnTimerTick;
            ActiveControl = null;
            Opacity = 0.8;
            PictureBox pb = new PictureBox();
            pb.Image = Image.FromFile("C:\\Users\\matej\\Desktop\\t.png");
            pb.SizeMode = PictureBoxSizeMode.AutoSize;
            pb.Location = new Point(0, 0);
            Controls.Add(pb);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Invoke(new EventHandler((_, _) => Utils.ShowInactiveTopmost(this)));
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
                TopmostUpdateTimer.Start();
            else
                TopmostUpdateTimer.Stop();
            base.OnVisibleChanged(e);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                // Set the form click-through
                cp.ExStyle |= 0x80000 /* WS_EX_LAYERED */ | 0x20 /* WS_EX_TRANSPARENT */ | 0x80 /*Turn on WS_EX_TOOLWINDOW*/;
                return cp;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            
            base.OnFormClosing(e);
        }
    }
}