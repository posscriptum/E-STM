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
    public partial class ChangeText : Form
    {
        OleDbConnection dbc;
        public ChangeText()
        {
            InitializeComponent();
        }

        private void ChangeText_Load(object sender, EventArgs e)
        {
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[126];
            dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.15.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
            dbc.Open();

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();

            DataRow[] row = getRowFromTable("SELECT Languidge FROM Languidge");

            comboBox1.Items.Add(row[0].ItemArray[0].ToString());
            for (int i = 1; i < row.Count<DataRow>(); i++)
            {
                comboBox2.Items.Add(row[i].ItemArray[0].ToString());
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            readFromDB();

        }

        private void readFromDB()
        {
            string nameColomne = getRowFromTable("SELECT Name_column FROM Languidge WHERE Languidge='" + comboBox2.Text + "'")[0].ItemArray[0].ToString();
            DataRow[] row1 = getRowFromTable("SELECT text_en FROM Text_prog");
            DataRow[] row2 = getRowFromTable("SELECT " + nameColomne + " FROM Text_prog");
            DataTable table = new DataTable();
            table.Columns.Add("English", typeof(string));
            table.Columns.Add(comboBox2.Text, typeof(string));
            for (int i = 0 ; i < row1.Count<DataRow>(); i++)
            {
                table.Rows.Add(row1[i].ItemArray[0].ToString(), row2[i].ItemArray[0].ToString());
            }
            dataGridView1.DataSource = table;
            dataGridView1.Columns[0].Width = (dataGridView1.Width / 2) - 41;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].Width = (dataGridView1.Width / 2) - 2;
            dataGridView1.Update();
        }

        // записать команду SQL
        private void commandToDB(string commandStr)
        {
            OleDbCommand command = dbc.CreateCommand();
            command.CommandText = commandStr;
            try
            {
                command.ExecuteNonQuery();
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        // получить массив строк из базы по строке запроса
        private DataRow[] getRowFromTable(string selectingString)
        {
            DataTable table = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectingString, dbc);
            adapter.Fill(table);
            return table.Select();
        }

        private void ChangeText_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbc.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            readFromDB();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable table = (DataTable)dataGridView1.DataSource;
            DataRow[] rows = table.Select();
            string nameColomne = getRowFromTable("SELECT Name_column FROM Languidge WHERE Languidge='" + comboBox2.Text + "'")[0].ItemArray[0].ToString();
            foreach (DataRow row in rows)
            {
                commandToDB("UPDATE [Text_prog] SET " + nameColomne + "='" + row.ItemArray[1] + "' WHERE text_en='" + row.ItemArray[0] + "'");
            }
            MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[125], "Save", MessageBoxButtons.OK);
        }
    }
}
