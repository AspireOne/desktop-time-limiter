using System.Drawing;
using System.Windows.Forms;

namespace Wellbeing
{
    public partial class TimeDialog : Form
    {
        public TimeDialog()
        {
            InitializeComponent();
            Controls[0].Visible = false;
            var cancelButt = new Button
            {
                Size = new Size(0, 0),
                TabStop = false
            };
            Controls.Add(cancelButt);
            CancelButton = cancelButt;
            
            OkButt.Click += (_, _) =>
            {
                DialogResult = DialogResult.OK;
                Close();
            };
            AcceptButton = OkButt;
        }
    }
}