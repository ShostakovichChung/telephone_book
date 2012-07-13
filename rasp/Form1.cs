using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace rasp
{
    public partial class Form1 : Form
    {
        private List<List<string>> DATA = new List<List<string>>();

        public Form1()
        {
            InitializeComponent();
        }

        TOOL tool = new TOOL();

        private void Form1_Load(object sender, EventArgs e)
        {
            DATA = tool.readFile();
            dataGridView1.Rows.Clear();
            foreach (List<string> i in DATA)
            {
                dataGridView1.Rows.Add(i[3], i[0], i[1], i[2]);
            }
            dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<List<string>> tmp = new List<List<string>>();
            tmp = tool.findLeads(textBox1.Text, textBox2.Text, textBox3.Text);
            dataGridView1.Rows.Clear();
            foreach (List<string> i in tmp)
            {
                dataGridView1.Rows.Add(i[3], i[0], i[1], i[2]);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string error = tool.appendLead(textBox4.Text, textBox5.Text, textBox6.Text);
            if (error != "OK")
            {
                MessageBox.Show(error);
            }
            DATA = tool.readFile();
            dataGridView1.Rows.Clear();
            foreach (List<string> i in DATA)
            {
                dataGridView1.Rows.Add(i[3], i[0], i[1], i[2]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DATA = tool.readFile();
            dataGridView1.Rows.Clear();
            foreach (List<string> i in DATA)
            {
                dataGridView1.Rows.Add(i[3], i[0], i[1], i[2]);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Point point = Form1.MousePosition;
            if (e.Button.ToString() == "Right")
            {
                tool.index = e.RowIndex;
                contextMenuStrip1.Show(point);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DATA.RemoveAt(tool.index);
            tool.rewriteLeads();
            DATA = tool.readFile();
            dataGridView1.Rows.Clear();
            foreach (List<string> i in DATA)
            {
                dataGridView1.Rows.Add(i[3], i[0], i[1], i[2]);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tool.index >= 0 && tool.index <= tool.DATA.Count - 1)
            {
                Form3 form3 = new Form3(tool);
                form3.ShowDialog(this);
            }
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

    }
    public class TOOL
    {
        public List<List<string>> DATA = new List<List<string>>();
        const string leadsFile = "data.txt";
        public int index = 0;

        public List<List<string>> readFile()
        {
            int I = 0;
            DATA.Clear();
            StreamReader streamReader = new StreamReader(leadsFile);
            string str = "";

            while (!streamReader.EndOfStream)
            {
                str = streamReader.ReadLine();
                char[] S = new Char[] { ' ' };
                
                
                List<string> mas = new List<string>(str.Split(new Char[] { ' ','e','s' }));
                mas.Add(I.ToString());
                DATA.Add(mas);
                I += 1;
            }
            streamReader.Close();
            return DATA;
        }

        public List<List<string>> findLeads(string name, string lastname, string phone)
        {
            List<List<string>> Data = new List<List<string>>();
            foreach (List<string> i in DATA)
            {
                string patternName = @"" + name + "";
                string patternLastName = @"" + lastname + "";
                string patternPhone = @"" + phone + "";

                Regex regex1 = new Regex(patternName);
                Match match1 = regex1.Match(i[0]);

                Regex regex2 = new Regex(patternLastName);
                Match match2 = regex2.Match(i[1]);

                Regex regex3 = new Regex(patternPhone);
                Match match3 = regex3.Match(i[2]);

                if (match1.Success)
                {
                    if (match2.Success)
                    {
                        if (match3.Success)
                        {
                            List<string> tmp = new List<string>();
                            tmp.Add(i[0]);
                            tmp.Add(i[1]);
                            tmp.Add(i[2]);
                            tmp.Add(i[3]);

                            Data.Add(tmp);
                        }
                    }
                }

            }
            return Data;
        }

        public string appendLead(string name, string lastname, string phone)
        {
            Regex regex = new Regex(@"^\d*$");
            bool res = regex.IsMatch(phone);

            string error = "";
            if (res)
            {

                if (name != "" && lastname != "")
                {
                    bool flag = true;
                    foreach (List<string> i in DATA)
                    {
                        if (i[0] == name && i[1] == lastname && i[2] == phone)
                        {
                            flag = false;
                            error = "Извините, но контакт с такими данными уже существует!";
                            break;
                        }
                    }

                    if (flag)
                    {
                        StreamWriter sw = new StreamWriter(leadsFile, true);
                        sw.WriteLine(name + " " + lastname + " " + phone);
                        sw.Close();
                        return "OK";
                    }
                    else
                    {
                        return error;
                    }
                }
                else
                {
                    return "Введите все поля контакта!";
                }
            }
            else
            {
                return "В поле телефон должны быть только цифры!";
            }

        }
        public void rewriteLeads()
        {
            StreamWriter sr = new StreamWriter(leadsFile, false);
            foreach (List<string> i in DATA)
            {
                sr.WriteLine(i[0] + " " + i[1] + " " + i[2]);
            }
            sr.Close();
        }

        public string editLead(string name, string lastname, string phone)
        {
            Regex regex = new Regex(@"^\d*$");
            bool res = regex.IsMatch(phone);

            if (res)
            {

                if (name != "" && lastname != "")
                {
                    DATA[index][0] = name;
                    DATA[index][1] = lastname;
                    DATA[index][2] = phone;

                    return "OK";
                }
                else
                {
                    return "Введите все поля контакта!";
                }
            }
            else
            {
                return "В поле телефон должны быть только цифры!";
            }
        }
    }
}