using System;
using System.Windows.Forms;

namespace Brainfuck_Interpreter
{
    public partial class Exiting : Form
    {
        public int pos;

        public Exiting()
        {
            InitializeComponent();
            pos = -1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            pos = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
            pos = 1;
        }
    }
}
