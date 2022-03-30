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
    public partial class SetTime : Form
    {
        public SetTime()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int result;
            if (int.TryParse(textBox1.Text, out result))
            {
                ((Application.OpenForms[0] as Form1).ElementForChangeTime as TimerOneSec).Time.Interval = result * 1000;
            }
        }

        private void SetTime_Load(object sender, EventArgs e)
        {
            textBox1.Text = (((Application.OpenForms[0] as Form1).ElementForChangeTime as TimerOneSec).Time.Interval / 1000).ToString();
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[40];
            label2.Text = (Application.OpenForms[0] as Form1).TextProg[101];
        }

        private void SetTime_FormClosed(object sender, FormClosedEventArgs e)
        {
            int result;
            if (int.TryParse(textBox1.Text, out result))
            {
                ((Application.OpenForms[0] as Form1).ElementForChangeTime as TimerOneSec).Time.Interval = result * 1000;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
