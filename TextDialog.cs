using System.Drawing;
using System.Windows.Forms;

namespace Digital_wellbeing
{
    public partial class TextDialog : Form
    {
        public TextDialog()
        {
            InitializeComponent();
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