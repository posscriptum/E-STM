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
    public partial class PasswordForReport : Form
    {
        private string password = "";
        public PasswordForReport()
        {
            InitializeComponent();
        }

        private void PasswordForReport_Load(object sender, EventArgs e)
        {
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[102];
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }

        private void PasswordForReport_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (textBox1.Text == (Application.OpenForms[0] as Form1).PassForReport)
            {
                (Application.OpenForms[0] as Form1).PassToReport = true;
            }
            else
            {
                (Application.OpenForms[0] as Form1).PassToReport = false;
            }
        }
    }
}
