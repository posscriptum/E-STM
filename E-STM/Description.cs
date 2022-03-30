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
    public partial class Description : Form
    {
        public Description()
        {
            InitializeComponent();
            
        }

        private void Description_Load(object sender, EventArgs e)
        {
            this.Location = new Point((Application.OpenForms[0] as Form1).Location.X + 200, (Application.OpenForms[0] as Form1).Location.Y + 100);
            (Application.OpenForms[0] as Form1).Enabled = false;
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[48];
        }

        private void Description_FormClosed(object sender, FormClosedEventArgs e)
        {
            (Application.OpenForms[0] as Form1).Enabled = true;
            (Application.OpenForms[0] as Form1).ElTestInProgress = true;
            (Application.OpenForms[0] as Form1).timerOnQuestion.Enabled = true;
        }

        private void descript_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            (Application.OpenForms[0] as Form1).label4.Text = descript.Text;
            this.Close();
        }
    }
}
