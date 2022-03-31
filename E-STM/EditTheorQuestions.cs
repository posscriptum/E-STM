using E_STM.Properties;
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
    public partial class EditTheorQuestions : Form
    {
        OleDbConnection dbc;
        string lengEnglish = "English";
        string lang;
        public string PathToPicOfQuestion;


        public EditTheorQuestions()
        {
            InitializeComponent();
        }

        private void EditTheorQuestions_Load(object sender, EventArgs e)
        {
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[136];
            label9.Text = (Application.OpenForms[0] as Form1).TextProg[137];
            label8.Text = (Application.OpenForms[0] as Form1).TextProg[138];
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[117];
            button2.Text = (Application.OpenForms[0] as Form1).TextProg[139];
            button3.Text = (Application.OpenForms[0] as Form1).TextProg[140];
            button5.Text = (Application.OpenForms[0] as Form1).TextProg[119];
            button4.Text = (Application.OpenForms[0] as Form1).TextProg[141];
            groupBox1.Text = (Application.OpenForms[0] as Form1).TextProg[142];
            groupBox8.Text = (Application.OpenForms[0] as Form1).TextProg[142];
            groupBox9.Text = (Application.OpenForms[0] as Form1).TextProg[146];

            groupBox2.Text = (Application.OpenForms[0] as Form1).TextProg[143];
            groupBox3.Text = (Application.OpenForms[0] as Form1).TextProg[144] + " 1 " + (Application.OpenForms[0] as Form1).TextProg[145];
            groupBox4.Text = (Application.OpenForms[0] as Form1).TextProg[144] + " 2 " + (Application.OpenForms[0] as Form1).TextProg[145];
            groupBox5.Text = (Application.OpenForms[0] as Form1).TextProg[144] + " 3 " + (Application.OpenForms[0] as Form1).TextProg[145];
            groupBox6.Text = (Application.OpenForms[0] as Form1).TextProg[144] + " 4 " + (Application.OpenForms[0] as Form1).TextProg[145];
            groupBox7.Text = (Application.OpenForms[0] as Form1).TextProg[144] + " 5 " + (Application.OpenForms[0] as Form1).TextProg[145];


            dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
            dbc.Open();          

            //заполнить список языков
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT Languidge FROM Languidge", dbc);
            adapter.Fill(table);
            DataRow[] RowsText;
            RowsText = table.Select();
            comboBox1.Items.Clear();
            for (int i = 1; i < table.Rows.Count; i++)
            {
                comboBox1.Items.Add(RowsText[i].ItemArray[0].ToString());
            }
            comboBox1.SelectedIndex = 0;

            addQuestionInComboBox();

            radioButton1.Checked = true;
        }

        private void addQuestionInComboBox()
        {
            //заполнить список вопросов
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT * FROM Questions_en", dbc);
            adapter1.Fill(table1);
            DataRow[] tabelQuestions;
            tabelQuestions = table1.Select();
            comboBox2.Items.Clear();
            for (int i = 0; i < table1.Rows.Count; i++)
            {
                comboBox2.Items.Add(tabelQuestions[i].ItemArray[2].ToString());
            }
            comboBox2.SelectedIndex = 0;
        }

        private void EditTheorQuestions_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbc.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lang = comboBox1.SelectedItem.ToString();

            if (label2.Text != "label2")
            {
                changeTextInTextBox(label2.Text);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = false;
            comboBox2.Enabled = true;
            button1.Enabled = true;
            button5.Enabled = true;
            button3.Enabled = true;

            label2.Visible = true;
            textBox8.Visible = false;
            label3.Visible = true;
            textBox9.Visible = false;
            label4.Visible = true;
            textBox10.Visible = false;
            label5.Visible = true;
            textBox11.Visible = false;
            label6.Visible = true;
            textBox12.Visible = false;
            label7.Visible = true;
            textBox13.Visible = false;

            addQuestionInComboBox();
            changeTextInTextBox(comboBox2.SelectedItem.ToString());
        }

        private void changeTextInTextBox(string question)
        {
            string nameTable = nameTatleCurrentLang();

            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT ID FROM Questions_en WHERE question ='" + question + "'", dbc);
            adapter1.Fill(table1);
            DataRow[] foundRows1;
            foundRows1 = table1.Select();
            string ID = foundRows1[0].ItemArray[0].ToString();

            DataTable table2 = new DataTable();
            OleDbDataAdapter adapter2 = new OleDbDataAdapter("SELECT * FROM " + nameTable + " WHERE ID =" + ID, dbc);
            adapter2.Fill(table2);
            DataRow[] foundRows2;
            foundRows2 = table2.Select();

            textBox1.Text = foundRows2[0].ItemArray[2].ToString();
            textBox2.Text = foundRows2[0].ItemArray[3].ToString();
            textBox3.Text = foundRows2[0].ItemArray[4].ToString();
            textBox4.Text = foundRows2[0].ItemArray[5].ToString();
            textBox5.Text = foundRows2[0].ItemArray[6].ToString();
            textBox6.Text = foundRows2[0].ItemArray[7].ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
            comboBox2.Enabled = false;
            button1.Enabled = false;
            button5.Enabled = false;
            button3.Enabled = false;

            label2.Visible = false;
            textBox8.Visible = true;
            label3.Visible = false;
            textBox9.Visible = true;
            label4.Visible = false;
            textBox10.Visible = true;
            label5.Visible = false;
            textBox11.Visible = true;
            label6.Visible = false;
            textBox12.Visible = true;
            label7.Visible = false;
            textBox13.Visible = true;

            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = "";
            textBox8.Text = textBox9.Text = textBox10.Text = textBox11.Text = textBox12.Text = textBox13.Text = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //заполнить поля
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM Questions_en WHERE question = '" + comboBox2.SelectedItem.ToString() + "'", dbc);
            adapter.Fill(table);
            DataRow[] RowsText;
            RowsText = table.Select();
            label2.Text = RowsText[0].ItemArray[2].ToString();
            label3.Text = RowsText[0].ItemArray[3].ToString();
            label4.Text = RowsText[0].ItemArray[4].ToString();
            label5.Text = RowsText[0].ItemArray[5].ToString();
            label6.Text = RowsText[0].ItemArray[6].ToString();
            label7.Text = RowsText[0].ItemArray[7].ToString();
            textBox7.Text = RowsText[0].ItemArray[8].ToString();

            //отобразить картинку, если есть
            if (RowsText[0].ItemArray[11].ToString() != "")
            {
                pictureOfQuestion.Image = (Image)Resources.ResourceManager.GetObject(RowsText[0].ItemArray[11].ToString());
            }else
            {
                pictureOfQuestion.Image = null;
            }

            radioButton4.Checked = (bool)(RowsText[0].ItemArray[9]);
            radioButton3.Checked = !(bool)(RowsText[0].ItemArray[9]);
            if (RowsText[0].ItemArray[10].ToString() == "DC electric circuits")
            {
                radioButton5.Checked = true;
            }
            else if (RowsText[0].ItemArray[10].ToString() == "AC electric circuits")
            {
                radioButton6.Checked = true;
            }
            else if (RowsText[0].ItemArray[10].ToString() == "Theory of magnetism")
            {
                radioButton7.Checked = true;
            }
            else if (RowsText[0].ItemArray[10].ToString() == "Digital electronics")
            {
                radioButton8.Checked = true;
            }
            else if (RowsText[0].ItemArray[10].ToString() == "Electrical machines")
            {
                radioButton9.Checked = true;
            }

            changeTextInTextBox(comboBox2.SelectedItem.ToString());
        }

        private void label2_TextChanged(object sender, EventArgs e)
        {
            //textBox8.Text = label2.Text;
        }

        private void label3_TextChanged(object sender, EventArgs e)
        {
            //textBox9.Text = label3.Text;
        }
        private void label4_TextChanged(object sender, EventArgs e)
        {
            //textBox10.Text = label4.Text;
        }

        private void label5_TextChanged(object sender, EventArgs e)
        {
            //textBox11.Text = label5.Text;
        }

        private void label6_TextChanged(object sender, EventArgs e)
        {
            //textBox12.Text = label6.Text;
        }

        private void label7_TextChanged(object sender, EventArgs e)
        {
            //textBox13.Text = label7.Text;
        }

        // получить название таблицы для выбранного языка
        private string nameTatleCurrentLang()
        {
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT Name_table FROM Languidge WHERE Languidge ='" + comboBox1.SelectedItem.ToString() + "'", dbc);
            adapter.Fill(table);
            DataRow[] foundRows;
            foundRows = table.Select();
            return foundRows[0].ItemArray[0].ToString();
        }
        //кнопка обновления вопроса
        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Update Base?", "Update", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string tableLang = nameTatleCurrentLang();

                DataTable table1 = new DataTable();
                OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT ID FROM Questions_en WHERE question ='" + comboBox2.Text + "'", dbc);
                adapter1.Fill(table1);
                DataRow[] foundRows1;
                foundRows1 = table1.Select();
                string ID = foundRows1[0].ItemArray[0].ToString();

                if (changeDBIfchangedText("question", tableLang, ID, textBox1.Text) |
                   changeDBIfchangedText("var1", tableLang, ID, textBox2.Text) |
                   changeDBIfchangedText("var2", tableLang, ID, textBox3.Text) |
                   changeDBIfchangedText("var3", tableLang, ID, textBox4.Text) |
                   changeDBIfchangedText("var4", tableLang, ID, textBox5.Text) |
                   changeDBIfchangedText("var5", tableLang, ID, textBox6.Text) |
                   changeRACBCInAllTableLang(ID)
                   )
                {
                    MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[147], "Update Base", MessageBoxButtons.OK);
                }
            }
        }

        private bool changeRACBCInAllTableLang(string ID)
        {
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT Name_table FROM Languidge", dbc);
            adapter1.Fill(table1);
            DataRow[] foundRows1;
            foundRows1 = table1.Select();
            bool ret = false;
            foreach (DataRow row in foundRows1)
            {
                ret = ret | changeDBIfChangedRightAnswer(row.ItemArray[0].ToString(), ID) |
                changeDBIfChangedChechBox(row.ItemArray[0].ToString(), ID) |
                changeDBIfChangedChapter(row.ItemArray[0].ToString(), ID);
            }
            return ret;
        }

        private bool changeDBIfchangedText(string field, string tableQuestion, string ID, string changeText)
        {
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT " + field + " FROM " + tableQuestion + " WHERE ID =" + ID, dbc);
            adapter1.Fill(table1);
            DataRow[] foundRows1;
            foundRows1 = table1.Select();
            string fieldCurrent = foundRows1[0].ItemArray[0].ToString();

            if (changeText != fieldCurrent)
            {
                OleDbCommand command = dbc.CreateCommand();
                command.CommandText = "UPDATE [" + tableQuestion + "] SET " + field + "='" + changeText + "' WHERE ID=" + ID;
                try
                {
                command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    MessageBox.Show("Error: " + e.Message);
                }
                return true;
            }else
            {
                return false;
            }
        }

        private bool changeDBIfChangedRightAnswer(string tableQuestion, string ID)
        {
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT rightAnswer FROM " + tableQuestion + " WHERE ID =" + ID, dbc);
            adapter1.Fill(table1);
            DataRow[] foundRows1;
            foundRows1 = table1.Select();
            string fieldCurrent = foundRows1[0].ItemArray[0].ToString();

            if (textBox7.Text != fieldCurrent)
            {
                OleDbCommand command = dbc.CreateCommand();
                command.CommandText = "UPDATE [" + tableQuestion + "] SET rightAnswer='" + textBox7.Text + "' WHERE ID=" + ID;
                command.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool changeDBIfChangedChechBox(string tableQuestion, string ID)
        {
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT checkBox FROM " + tableQuestion + " WHERE ID =" + ID, dbc);
            adapter1.Fill(table1);
            DataRow[] foundRows1;
            foundRows1 = table1.Select();
            bool fieldCurrent = (bool)foundRows1[0].ItemArray[0];

            if ((fieldCurrent && !radioButton4.Checked) || (!fieldCurrent && radioButton4.Checked))
            {
                OleDbCommand command = dbc.CreateCommand();
                command.CommandText = "UPDATE [" + tableQuestion + "] SET checkBox=" + radioButton4.Checked.ToString() + " WHERE ID=" + ID;
                command.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool changeDBIfChangedChapter(string tableQuestion, string ID)
        {
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT chapter FROM " + tableQuestion + " WHERE ID =" + ID, dbc);
            adapter1.Fill(table1);
            DataRow[] foundRows1;
            foundRows1 = table1.Select();
            string fieldCurrent = foundRows1[0].ItemArray[0].ToString();
            string radiobutton = valueCurrentCrapter();

            if (fieldCurrent != radiobutton)
            {
                OleDbCommand command = dbc.CreateCommand();
                command.CommandText = "UPDATE [" + tableQuestion + "] SET chapter='" + radiobutton + "' WHERE ID=" + ID;
                command.ExecuteNonQuery();
                return true;
            }
            else
            {
                return false;
            }
        }

        private string valueCurrentCrapter()
        {
            if (radioButton5.Checked)
            {
                return "DC electric circuits";
            }
            else if (radioButton6.Checked)
            {
                return "AC electric circuits";
            }
            else if (radioButton7.Checked)
            {
                return "Theory of magnetism";
            }
            else if (radioButton8.Checked)
            {
                return "Digital electronics";
            }
            else
            {
                return "Electrical machines";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[148], (Application.OpenForms[0] as Form1).TextProg[26], MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = textBox6.Text = "";
                textBox8.Text = textBox9.Text = textBox10.Text = textBox11.Text = textBox12.Text = textBox13.Text = "";
            }
        }

        // добавление вороса в базу
        private void button2_Click(object sender, EventArgs e)
        {
            // проверка, что заполнены поля с вопросом и первые два поля
            if (textBox1.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[132] + " '" + (Application.OpenForms[0] as Form1).TextProg[143] + "'");
                return;
            }
            if (textBox2.Text == "" || textBox3.Text == "" || textBox9.Text == "" || textBox10.Text == "")
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[149]);
                return;
            }

            // добавление вопроса
            addRowInTabels();
        }

        private void addRowInTabels()
        {
            // добавляем вопрос в английскую таблицу
            // добавить вопрос в базу
            if (getRowFromTable("SELECT ID FROM Questions_en WHERE question='" + textBox8.Text + "'").Count<DataRow>() == 0)
            {
                commandToDB("INSERT INTO [Questions_en](question, var1, var2, var3, var4, var5, rightAnswer, checkBox, chapter) VALUES('" + textBox8.Text + "', '" + textBox9.Text + "', '" + textBox10.Text + "', '" + textBox11.Text + "', '" + textBox12.Text + "', '" + textBox13.Text + "', '" + textBox7.Text + "', " + radioButton4.Checked.ToString() + ", '" + valueCurrentCrapter() + "');");
            }else
            {
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[150]);
                return;
            }

            // получить ID добавленного вопроса в английской таблице
            string ID = getRowFromTable("SELECT ID FROM Questions_en WHERE question ='" + textBox8.Text + "'")[0].ItemArray[0].ToString();

            // проверить, есть-ли такая строчка с ID в других таблицах
            // получить список таблиц
            DataRow[] dataRow = getRowFromTable("SELECT Name_table FROM Languidge");
            foreach (DataRow row in dataRow)
            {
                string strLang = (getRowFromTable("SELECT Languidge FROM Languidge WHERE Name_table='"+ row.ItemArray[0].ToString() + "'")[0].ItemArray[0].ToString());
                if (strLang != "English" && strLang != comboBox1.SelectedItem.ToString())
                {
                    // пробуем получить строку с соответствующим ID и если она существует, то обновляем ее, если нет, то записываем новую строку
                    if (getRowFromTable("SELECT question FROM " + row.ItemArray[0].ToString() + " WHERE ID=" + ID).Count<DataRow>() > 0)
                    {
                        commandToDB("UPDATE [" + row.ItemArray[0].ToString() + "] SET question='" + textBox8.Text + "', var1='" + textBox9.Text + "', var2='" + textBox10.Text + "', var3='" + textBox11.Text + "', var4='" + textBox12.Text + "', var5='" + textBox13.Text + "', rightAnswer=" + textBox7.Text + ", checkBox=" + radioButton4.Checked.ToString() + ", chapter='" + valueCurrentCrapter() + "' WHERE ID=" + ID);
                    } else
                    {
                        commandToDB("INSERT INTO [" + row.ItemArray[0].ToString() + "](ID, question, var1, var2, var3, var4, var5, rightAnswer, checkBox, chapter) VALUES('" + ID + "', '" + textBox8.Text + "', '" + textBox9.Text + "', '" + textBox10.Text + "', '" + textBox11.Text + "', '" + textBox12.Text + "', '" + textBox13.Text + "', '" + textBox7.Text + "', " + radioButton4.Checked.ToString() + ", '" + valueCurrentCrapter() + "');");
                    }
                } else if (strLang == comboBox1.SelectedItem.ToString())
                {
                    // пробуем получить строку с соответствующим ID и если она существует, то обновляем ее, если нет, то записываем новую строку
                    if (getRowFromTable("SELECT question FROM " + row.ItemArray[0].ToString() + " WHERE ID=" + ID).Count<DataRow>() > 0)
                    {
                        commandToDB("UPDATE [" + row.ItemArray[0].ToString() + "] SET question='" + textBox1.Text + "', var1='" + textBox2.Text + "', var2='" + textBox3.Text + "', var3='" + textBox4.Text + "', var4='" + textBox5.Text + "', var5='" + textBox6.Text + "', rightAnswer=" + textBox7.Text + ", checkBox=" + radioButton4.Checked.ToString() + ", chapter='" + valueCurrentCrapter() + "' WHERE ID=" + ID);
                    }
                    else
                    {
                        commandToDB("INSERT INTO [" + row.ItemArray[0].ToString() + "](ID, question, var1, var2, var3, var4, var5, rightAnswer, checkBox, chapter) VALUES('" + ID + "', '" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', '" + textBox7.Text + "', " + radioButton4.Checked.ToString() + ", '" + valueCurrentCrapter() + "');");
                    }
                }
            }
            MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[151]);

        }

        private void commandToDB (string commandStr)
        {
            OleDbCommand command = dbc.CreateCommand();
            command.CommandText = commandStr;
            command.ExecuteNonQuery();
        }

        // получить массив строк из базы по строке запроса
        private DataRow[] getRowFromTable (string selectingString)
        {
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectingString, dbc);
            adapter.Fill(table);
            return table.Select();
        }

        private void button5_Click(object sender, EventArgs e)
        {
           if (MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[152], "Delete question", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // получить ID добавленного вопроса в английской таблице
                string ID = getRowFromTable("SELECT ID FROM Questions_en WHERE question ='" + comboBox2.SelectedItem.ToString() + "'")[0].ItemArray[0].ToString();

                // удалить этот вопрос из всех таблиц
                // получить список таблиц
                DataRow[] dataRow = getRowFromTable("SELECT Name_table FROM Languidge");
                foreach (DataRow row in dataRow)
                {
                    commandToDB("DELETE FROM " + row.ItemArray[0].ToString() + " WHERE ID=" + ID);
                }
                addQuestionInComboBox();
                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[153]);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to change picture into this question?", "Change picture", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            LoadPic loadPic = new LoadPic();
            loadPic.ShowDialog();

            // получить ID добавленного вопроса в английской таблице
            string ID = getRowFromTable("SELECT ID FROM Questions_en WHERE question ='" + comboBox2.SelectedItem.ToString() + "'")[0].ItemArray[0].ToString();

            // добавить в этот вопрос путь на картинку во всех таблицах
            // получить список таблиц
            DataRow[] dataRow = getRowFromTable("SELECT Name_table FROM Languidge");
            foreach (DataRow row in dataRow)
            {
                commandToDB("UPDATE [" + row.ItemArray[0].ToString() + "] SET pic_name='" + PathToPicOfQuestion + "' WHERE ID=" + ID);
            }
            addQuestionInComboBox();
            MessageBox.Show("Picture successfully added!");
        }
    }
}
