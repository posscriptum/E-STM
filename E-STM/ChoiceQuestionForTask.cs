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
    public partial class ChoiceQuestionForTask : Form
    {
        public ChoiceQuestionForTask()
        {
            InitializeComponent();
        }

        private void ChoiceQuestionForTask_Load(object sender, EventArgs e)
        {     
            foreach (var item in (Application.OpenForms[0] as Form1).listAllPracticalQuestion)
            {
                SelectTaskComboBox.Items.Add(item.name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var v = (Application.OpenForms[0] as Form1).listAllPracticalQuestion.Where(p => p.name.Equals(SelectTaskComboBox.SelectedItem.ToString())).ToList();
            (Application.OpenForms[0] as Form1).returnedNumberSelectedQuestion = v[0].id;
            // записать в listAllPracticalQuestion путь к файлу для данного вопроса
            v[0].nameTask = @".\Tasks\Task" + v[0].id.ToString() + @".xml";
            this.Close();
        }
    }
}
