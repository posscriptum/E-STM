using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System;
using System.IO;
using E_STM.Properties;

namespace E_STM
{
    public partial class LoadPic : Form
    {
        OpenFileDialog OFD = new OpenFileDialog();
        public LoadPic()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {        
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(OFD.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (OFD.FileName != "")
            {
                File.Copy(OFD.FileName, Application.StartupPath + "\\" + OFD.SafeFileName, true);
                (Application.OpenForms[Application.OpenForms.Count - 2] as EditTheorQuestions).PathToPicOfQuestion = OFD.SafeFileName;
                this.Close();
            } else
            {
                MessageBox.Show("Not choise picture!");
            }
        }

        private void LoadPic_Load(object sender, EventArgs e)
        {
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[103];
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[104];
            button2.Text = (Application.OpenForms[0] as Form1).TextProg[105];
        }
    }

}
