using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brainfuck_Interpreter
{
    public partial class Brainfuck : Form
    {
        BF Inter;
        Quest quest;

        bool haspathc, haspathi, again;
        bool changemadec, changemadei;
        string wasopenpathc = "", wasopenpathi = "";

        public Brainfuck()
        {
            InitializeComponent();
            BoxDraw();
            changemadec = false;
            changemadei = false;
            haspathc = false;
            haspathi = false;
            again = false;
            Inter = new BF();
        }

        private void Brainfuck_SizeChanged(object sender, EventArgs e)
        {
            BoxDraw();
        }

        void BoxDraw()
        {
            label2.Visible = окноВводаToolStripMenuItem.Checked;
            panel2.Visible = окноВводаToolStripMenuItem.Checked;
            InputBox.Visible = окноВводаToolStripMenuItem.Checked;

            label3.Visible = окноВыводаToolStripMenuItem.Checked;
            panel3.Visible = окноВыводаToolStripMenuItem.Checked;
            OutBox.Visible = окноВыводаToolStripMenuItem.Checked;
            if (окноВводаToolStripMenuItem.Checked || окноВыводаToolStripMenuItem.Checked)
            {
                panel1.Size = new Size(panel1.Width, Height - 248);
                panel2.Size = new Size((Width - 44) / 2, panel2.Height);

                label2.Left = panel2.Width / 2 - label2.Width / 2 + panel2.Location.X;
            }

            if(окноВводаToolStripMenuItem.Checked && окноВыводаToolStripMenuItem.Checked)
            {
                panel3.Size = new Size((Width - 44) / 2, panel3.Height);
                panel3.Location = new Point(panel2.Width + 18, panel3.Location.Y);

                label3.Left = panel3.Width / 2 - label3.Width / 2 + panel2.Width + 18;
            }
            else if(!окноВводаToolStripMenuItem.Checked && окноВыводаToolStripMenuItem.Checked)
            {
                panel3.Size = new Size(2 * panel2.Width + 6, panel3.Height);
                panel3.Location = new Point(panel2.Location.X, panel3.Location.Y);

                label3.Left = Width / 2 - label3.Width / 2;
            }
            else if (окноВводаToolStripMenuItem.Checked && !окноВыводаToolStripMenuItem.Checked)
            {
                panel2.Size = new Size(2 * panel2.Width + 6, panel2.Height);

                label2.Left = Width / 2 - label2.Width / 2;
            }
            else if (!окноВводаToolStripMenuItem.Checked && !окноВыводаToolStripMenuItem.Checked)
            {
                panel1.Size = new Size(panel1.Width, Height - 248 + 151);
            }

            InputBox.Size = new Size(panel2.Width - 10, InputBox.Height);
            OutBox.Size = new Size(panel3.Width - 10, OutBox.Height);

            label1.Left = Width / 2 - label1.Width / 2;
            
            if(panel1.Width == 0)
            {
                return;
            }
            if (haspathc && !again)
            {
                label1.Text = Remaker(wasopenpathc) + " - Окно кода";
            }

            if (haspathi && !again)
            {
                label2.Text = Remaker(wasopenpathi) + " - Окно ввода";
            }

            if (label1.Width > panel1.Width || label2.Width > panel2.Width)
            {
                if (label1.Width > panel1.Width)
                {
                    LabelRemaker(ref label1, panel1.Width - 100, Remaker(wasopenpathc), " - Окно кода");
                }
                if (label2.Width > panel2.Width)
                {
                    LabelRemaker(ref label2, panel2.Width - 100, Remaker(wasopenpathi), " - Окно ввода");
                }
                again = true;
                BoxDraw();
            }
            again = false;
        }

        private void окноВводаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            окноВводаToolStripMenuItem.Checked = !окноВводаToolStripMenuItem.Checked;
            BoxDraw();
        }

        private void окноВыводаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            окноВыводаToolStripMenuItem.Checked = !окноВыводаToolStripMenuItem.Checked;
            BoxDraw();
            BoxDraw();
        }

        private void CodeBox_TextChanged(object sender, EventArgs e)
        {
            changemadec = true;
            сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
        }

        private void InputBox_TextChanged(object sender, EventArgs e)
        {
            changemadei = true;
            сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (button3.Enabled)
            {
                Exiting save = new Exiting();
                save.ShowDialog();
                if (save.pos == 0)
                {
                    сохранитьToolStripMenuItem_Click(sender, e);
                }
            }

            haspathc = false;
            haspathi = false;
            CodeBox.Clear();
            InputBox.Clear();
            OutBox.Clear();
            changemadei = false;
            changemadec = false;
            сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
            label1.Text = "Окно кода";
            label2.Text = "Окно ввода";
            BoxDraw();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quest = new Quest("открыть");
            quest.ShowDialog();
            if(quest.pos == 0)
            {
                this.openFileDialog1.Filter = "Brainfuck files code|*.bfc";
                openFileDialog1.Title = "Открыть код программы";
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    openFileDialog1.FileName = "";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs = new FileStream(openFileDialog1.FileName.ToString(), FileMode.Open, FileAccess.Read);
                        Save save = (Save)bf.Deserialize(fs);
                        fs.Close();
                        wasopenpathc = openFileDialog1.FileName.ToString();
                        CodeBox.Lines = save.text;
                        changemadec = false;
                        сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
                        label1.Text = Remaker(wasopenpathc) + " - Окно кода";
                        haspathc = true;
                    }
                }
                catch
                {
                    MessageBox.Show("К сожалению, программа не может открыть этот файл!");
                }
            }
            else if(quest.pos == 1)
            {
                this.openFileDialog1.Filter = "Brainfuck files input|*.bfi";
                openFileDialog1.Title = "Открыть входные данные";
                try
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    openFileDialog1.FileName = "";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs = new FileStream(openFileDialog1.FileName.ToString(), FileMode.Open, FileAccess.Read);
                        Save save = (Save)bf.Deserialize(fs);
                        fs.Close();
                        wasopenpathi = openFileDialog1.FileName.ToString();
                        InputBox.Lines = save.text;
                        changemadei = false;
                        сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
                        label2.Text = Remaker(wasopenpathi) + " - Окно ввода";
                        haspathi = true;
                    }
                }
                catch
                {
                    MessageBox.Show("К сожалению, программа не может открыть этот файл!");
                }
            }
            BoxDraw();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haspathc)
            {
                BinaryFormatter bf = new BinaryFormatter();
                Save save = new Save();
                save.text = CodeBox.Lines;
                FileStream fs = new FileStream(wasopenpathc, FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, save);
                fs.Close();
                changemadec = false;
                сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
            }
            else
            {
                SaveCode();
            }

            if (haspathi)
            {
                BinaryFormatter bf = new BinaryFormatter();
                Save save = new Save();
                save.text = InputBox.Lines;
                FileStream fs = new FileStream(wasopenpathi, FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, save);
                fs.Close();
                changemadei = false;
                сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
            }
            else
            {
                SaveInput();
            }
            BoxDraw();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            quest = new Quest("сохранить");
            quest.ShowDialog();
            if (quest.pos == 0)
            {
                SaveCode();
            }
            else if (quest.pos == 1)
            {
                SaveInput();
            }
            BoxDraw();
        }

        private void запуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start();
        }

        void Start()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = true;
            menuStrip1.Enabled = false;
            string code = "", input = "";
            OutBox.Text = "";
            for (int i = 0; i < CodeBox.Lines.Count(); i++)
            {
                code += CodeBox.Lines[i];
            }

            for (int i = 0; i < InputBox.Lines.Count(); i++)
            {
                input += InputBox.Lines[i];
            }


            Inter.Code = code;
            Inter.Input = input;

            Inter.Solving();

            if (Inter.Warning)
            {
                if (!Inter.Ok)
                {
                    MessageBox.Show("Не хватает входных данных!", "Ошибка!");
                    //return;
                }
                else
                {
                    MessageBox.Show("Входные данные до конца не считываются!", "Предупреждение!");
                }
            }

            OutBox.Text = Inter.Result;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = false;
            menuStrip1.Enabled = true;
        }

        private void сохранитьToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            button3.Enabled = сохранитьToolStripMenuItem.Enabled;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            новыйToolStripMenuItem_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            открытьToolStripMenuItem_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            сохранитьToolStripMenuItem_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            сохранитьКакToolStripMenuItem_Click(sender, e);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Brainfuck_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(button3.Enabled)
            {
                Exiting save = new Exiting();
                save.ShowDialog();
                if(save.pos == 0)
                {
                    сохранитьToolStripMenuItem_Click(sender, e);
                }
                else if (save.pos == -1)
                {
                    e.Cancel = true;
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Stop();
        }

        void Stop()
        {
            Inter.Stop();
            MessageBox.Show("Программа остановлена");
        }

        string Remaker(string a)
        {
            string b = "", c = "";
            for(int i = a.Length - 1; i >= 0 && a[i] != '\\'; i--)
            {
                b += a[i];
            }

            for(int i = b.Length - 1; i >= 0; i--)
            {
                c += b[i];
            }

            return c;
        }

        void SaveCode()
        {
            this.saveFileDialog1.Filter = "Brainfuck files code|*.bfc";
            BinaryFormatter bf = new BinaryFormatter();
            saveFileDialog1.Title = "Сохранить как код программы";
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Save save = new Save();
                save.text = CodeBox.Lines;
                FileStream fs = new FileStream(saveFileDialog1.FileName.ToString(), FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, save);
                fs.Close();
                changemadec = false;
                сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
                wasopenpathc = saveFileDialog1.FileName.ToString();
                label1.Text = Remaker(wasopenpathc) + " - Окно кода";
                haspathc = true;
            }
        }

        void SaveInput()
        {
            this.saveFileDialog1.Filter = "Brainfuck files input|*.bfi";
            BinaryFormatter bf = new BinaryFormatter();
            saveFileDialog1.Title = "Сохранить как входные данные";
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Save save = new Save();
                save.text = CodeBox.Lines;
                FileStream fs = new FileStream(saveFileDialog1.FileName.ToString(), FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, save);
                fs.Close();
                changemadei = false;
                сохранитьToolStripMenuItem.Enabled = changemadec | changemadei;
                wasopenpathi = saveFileDialog1.FileName.ToString();
                label2.Text = Remaker(wasopenpathi) + " - Окно ввода";
                haspathi = true;
            }
        }

        void LabelRemaker(ref Label a, int size, string path, string name)
        {
            int back = 9;
            string newpath = "";
            for(int i = 0; i < path.Length; i++)
            {
                if(i == path.Length - 8 || i == path.Length - 7 || i == path.Length - 6)
                {
                    newpath += '.';
                }
                else
                {
                    newpath += path[i];
                }
            }
            path = newpath;
            temp.Text = path + name;
            while (temp.Width >= size)
            {
                newpath = "";
                for (int i = 0; i < path.Length; i++)
                {
                    if (i <= path.Length - back && i >= path.Length - back - 5)
                    {
                        continue;
                    }
                    else
                    {
                        newpath += path[i];
                    }
                }
                path = newpath;
                temp.Text = path + name;
            }
            a.Text = path + name;
        }
    }
}
