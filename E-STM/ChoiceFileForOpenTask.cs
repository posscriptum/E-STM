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
    public partial class ChoiceFileForOpenTask : Form
    {
        public ChoiceFileForOpenTask()
        {
            InitializeComponent();
        }

        private void ChoiceFileForOpenTask_Load(object sender, EventArgs e)
        {
            //comboBoxChoiceFileTasks.Items.AddRange((Application.OpenForms[0] as Form1).fliesnameOfTasks.ToArray());

            var listOfFiles = (Application.OpenForms[0] as Form1).fliesnameOfTasks;
            foreach(var name in listOfFiles)
            {
                var arrayFilename = name.Split(new char[] { '.' });
                var midStr = arrayFilename[arrayFilename.Length - 2];
                int num = int.Parse(midStr.Replace("\\Tasks\\Task", ""));
                comboBoxChoiceFileTasks.Items.Add((Application.OpenForms[0] as Form1).listAllPracticalQuestion.Select(p => p).Where(p => p.id == num).ToList()[0].name);
            }

            
        }

        private void comboBoxChoiceFileTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            var one = (Application.OpenForms[0] as Form1).listAllPracticalQuestion;
            var a = comboBoxChoiceFileTasks.SelectedItem.ToString();
            var two = one.Select(p => p).Where(p => p.name.Equals(a));
            (Application.OpenForms[0] as Form1).pathToTaskFile = two.ToList()[0].nameTask; //comboBoxChoiceFileTasks.SelectedItem.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if ((Application.OpenForms[0] as Form1).pathToTaskFile != null)
            //{
                this.Close();
            //}
        }

        private void ChoiceFileForOpenTask_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if ((Application.OpenForms[0] as Form1).pathToTaskFile == null)
            //{
            //    ChoiceFileForOpenTask t = new ChoiceFileForOpenTask();
            //    this.Visible = false;
            //    t.ShowDialog();   
            //}
        }
    }
}
