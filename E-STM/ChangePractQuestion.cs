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

namespace E_STM
{
    public partial class ChangePractQuestion : Form
    {
        OleDbConnection dbc;
        string lang1, lang2;
        public ChangePractQuestion()
        {
            InitializeComponent();
        }

        private void GhangePractQuestion_Load(object sender, EventArgs e)
        {
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[116];
            button4.Text = (Application.OpenForms[0] as Form1).TextProg[117];
            button2.Text = (Application.OpenForms[0] as Form1).TextProg[118];
            button3.Text = (Application.OpenForms[0] as Form1).TextProg[119];
            dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
            dbc.Open();
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT Languidge FROM Languidge", dbc);
            adapter1.Fill(table1);
            DataRow[] RowsText;
            RowsText = table1.Select();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Items.Add(RowsText[0].ItemArray[0].ToString());
            for (int i = 1; i < table1.Rows.Count; i++)
            {               
                comboBox2.Items.Add(RowsText[i].ItemArray[0].ToString());
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            readFromDB();
            dataGridView1.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            readFromDB();
        }

        private void readFromDB()
        {
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT Practical FROM Languidge WHERE Languidge = '" + comboBox1.SelectedItem.ToString() + "'", dbc);
            adapter.Fill(table);
            DataRow[] foundRows;
            foundRows = table.Select();
            lang1 = foundRows[0].ItemArray[0].ToString();

            table.Clear();
            adapter = new OleDbDataAdapter("SELECT Practical FROM Languidge WHERE Languidge = '" + comboBox2.SelectedItem.ToString() + "'", dbc);
            adapter.Fill(table);
            foundRows = table.Select();
            lang2 = foundRows[0].ItemArray[0].ToString();

            //DataTable ds = new DataTable();
            if (lang1 == lang2)
            {
                MessageBox.Show("Selected equal languages!");
                return;
            }
            DataTable table2 = new DataTable();
            adapter = new OleDbDataAdapter("SELECT " + lang1 + ", " + lang2 + " FROM Practical", dbc);
            adapter.Fill(table2);

            dataGridView1.DataSource = table2;
            dataGridView1.Columns[0].Width = (dataGridView1.Width / 2) - 41;
            dataGridView1.Columns[1].Width = (dataGridView1.Width / 2) - 2;
            dataGridView1.Update();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            label2.Text = comboBox2.SelectedItem.ToString();
            readFromDB();
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();
            textBox2.Text = dataGridView1[1, dataGridView1.CurrentRow.Index].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // проверить, нет-ли такого вопроса в базе
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT " + lang1 + " FROM Practical", dbc);
            adapter.Fill(table);
            DataRow[] foundRows;
            foundRows = table.Select();
            if (textBox1.Text == "")
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[120]);
                return;
            }
            foreach(DataRow row in foundRows)
            {
                if (textBox1.Text == row.ItemArray[0].ToString())
                {
                    MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[121]);
                    return;
                }
            }

            // добавить вопрос в базу
            OleDbCommand command = dbc.CreateCommand();
            command.CommandText = "INSERT INTO [Practical](" + lang1 + ") VALUES('" + textBox1.Text + "');";
            command.ExecuteNonQuery();
            if (textBox2.Text != "")
            {
                command.CommandText = "UPDATE [Practical] SET " + lang2 + "='" + textBox2.Text + "' WHERE " + lang1 + "='" + textBox1.Text + "'";
                command.ExecuteNonQuery();
            }
            // заполнить пустые поля английским вариантом
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT Practical FROM Languidge", dbc);
            adapter1.Fill(table1);
            DataRow[] foundRows1;
            foundRows1 = table1.Select();
            foreach (DataRow row in foundRows1)
            {
                DataTable table4 = new DataTable();
                OleDbDataAdapter adapter4 = new OleDbDataAdapter("SELECT " + row.ItemArray[0].ToString() + " FROM Practical WHERE practical_question_en = '" + textBox1.Text + "'", dbc);
                adapter4.Fill(table4);
                DataRow[] foundRows4 = table4.Select();
                string text = foundRows4[0].ItemArray[0].ToString();
                if (text == "")
                {
                    command.CommandText = "UPDATE [Practical] SET " + row.ItemArray[0].ToString() + "='" + textBox1.Text + "' WHERE " + lang1 + "='" + textBox1.Text + "'";
                    command.ExecuteNonQuery();
                }
            }
            readFromDB();
        }

        // кнопка Update
        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[124], "Update", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            // проверить, есть-ли такой вопрос в базе
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT " + lang1 + " FROM Practical", dbc);
            adapter.Fill(table);
            DataRow[] foundRows;
            foundRows = table.Select();
            if (textBox1.Text == "")
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[120]);
                return;
            }
            bool questionIsInBase = false;
            foreach (DataRow row in foundRows)
            {
                if (textBox1.Text == row.ItemArray[0].ToString())
                {
                    questionIsInBase = true;
                }
            }
            if (!questionIsInBase)
            {
                MessageBox.Show("Fiedl isn't exist in Base.");
                return;
            }

            // обновить вопрос в базе
            OleDbCommand command = dbc.CreateCommand();
            if (textBox2.Text != "")
            {
                command.CommandText = "UPDATE [Practical] SET " + lang2 + "='" + textBox2.Text + "' WHERE " + lang1 + "='" + textBox1.Text + "'";
                try
                {
                    command.ExecuteNonQuery();
                    MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[147]);
                }
                catch(Exception e2)
                {
                    MessageBox.Show("Error: " + e2.Message);
                }
            }
            readFromDB();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[122], "Delete", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            // проверить, есть-ли такой вопрос в базе
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT " + lang1 + " FROM Practical", dbc);
            adapter.Fill(table);
            DataRow[] foundRows;
            foundRows = table.Select();
            if (textBox1.Text == "")
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[120]);
                return;
            }
            bool questionIsInBase = false;
            foreach (DataRow row in foundRows)
            {
                if (textBox1.Text == row.ItemArray[0].ToString())
                {
                    questionIsInBase = true;
                }
            }
            if (!questionIsInBase)
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[123]);
                return;
            }

            // и если есть, то вопрос удалить из базы
            OleDbCommand command = dbc.CreateCommand();
            command.CommandText = "DELETE FROM Practical WHERE " + lang1 + "='" + textBox1.Text + "'";
            command.ExecuteNonQuery();
            readFromDB();
        }

        private void GhangePractQuestion_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbc.Close();
        }
    }
}
