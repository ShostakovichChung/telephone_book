using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rasp
{
    public partial class Form3 : Form
    {

        public Form3(TOOL Form1_tool)
        {
            InitializeComponent();
            tool = Form1_tool;
            textBox1.Text = tool.DATA[tool.index][0];
            textBox2.Text = tool.DATA[tool.index][1];
            textBox3.Text = tool.DATA[tool.index][2];
        }
        TOOL tool;

        private void button1_Click(object sender, EventArgs e)
        {
            string error = tool.editLead(textBox1.Text, textBox2.Text, textBox3.Text);
            if (error != "OK")
            {
                MessageBox.Show(error);
            }
            else
            {
                //List<List<string>> tmp = new List<List<string>>(); 
                tool.rewriteLeads();
                tool.readFile();

                Form1 frm1 = this.Owner as Form1;
                frm1.dataGridView1.Rows.Clear();

                foreach (List<string> i in tool.DATA)
                {
                    frm1.dataGridView1.Rows.Add(i[3], i[0], i[1], i[2]);
                }
                this.Hide();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
