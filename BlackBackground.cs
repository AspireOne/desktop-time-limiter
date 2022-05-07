using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Digital_wellbeing
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
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            Invoke(new EventHandler((_, _) => ShowInactiveTopmost(this)));
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
                cp.ExStyle |= 0x80000 /* WS_EX_LAYERED */ | 0x20 /* WS_EX_TRANSPARENT */;
                return cp;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
            
            base.OnFormClosing(e);
        }
        
        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(
            int hWnd,             // Window handle
            int hWndInsertAfter,  // Placement-order handle
            int x,                // Horizontal position
            int y,                // Vertical position
            int cx,               // Width
            int cy,               // Height
            uint uFlags);         // Window positioning flags

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private static void ShowInactiveTopmost(Form frm)
        {
            try
            {
                ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
                SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, 0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, SWP_NOACTIVATE);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Could not set overlay topmost.");
            }
        }
    }
}