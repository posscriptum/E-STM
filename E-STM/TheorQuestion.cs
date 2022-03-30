using E_STM.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace E_STM
{
    public class TheorQuestion
    {
        public string Name;
        public List<string> Questions = new List<string>();
        public PictureBox Picture = new PictureBox();
        public string RightAnswer;
        public string Answer;
        public bool itIsCheckBox;
        public List<CheckBox> QuestionCB = new List<CheckBox>();
        public List<RadioButton> QuestionRB = new List<RadioButton>();
        public bool RightOrNotAnswer;
        //public string pathToPicture;
        public int timeSpendOnQuestion;

        /// <summary>
        /// теоретический вопрос
        /// </summary>
        /// <param name="Name">сам вопрос</param>
        /// <param name="Q1">вариант 1</param>
        /// <param name="Q2">вариант 2</param>
        /// <param name="Q3">вариант 3</param>
        /// <param name="Q4">вариант 4</param>
        /// <param name="Q5">вариант 5</param>
        /// <param name="pic">рисунок к вопросу</param>
        /// <param name="RightAnswer">правильный ответ</param>
        /// <param name="itIsCheckBox">галочки или точки</param>
        public TheorQuestion (string Name, string Q1, string Q2, string Q3, string Q4, string Q5, string pic, string RightAnswer, bool itIsCheckBox)
        {
            this.Name = Name;
            this.itIsCheckBox = itIsCheckBox;

            if (Q1 != "" && !itIsCheckBox)
            {
                QuestionRB.Add(new RadioButton());
                QuestionRB[QuestionRB.Count - 1].Location = new Point(42, 159);
                QuestionRB[QuestionRB.Count - 1].Visible = false;
                QuestionRB[QuestionRB.Count - 1].Text = Q1;
                QuestionRB[QuestionRB.Count - 1].AutoSize = true;
                QuestionRB[QuestionRB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q2 != "" && !itIsCheckBox)
            {
                QuestionRB.Add(new RadioButton());
                QuestionRB[QuestionRB.Count - 1].Location = new Point(42, QuestionRB[QuestionRB.Count - 2].Location.Y + QuestionRB[QuestionRB.Count - 2].Height +7);
                QuestionRB[QuestionRB.Count - 1].Visible = false;
                QuestionRB[QuestionRB.Count - 1].Text = Q2;
                QuestionRB[QuestionRB.Count - 1].AutoSize = true;
                QuestionRB[QuestionRB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q3 != "" && !itIsCheckBox)
            {
                QuestionRB.Add(new RadioButton());
                QuestionRB[QuestionRB.Count - 1].Location = new Point(42, QuestionRB[QuestionRB.Count - 2].Location.Y + QuestionRB[QuestionRB.Count - 2].Height + 7);
                QuestionRB[QuestionRB.Count - 1].Visible = false;
                QuestionRB[QuestionRB.Count - 1].Text = Q3;
                QuestionRB[QuestionRB.Count - 1].AutoSize = true;
                QuestionRB[QuestionRB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q4 != "" && !itIsCheckBox)
            {
                QuestionRB.Add(new RadioButton());
                QuestionRB[QuestionRB.Count - 1].Location = new Point(42, QuestionRB[QuestionRB.Count - 2].Location.Y + QuestionRB[QuestionRB.Count - 2].Height + 7);
                QuestionRB[QuestionRB.Count - 1].Visible = false;
                QuestionRB[QuestionRB.Count - 1].Text = Q4;
                QuestionRB[QuestionRB.Count - 1].AutoSize = true;
                QuestionRB[QuestionRB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q5 != "" && !itIsCheckBox)
            {
                QuestionRB.Add(new RadioButton());
                QuestionRB[QuestionRB.Count - 1].Location = new Point(42, QuestionRB[QuestionRB.Count - 2].Location.Y + QuestionRB[QuestionRB.Count - 2].Height + 7);
                QuestionRB[QuestionRB.Count - 1].Visible = false;
                QuestionRB[QuestionRB.Count - 1].Text = Q5;
                QuestionRB[QuestionRB.Count - 1].AutoSize = true;
                QuestionRB[QuestionRB.Count - 1].CheckedChanged += CheckAnswer;
            }

            if (Q1 != "" && itIsCheckBox)
            {
                QuestionCB.Add(new CheckBox());
                QuestionCB[QuestionCB.Count - 1].Location = new Point(42, 159);
                QuestionCB[QuestionCB.Count - 1].Visible = false;
                QuestionCB[QuestionCB.Count - 1].Text = Q1;
                QuestionCB[QuestionCB.Count - 1].AutoSize = true;
                QuestionCB[QuestionCB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q2 != "" && itIsCheckBox)
            {
                QuestionCB.Add(new CheckBox());
                QuestionCB[QuestionCB.Count - 1].Location = new Point(42, QuestionCB[QuestionCB.Count - 2].Location.Y + QuestionCB[QuestionCB.Count - 2].Height + 7);
                QuestionCB[QuestionCB.Count - 1].Visible = false;
                QuestionCB[QuestionCB.Count - 1].Text = Q2;
                QuestionCB[QuestionCB.Count - 1].AutoSize = true;
                QuestionCB[QuestionCB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q3 != "" && itIsCheckBox)
            {
                QuestionCB.Add(new CheckBox());
                QuestionCB[QuestionCB.Count - 1].Location = new Point(42, QuestionCB[QuestionCB.Count - 2].Location.Y + QuestionCB[QuestionCB.Count - 2].Height + 7);
                QuestionCB[QuestionCB.Count - 1].Visible = false;
                QuestionCB[QuestionCB.Count - 1].Text = Q3;
                QuestionCB[QuestionCB.Count - 1].AutoSize = true;
                QuestionCB[QuestionCB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q4 != "" && itIsCheckBox)
            {
                QuestionCB.Add(new CheckBox());
                QuestionCB[QuestionCB.Count - 1].Location = new Point(42, QuestionCB[QuestionCB.Count - 2].Location.Y + QuestionCB[QuestionCB.Count - 2].Height + 7);
                QuestionCB[QuestionCB.Count - 1].Visible = false;
                QuestionCB[QuestionCB.Count - 1].Text = Q4;
                QuestionCB[QuestionCB.Count - 1].AutoSize = true;
                QuestionCB[QuestionCB.Count - 1].CheckedChanged += CheckAnswer;
            }
            if (Q5 != "" && itIsCheckBox)
            {
                QuestionCB.Add(new CheckBox());
                QuestionCB[QuestionCB.Count - 1].Location = new Point(42, QuestionCB[QuestionCB.Count - 2].Location.Y + QuestionCB[QuestionCB.Count - 2].Height + 7);
                QuestionCB[QuestionCB.Count - 1].Visible = false;
                QuestionCB[QuestionCB.Count - 1].Text = Q5;
                QuestionCB[QuestionCB.Count - 1].AutoSize = true;
                QuestionCB[QuestionCB.Count - 1].CheckedChanged += CheckAnswer;
            }

            Questions.Add(Q1);
            Questions.Add(Q2);
            Questions.Add(Q3);
            Questions.Add(Q4);
            Questions.Add(Q5);
            if (pic != "")
            {
                this.Picture.Image = (Image)Resources.ResourceManager.GetObject(pic);
                if ((Image)Resources.ResourceManager.GetObject(pic) == null)
                {
                    try
                    {
                        Picture.Image = new Bitmap(Application.StartupPath + "\\" + pic);
                        //pathToPicture = Resources.ResourceManager.GetObject.pathToPicture
                    }
                    catch { }
                }
            }
            this.RightAnswer = RightAnswer;
        }


        /// <summary>
        /// проверка правильности ответа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CheckAnswer(object sender, EventArgs e)
        {
            if (!itIsCheckBox)
            {
                for (int i = 0; i < QuestionRB.Count; i++)
                {
                    if (QuestionRB[i].Checked)
                    {
                        Answer = (i+1).ToString();
                    }
                }
            }
            else
            {
                Answer = "";
                for (int i = 0; i < QuestionCB.Count; i++)
                {
                    if (QuestionCB[i].Checked)
                    {
                        Answer += (i + 1).ToString();
                    }
                }
            }
            if (Answer == RightAnswer)
            {
                RightOrNotAnswer = true;
            }else
            {
                RightOrNotAnswer = false;
            }
        }

    }
}
