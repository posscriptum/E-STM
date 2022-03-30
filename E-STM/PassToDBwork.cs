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
    public partial class PassToDBwork : Form
    {
        private string password = "";
        public PassToDBwork()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
            //if (textBox1.Text.Length > password.Length)
            //{
            //     password += textBox1.Text.Substring(textBox1.Text.Length - 1);
            //}
        }

        private void PassToDBwork_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (textBox1.Text == "secret")
            {
                (Application.OpenForms[0] as Form1).groupBoxDBwork.Enabled = true;
            }
        }

        private void PassToDBwork_Load(object sender, EventArgs e)
        {
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[102];
        }
    }
}
