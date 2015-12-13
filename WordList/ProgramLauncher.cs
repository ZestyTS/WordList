using System;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmProgramLauncher : Form
    {
        public frmProgramLauncher()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void frmProgramLauncher_Load(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                var WordList = new frmWordList();
                WordList.Show();
                Hide();
            }));             
        }
    }
}
