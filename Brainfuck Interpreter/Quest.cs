using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brainfuck_Interpreter
{
    public partial class Quest : Form
    {
        public int pos;

        public Quest(string text)
        {
            InitializeComponent();
            pos = -1;
            label1.Text =  $"Что {text}?";
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

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
