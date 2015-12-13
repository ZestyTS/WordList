using System;
using System.Windows.Forms;

namespace WordList
{
    public partial class FrmProgramLauncher : Form
    {
        public FrmProgramLauncher()
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
                var wordList = new FrmWordList();
                wordList.Show();
                Hide();
            }));             
        }
    }
}
