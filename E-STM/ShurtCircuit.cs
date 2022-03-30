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
    public partial class ShurtCircuit : Form
    {
        public ShurtCircuit()
        {
            InitializeComponent();
        }

        private void ShurtCircuit_Load(object sender, EventArgs e)
        {
            //if ((Application.OpenForms[0] as Form1).ShortCircuit == true)
            //{
            //    this.Close();
            //}
            (Application.OpenForms[0] as Form1).ShortCircuit = true;
        }

        private void ShurtCircuit_FormClosing(object sender, FormClosingEventArgs e)
        {
            (Application.OpenForms[0] as Form1).ShortCircuit = false;
        }
    }
}
