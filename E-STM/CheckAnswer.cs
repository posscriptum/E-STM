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
    public partial class CheckAnswer : Form
    {
        public CheckAnswer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //(Application.OpenForms[0] as Form1).Enabled = true;
            //(Application.OpenForms[0] as Form1).NextQuestion();
            ////(Application.OpenForms[0] as Form1).button7.Enabled = false;
            //(Application.OpenForms[0] as Form1).button10.Enabled = true;
            //(Application.OpenForms[0] as Form1).button11.Enabled = false;
            //(Application.OpenForms[0] as Form1).timerOnQuestion.Enabled = false;
            //(Application.OpenForms[0] as Form1).CalculateSignal.Enabled = false;
            //(Application.OpenForms[0] as Form1).panelPractical.Enabled = false;
            //(Application.OpenForms[0] as Form1).groupBox3.Enabled = false;
            //(Application.OpenForms[0] as Form1).label4.Text = "";
            (Application.OpenForms[0] as Form1).PushedNext = true;
            //this.Visible = false;
            this.Close();
        }

        private void CheckAnswer_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Application.OpenForms[0] as Form1).Location.X + 200, (Application.OpenForms[0] as Form1).Location.Y + 100);
            (Application.OpenForms[0] as Form1).Enabled = false;
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[46] + "\n" + (Application.OpenForms[0] as Form1).TextProg[47];
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[48];
            button2.Text = (Application.OpenForms[0] as Form1).TextProg[49];
        }

        private void CheckAnswer_FormClosed(object sender, FormClosedEventArgs e)
        {
            (Application.OpenForms[0] as Form1).Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            (Application.OpenForms[0] as Form1).Enabled = true;
            (Application.OpenForms[0] as Form1).BringToFront();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
