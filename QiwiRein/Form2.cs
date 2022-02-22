using System;
using System.Windows.Forms;

namespace QiwiRein
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            Program.buff = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ViewTokens viewTokens = new ViewTokens();
            viewTokens.Show();
        }
    }
}
