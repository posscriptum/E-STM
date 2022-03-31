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

    public partial class Questions : Form
    {
        Size sizeWindow;
        // соединение с базой
        OleDbConnection dbc;
        // 5 листов для 5 разделов теор. теста
        private List<TheorQuestion> List1TheorQuestion = new List<TheorQuestion>();
        private List<TheorQuestion> List2TheorQuestion = new List<TheorQuestion>();
        private List<TheorQuestion> List3TheorQuestion = new List<TheorQuestion>();
        private List<TheorQuestion> List4TheorQuestion = new List<TheorQuestion>();
        private List<TheorQuestion> List5TheorQuestion = new List<TheorQuestion>();

        private int CountQuestion;
        private List<string> List;
        private bool closeButton;
        private List<TheorQuestion> ListCurrent = new List<TheorQuestion>();
        private List<string> ListAnswers = new List<string>();
        private int TimeForEnd;
        private int saveTime;
        private int LocationGT, LocationRT;

        public Questions()
        {
            InitializeComponent();
            // скопировать тексты приложения в List
            if (Application.OpenForms.Count > 0) { List = (Application.OpenForms[0] as Form1).TextProg; }
        }

        private void fillListOfQuestion (List<TheorQuestion> list, string chapter, int numberOfQuestion)
        {
            //записть вопросов из БД
            dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
            dbc.Open();

            string NameOfTable;
            DataTable tab = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT Name_table FROM Languidge WHERE Languidge = '" + (Application.OpenForms[0] as Form1).Lang + "'", dbc);
            adapter.Fill(tab);
            DataRow[] arrRows;
            arrRows = tab.Select();
            NameOfTable = arrRows[0].ItemArray[0].ToString();

            DataTable table = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT question, var1, var2, var3, var4, var5, rightAnswer, checkBox, pic_name FROM " + NameOfTable + " WHERE chapter = '" + chapter + "'", dbc);
            adapter1.Fill(table);
            DataRow[] RowsText;
            RowsText = table.Select();
            list.Clear();
            List<TheorQuestion> listWithAllQuestionFromDB = new List<TheorQuestion>();
            List<TheorQuestion> tempList = new List<TheorQuestion>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                tempList.Add(new TheorQuestion(RowsText[i].ItemArray[0].ToString(), RowsText[i].ItemArray[1].ToString(), RowsText[i].ItemArray[2].ToString(), RowsText[i].ItemArray[3].ToString(), RowsText[i].ItemArray[4].ToString(), RowsText[i].ItemArray[5].ToString(), RowsText[i].ItemArray[8].ToString(), RowsText[i].ItemArray[6].ToString(), (bool)RowsText[i].ItemArray[7]));
            }
            //перемешать вопросы в случайном порядке
            for (int i = 1; i <= numberOfQuestion; i++)
            {
                Random rand = new Random();
                int elementOfList = rand.Next(0, tempList.Count - 1);
                listWithAllQuestionFromDB.Add(tempList[elementOfList]);
                tempList.RemoveAt(elementOfList);
            }

            //обрезать количество вопросов по заданному количеству 
            if (chapter == "DC electric circuits")
            {
                var numberQuestionInSetOnSettingScreen = 0;
                int.TryParse((Application.OpenForms[0] as Form1).textBox5.Text, out numberQuestionInSetOnSettingScreen);
                if (numberQuestionInSetOnSettingScreen != 0) list.AddRange(listWithAllQuestionFromDB.GetRange(0, numberQuestionInSetOnSettingScreen));
            }else if (chapter == "AC electric circuits")
            {
                var numberQuestionInSetOnSettingScreen = 0;
                int.TryParse((Application.OpenForms[0] as Form1).textBox6.Text, out numberQuestionInSetOnSettingScreen);
                if (numberQuestionInSetOnSettingScreen != 0) list.AddRange(listWithAllQuestionFromDB.GetRange(0, numberQuestionInSetOnSettingScreen));
            }
            else if (chapter == "Theory of magnetism")
            {
                var numberQuestionInSetOnSettingScreen = 0;
                int.TryParse((Application.OpenForms[0] as Form1).textBox7.Text, out numberQuestionInSetOnSettingScreen);
                if (numberQuestionInSetOnSettingScreen != 0) list.AddRange(listWithAllQuestionFromDB.GetRange(0, numberQuestionInSetOnSettingScreen));
            }
            else if (chapter == "Digital electronics")
            {
                var numberQuestionInSetOnSettingScreen = 0;
                int.TryParse((Application.OpenForms[0] as Form1).textBox8.Text, out numberQuestionInSetOnSettingScreen);
                if (numberQuestionInSetOnSettingScreen != 0) list.AddRange(listWithAllQuestionFromDB.GetRange(0, numberQuestionInSetOnSettingScreen));
            }
            else if (chapter == "Electrical machines")
            {
                var numberQuestionInSetOnSettingScreen = 0;
                int.TryParse((Application.OpenForms[0] as Form1).textBox9.Text, out numberQuestionInSetOnSettingScreen);
                if (numberQuestionInSetOnSettingScreen != 0) list.AddRange(listWithAllQuestionFromDB.GetRange(0, numberQuestionInSetOnSettingScreen));
            }
            
            //закрыть базу за ненадобностью
            dbc.Close();
        }

        private void Questions_Load(object sender, EventArgs e)
        {
            sizeWindow = this.Size;
            this.ControlBox = false;
            LocationGT = this.Width - label17.Location.X;
            LocationRT = this.Width - label5.Location.X;
            //this.CenterToScreen();
            try
            {
                saveTime = (Application.OpenForms[1] as Theoretical_test).SaveTime;
            }
            catch
            {
                //MessageBox.Show("4"); 
            }

            // постоянные цепи
            fillListOfQuestion(List1TheorQuestion, "DC electric circuits", (Application.OpenForms[0] as Form1).NrQuestions1);

            // переменнные цепи
            fillListOfQuestion(List2TheorQuestion, "AC electric circuits", (Application.OpenForms[0] as Form1).NrQuestions2);

            //магнетизм
            fillListOfQuestion(List3TheorQuestion, "Theory of magnetism", (Application.OpenForms[0] as Form1).NrQuestions3);

            //цифровая электроника
            fillListOfQuestion(List4TheorQuestion, "Digital electronics", (Application.OpenForms[0] as Form1).NrQuestions4);

            //электрические машины
            fillListOfQuestion(List5TheorQuestion, "Electrical machines", (Application.OpenForms[0] as Form1).NrQuestions5);

            if (Application.OpenForms[0] is Form1)
            {
                timer1.Enabled = false; // таймер с тиком в 1 сек.

                showQuestionStart(1, List1TheorQuestion, (Application.OpenForms[1] as Theoretical_test).time1);
                showQuestionStart(2, List2TheorQuestion, (Application.OpenForms[1] as Theoretical_test).time2);
                showQuestionStart(3, List3TheorQuestion, (Application.OpenForms[1] as Theoretical_test).time3);
                showQuestionStart(4, List4TheorQuestion, (Application.OpenForms[1] as Theoretical_test).time4);
                showQuestionStart(5, List5TheorQuestion, (Application.OpenForms[1] as Theoretical_test).time5);
            }
        }

        private void showQuestionStart (int number, List<TheorQuestion> list, int time)
        {
            if ((Application.OpenForms[0] as Form1).NumberOfTypeQuestion == number)
            {
                ShowQuestion(list[CountQuestion]);
                ListCurrent = list;
                timer1.Enabled = true;
                TimeForEnd = (time + saveTime) * 60;
            }
        }
        /// <summary>
        /// показать вопрос 
        /// </summary>
        /// <param name="Q">вопрос</param>
        private void ShowQuestion(TheorQuestion Q)
        {
            this.Controls.Clear();
            this.Controls.Add(QuestionText);
            QuestionText.Text = Q.Name;
            this.Controls.Add(pictureBox1);
            this.Controls.Add(button1);
            this.Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(label17);
            Controls.Add(label4);
            Controls.Add(label5);
            pictureBox1.Image = Q.Picture.Image;

            if (Q.itIsCheckBox)
            {
                for (int i = 0; i < Q.QuestionCB.Count; i++)
                {
                    Q.QuestionCB[i].Visible = true;
                    if (i > 0)
                    {
                        Q.QuestionCB[i].Location = new Point(Q.QuestionCB[i - 1].Location.X, Q.QuestionCB[i - 1].Location.Y + 2 + Q.QuestionCB[i - 1].Size.Height);
                    }
                    Controls.Add(Q.QuestionCB[i]);
                    //изменение размера окна, если текст слишком длинный
                    if (47 + Q.QuestionCB[i].Size.Width > this.Width)
                    {
                        this.Width = 47 + Q.QuestionCB[i].Size.Width + 47;
                    }
                    if(i == (Q.QuestionCB.Count - 1))
                    {
                        pictureBox1.Location = new Point(pictureBox1.Location.X, Q.QuestionCB[i].Location.Y + 2 + Q.QuestionCB[i].Size.Height);
                        pictureBox1.Size = new Size(pictureBox1.Size.Width, button1.Location.Y - Q.QuestionCB[i].Location.Y - 25);
                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < Q.QuestionRB.Count; i++)
                {
                    Q.QuestionRB[i].Visible = true;
                    if (i > 0)
                    {
                        Q.QuestionRB[i].Location = new Point(Q.QuestionRB[i - 1].Location.X, Q.QuestionRB[i-1].Location.Y + 2 + Q.QuestionRB[i-1].Size.Height);
                    }
                    Controls.Add(Q.QuestionRB[i]);
                    //изменение размера окна, если текст слишком длинный
                    if (47 + Q.QuestionRB[i].Size.Width > this.Width)
                    {
                        this.Width = 47 + Q.QuestionRB[i].Size.Width + 47;
                    }
                    if (i == (Q.QuestionRB.Count - 1))
                    {
                        pictureBox1.Location = new Point(pictureBox1.Location.X, Q.QuestionRB[i].Location.Y + 2 + Q.QuestionRB[i].Size.Height);
                        pictureBox1.Size = new Size(pictureBox1.Size.Width, button1.Location.Y - Q.QuestionRB[i].Location.Y - 25);
                    }
                }
            }
        }


        /// <summary>
        /// кнопка next
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Size = sizeWindow;
            if (closeButton)
            {

                this.Close();
            }
            if (CountQuestion < ListCurrent.Count - 1)
            {
                CountQuestion++;
                ShowQuestion(ListCurrent[CountQuestion]);


                closeButton = false;
                return;
            }
            if (CountQuestion == ListCurrent.Count - 1)
            {
                Controls.Clear();
                Controls.Add(label1);
                Controls.Add(label2);
                Controls.Add(button2);
                Controls.Add(button1);
                label1.Text = List[60];
                label1.Visible = true;
                label2.Text = List[61];
                label2.Visible = true;
                CountQuestion++;
                closeButton = true;
            }
        }

        // кнопка preview
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (CountQuestion > 0)
            {
                this.Size = sizeWindow;
                CountQuestion--;
                ShowQuestion(ListCurrent[CountQuestion]);
                Controls.Add(label1);
                Controls.Add(label2);
                Controls.Add(button2);
                closeButton = false;
                QuestionText.Visible = true;
                label1.Visible = false;
                label2.Visible = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// обратный отсчет для отсавшегося времени (текущий раздел и общее время)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ListCurrent[CountQuestion].timeSpendOnQuestion++;
            }
            catch {  }
            
            if (Application.OpenForms.Count == 1)
            {
                timer1.Enabled = false;
                return;
            }
            label17.Text = (Application.OpenForms[1] as Theoretical_test).label17.Text;
            TimeForEnd--;

            int min, sec;
            string mins, secs;
            if (TimeForEnd == 0)
            {
                (Application.OpenForms[1] as Theoretical_test).SaveTime = 0;
                this.Close();
            }
            else if (TimeForEnd <= 0)
            {
                this.Close();
            }
            else
            {
                min = TimeForEnd  / 60;
                sec = TimeForEnd - min*60;
                if (min < 10)
                {
                    mins = "0" + min.ToString();
                }
                else
                {
                    mins = min.ToString();
                }
                if (sec < 10)
                {
                    secs = "0" + sec.ToString();
                }
                else
                {
                    secs = sec.ToString();
                }
                label5.Text = mins + ":" + secs;
                (Application.OpenForms[1] as Theoretical_test).SaveTime = min;
            }
        }

        private void Questions_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeButton = false;
            int a = (Application.OpenForms[1] as Theoretical_test).TotalQ = (Application.OpenForms[1] as Theoretical_test).TotalQ + ListCurrent.Count;
            int result = 0;
            for (int i = 0; i < ListCurrent.Count; i++)
            {
                if (ListCurrent[i].Answer == ListCurrent[i].RightAnswer)
                {
                    result++;
                }
            }
            int b = (Application.OpenForms[1] as Theoretical_test).RightQ = (Application.OpenForms[1] as Theoretical_test).RightQ + result;

            (Application.OpenForms[1] as Theoretical_test).label2.Text = a.ToString() + "/" + b.ToString();

            if ((Application.OpenForms[0] as Form1).NumberOfTypeQuestion == 1)
            {
                fillingListsAnswers((Application.OpenForms[0] as Form1).DCchapter);
                (Application.OpenForms[1] as Theoretical_test).label4.Text = ListCurrent.Count.ToString() + "/" + result.ToString();
            }
            if ((Application.OpenForms[0] as Form1).NumberOfTypeQuestion == 2)
            {
                fillingListsAnswers((Application.OpenForms[0] as Form1).ACchapter);
                (Application.OpenForms[1] as Theoretical_test).label5.Text = ListCurrent.Count.ToString() + "/" + result.ToString();
            }
            if ((Application.OpenForms[0] as Form1).NumberOfTypeQuestion == 3)
            {
                fillingListsAnswers((Application.OpenForms[0] as Form1).MagnetChapter);
                (Application.OpenForms[1] as Theoretical_test).label6.Text = ListCurrent.Count.ToString() + "/" + result.ToString();
            }
            if ((Application.OpenForms[0] as Form1).NumberOfTypeQuestion == 4)
            {
                fillingListsAnswers((Application.OpenForms[0] as Form1).DigitChapter);
                (Application.OpenForms[1] as Theoretical_test).label7.Text = ListCurrent.Count.ToString() + "/" + result.ToString();
            }
            if ((Application.OpenForms[0] as Form1).NumberOfTypeQuestion == 5)
            {
                fillingListsAnswers((Application.OpenForms[0] as Form1).MachinChapter);
                (Application.OpenForms[1] as Theoretical_test).label8.Text = ListCurrent.Count.ToString() + "/" + result.ToString();
            }
        }

        private void fillingListsAnswers(List<TheorQuestion> list)
        {
            foreach (TheorQuestion t in ListCurrent)
            {
                list.Add(t);
            }
        }

        private void QuestionText_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }

        private void Questions_Resize(object sender, EventArgs e)
        {
            label17.Location = new Point(this.Width - LocationGT, label17.Location.Y);
            label5.Location = new Point(this.Width - LocationRT, label5.Location.Y);
            label3.Location = new Point(label17.Location.X - label3.Width - 10, label3.Location.Y);
            label4.Location = new Point(label5.Location.X - label4.Width - 10, label4.Location.Y);
        }
    }
}