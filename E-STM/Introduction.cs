using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E_STM
{
    public partial class Introduction : Form
    {
        public Introduction()
        {
            InitializeComponent();
        }

        private void Introduction_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.OpenForms[0].Visible = true;
            Application.OpenForms[0].Enabled = true;
        }

        private void Introduction_Load(object sender, EventArgs e)
        {
            string introduction = "";
            for (int i = 1; i < 13; i++)
            {
                introduction += (Application.OpenForms[0] as Form1).TextProg[i] + "\n";
                if (i == 1 || i == 4 || i == 5 || i == 8 || i == 10 || i == 11)
                {
                    introduction += "\n";
                }
            }
            label1.Text = introduction;
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[48];
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
