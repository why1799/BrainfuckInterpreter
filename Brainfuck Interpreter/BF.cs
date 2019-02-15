using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brainfuck_Interpreter
{
    class BF
    {
        string code, output, input;
        bool ok, warning, stop;

        public void Solving()
        {
            stop = false;
            ok = true;
            warning = false;
            output = "";
            //char[] cpu = new char[30000]; //Лента

            List<char> cpu = new List<char>();

            int j = 0, k = 0;
            cpu.Add((char)0);
            int brc = 0;
            for (int i = 0; !stop && i < code.Length; ++i)
            {
                Application.DoEvents();
                switch (code[i])
                {
                    case '>': j++;
                        {
                            if(j == cpu.Count)
                            {
                                cpu.Add((char)0);
                            }
                            break;
                        }
                    case '<': j--;
                        {
                            if (j == -1)
                            {
                                cpu.Insert(0, (char)0);
                                j++;
                            }
                            break;
                        }
                    case '+': cpu[j]++; break;
                    case '-': cpu[j]--; break;
                    case '.': output += cpu[j]; break;
                    case ',':
                        {
                            if(k >= input.Length)
                            {
                                cpu[j] = (char)0;
                                ok = false;
                                warning = true;
                                break;
                                //return;
                            }
                            cpu[j] = input[k];
                            k++;
                            break;
                        }
                    case '[':
                        {
                            if (cpu[j] == 0)
                            {
                                ++brc;
                                while (brc != 0)
                                {
                                    ++i;
                                    if (code[i] == '[') ++brc;
                                    if (code[i] == ']') --brc;
                                }
                            }
                            else continue;
                            break;
                        }
                    case ']':
                        {
                            if (cpu[j] == 0)
                            {
                                continue;
                            }
                            else
                            {
                                if (code[i] == ']') brc++;
                                while (brc != 0)
                                {
                                    --i;
                                    if (code[i] == '[') brc--;
                                    if (code[i] == ']') brc++;
                                }
                                --i;
                            }
                            break;
                        }
                    default: break;
                }
            }
            if (k != input.Length)
            {
                warning = true;
            }
        }

        public string Code
        {
            set
            {
                code = value;
            }
            get
            {
                return code;
            }
        }

        public string Result
        {
            get
            {
                return output;
            }
        }

        public string Input
        {
            set
            {
                input = value;
            }
            get
            {
                return input;
            }
        }

        public bool Ok
        {
            get
            {
                return ok;
            }
        }

        public bool Warning
        {
            get
            {
                return warning;
            }
        }

        public void Stop()
        {
            stop = true;
        }
    }
}
