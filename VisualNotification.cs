using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wellbeing.Properties;

namespace Wellbeing;

public partial class VisualNotification : Form
{
    private readonly FadeAnimation FadeState;
    public int NotificationDurationSecs { get; set; } = 6;

    public VisualNotification()
    {
        InitializeComponent();
        Opacity = 0;
        FadeState = new FadeAnimation(this) {MaxOpacity = 0.8};
        Location = new Point(
            (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2,
            (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2);
    }

    public void ShowNotification(string content)
    {
        NotificationTextLbl.Text = content;
        FadeState.StartFade(FadeAnimation.Fades.In, () =>
        {
            Utils.ShowInactiveTopmost(this);
            Task.Delay(TimeSpan.FromSeconds(NotificationDurationSecs))
                .ContinueWith(_ =>
                {
                    // It has to be called from the UI thread. 
                    NotificationTextLbl.Invoke((MethodInvoker)delegate {
                        FadeState.StartFade(FadeAnimation.Fades.Out); 
                    });
                });
        });
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

    private void label1_Click(object sender, EventArgs e)
    {
        
    }
}