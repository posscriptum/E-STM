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
    public partial class EditLanguage : Form
    {
        OleDbConnection dbc;
        public EditLanguage()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void readDB()
        {
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT Languidge FROM Languidge", dbc);
            adapter1.Fill(table1);
            DataRow[] RowsText;
            RowsText = table1.Select();
            listBox1.Items.Clear();
            foreach (DataRow row in RowsText)
            {
                if (row.ItemArray[0].ToString() != "English")
                {
                    listBox1.Items.Add(row.ItemArray[0].ToString());
                }
            }
            listBox1.SelectedIndex = 0;
        }

        private void EditLanguage_Load(object sender, EventArgs e)
        {
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[67];
            label2.Text = (Application.OpenForms[0] as Form1).TextProg[127];
            label3.Text = (Application.OpenForms[0] as Form1).TextProg[128];
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[129];
            button2.Text = (Application.OpenForms[0] as Form1).TextProg[130];
            button3.Text = (Application.OpenForms[0] as Form1).TextProg[131];
            button4.Text = (Application.OpenForms[0] as Form1).TextProg[116];

            dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
            dbc.Open();
            readDB();
        }

        private void EditLanguage_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbc.Close();
        }

        // добавление языка
        private void button1_Click(object sender, EventArgs e)
        {
            // если не заполнены поля, выдать сообщение
            if (textBox1.Text == "")
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[132] + " '" + (Application.OpenForms[0] as Form1).TextProg[127] + "'.");
                return;
            }
            if(textBox2.Text == "")
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[132] + " '" + (Application.OpenForms[0] as Form1).TextProg[128] + "'.");
                return;
            }
            if (textBox2.Text.Length == 1 || textBox2.Text.Length > 2)
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[133]);
                return;
            }
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT Languidge FROM Languidge", dbc);
            adapter1.Fill(table1);
            DataRow[] RowsText;
            RowsText = table1.Select();
            foreach (DataRow row in RowsText)
            {
                if (textBox1.Text == row.ItemArray[0].ToString())
                {
                    MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[134]);
                    return;
                }
            }

            // поля заполнены, создаем язык
            insertLanguage();

            readDB();
        }

        private void insertLanguage()
        {
            // вставить данные в таблицу Language
            OleDbCommand command = dbc.CreateCommand();
            // создать таблицу с текстом вопросов (копия английской версии)
            command.CommandText = "SELECT * INTO Questions_" + textBox2.Text + " FROM Questions_en";
            command.ExecuteNonQuery();

            command.CommandText = "INSERT INTO [Languidge](Languidge, Name_table, Name_column, Practical) VALUES('" + textBox1.Text + "', 'Questions_" + textBox2.Text + "', 'text_" + textBox2.Text + "', 'practical_question_" + textBox2.Text + "');";
            command.ExecuteNonQuery();
            // создать столбец в таблице Practical
            command.CommandText = "ALTER TABLE Practical ADD practical_question_" + textBox2.Text + " LONGTEXT";
            command.ExecuteNonQuery();
            // заполнить столбец
            command.CommandText = "UPDATE Practical SET practical_question_" + textBox2.Text + "=practical_question_en";
            command.ExecuteNonQuery();
            // создать столбец в таблице Text_prog
            command.CommandText = "ALTER TABLE Text_prog ADD text_" + textBox2.Text + " LONGTEXT";
            command.ExecuteNonQuery();
            // заполнить столбец
            command.CommandText = "UPDATE Text_prog SET text_" + textBox2.Text + "=text_en";
            command.ExecuteNonQuery();
            command.CommandText = "UPDATE Text_prog SET text_" + textBox2.Text + "=text_en";
            command.ExecuteNonQuery();
        }

        private void deliteLanguage( string lang)
        {
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT Name_table, Name_column, Practical FROM Languidge WHERE Languidge = '" + lang + "'", dbc);
            adapter1.Fill(table1);
            DataRow[] RowsText;
            RowsText = table1.Select();
            string NameTable = RowsText[0].ItemArray[0].ToString();
            string NameColumn = RowsText[0].ItemArray[1].ToString();
            string Practical = RowsText[0].ItemArray[2].ToString();

            OleDbCommand command = dbc.CreateCommand();
            // удалить столбец в таблице Practical
            command.CommandText = "ALTER TABLE Practical DROP COLUMN " + Practical;
            command.ExecuteNonQuery();
            // удалить столбец в таблице Text_prog
            command.CommandText = "ALTER TABLE Text_prog DROP COLUMN " + NameColumn;
            command.ExecuteNonQuery();
            // удалить таблицу с текстом языка
            command.CommandText = "DROP TABLE [" + NameTable + "]";
            command.ExecuteNonQuery();
            // удалить строку с языком в таблице Languidge
            command.CommandText = "DELETE FROM Languidge WHERE Languidge ='" + lang + "'";
            command.ExecuteNonQuery();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            string lang = listBox1.SelectedItem.ToString();

            if (MessageBox.Show(((Application.OpenForms[0] as Form1).TextProg[135] + " " + lang + "?"), (Application.OpenForms[0] as Form1).TextProg[130], MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                deliteLanguage(lang);
                readDB();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            readDB();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = ((ListBox)sender).SelectedItem.ToString();
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT Name_table FROM Languidge WHERE Languidge ='" + textBox1.Text + "'", dbc);
            adapter1.Fill(table1);
            DataRow[] RowsText;
            RowsText = table1.Select();
            textBox2.Text = RowsText[0].ItemArray[0].ToString().Substring(RowsText[0].ItemArray[0].ToString().Length-2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deliteLanguage(listBox1.SelectedItem.ToString());
            insertLanguage();
            readDB();
        }
    }
}
