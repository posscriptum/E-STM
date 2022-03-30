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
    public partial class DeleteAllElements : Form
    {
        public DeleteAllElements()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void DeleteAllElements_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            Form form = Application.OpenForms[0] as Form1;
            this.Location = new Point(form.Location.X + (form.Width/2), form.Location.Y + (form.Height / 8));
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[106];
            button2.Text = (Application.OpenForms[0] as Form1).TextProg[49];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (Application.OpenForms[0] as Form1).deleteAllFromPanel();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
