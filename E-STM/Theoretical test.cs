using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using Xceed.Words.NET;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using E_STM;

namespace E_STM
{
    public partial class Theoretical_test : Form
    {
        public int TotalQ, RightQ;
        private int GeneralTime;
        private int LastTime;
        public int SaveTime =0;
        public int time1, time2, time3, time4, time5;
        bool GoBack, TestFinished;
        public int currentLineNumber = 1; // счётчик срок в документе Word.
        
        public Theoretical_test()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Theoretical_test_Load(object sender, EventArgs e)
        {
            EnableButton8.Enabled = true;
            

            this.ControlBox = false;
            GoBack = false;
            if (!(Application.OpenForms[0] as Form1).ViewLogout)
            {
                button6.Enabled = false;
            }
            int result;
            Application.OpenForms[0].Visible = false;
            // button1.BackColor = Color.Azure;
            label1.Text = (Application.OpenForms[0] as Form1).TextProg[54];
            Result.Text = (Application.OpenForms[0] as Form1).TextProg[53];
            button1.Text = (Application.OpenForms[0] as Form1).TextProg[55];
            button2.Text = (Application.OpenForms[0] as Form1).TextProg[56];
            button3.Text = (Application.OpenForms[0] as Form1).TextProg[57];
            button4.Text = (Application.OpenForms[0] as Form1).TextProg[58];
            button5.Text = (Application.OpenForms[0] as Form1).TextProg[59];
            button6.Text = (Application.OpenForms[0] as Form1).TextProg[52];
            label3.Text = (Application.OpenForms[0] as Form1).TextProg[84];
            button8.Text = (Application.OpenForms[0] as Form1).TextProg[155];

            int.TryParse(((Application.OpenForms[0] as Form1).textTimeDC.Text), out result);
            time1 = result;
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeAC.Text), out result);
            time2 = result;
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeMagn.Text), out result);
            time3 = result;
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeDigEl.Text), out result);
            time4 = result;
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeElMach.Text), out result);
            time5 = result;
            GeneralTime += result;
            string min = GeneralTime.ToString();
            if (GeneralTime < 10)
            {
                min = "0" + GeneralTime.ToString();
            }
            label17.Text = min + ":00";
            label3.Location = new Point(label17.Location.X - label3.Width, label3.Location.Y);
        }

        private void Theoretical_test_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Application.OpenForms.Count != 0 && !GoBack)
            {
                Application.OpenForms[0].Close();
                Questions Q = new Questions();
                Q.ShowDialog();
            }
            EnableButton8.Enabled = false;
            
        }

        private void clickButton(int numButton, Control button)
        {
            GeneralTheorTimer.Enabled = true;
            (Application.OpenForms[0] as Form1).NumberOfTypeQuestion = numButton;
            Questions Q = new Questions();
            Q.ShowDialog();
            button.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clickButton(1, button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clickButton(2, button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clickButton(3, button3);
        }

        // формирование отчёта
        private void button8_Click(object sender, EventArgs e)
        {
            List<ImageAndPositionInDoc> imagesForAddToWord = new List<ImageAndPositionInDoc>();
            //labelReportCreating.Visible = true;
            //try
            //{
            //    PasswordForReport passRep = new PasswordForReport();
            //    passRep.ShowDialog();

            //    if ((Application.OpenForms[0] as Form1).PassToReport == true)
            //    {
            //        //показать кнопку закрытия приложения
            //        button7.Visible = true;

            //        //создать отчет
            //        Word.Application word = new Word.Application();
            //        Word.Document doc = word.Documents.Add();
            //        var range = doc.Range();
            //        range.Font.Size = 14;
            //        range.Font.Bold = 0;

            //        // добавление в отчет тескта практических вопросов
            //        if (!(Application.OpenForms[0] as Form1).checkBox1.Checked)
            //        {

            //            range.Text = (Application.OpenForms[0] as Form1).TextProg[159] + (Application.OpenForms[0] as Form1).textBox2.Text + " " + (Application.OpenForms[0] as Form1).textBox3.Text;
            //            if ((Application.OpenForms[0] as Form1).ListOfPracticalQestion.Count > 0)
            //            {
            //                int numQuestion = 0;
            //                foreach (QuestionTest t in (Application.OpenForms[0] as Form1).ListOfPracticalQestion)
            //                {
            //                    range.Text += (Application.OpenForms[0] as Form1).TextProg[160] + " " + (Application.OpenForms[0] as Form1).ListOfPracticalQestion[numQuestion].name;
            //                    range.Text += " ---- ";
            //                    numQuestion += 1;
            //                    currentLineNumber++;
            //                    currentLineNumber++;
            //                }
            //            }
            //            doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            //        }

            //        //range.Text += " ";
            //        // добавление в отчет текста теор вопросов
            //        if (!(Application.OpenForms[0] as Form1).checkBox3.Checked)
            //        {
            //            range.Text += (Application.OpenForms[0] as Form1).TextProg[161];
            //            range.Text += (Application.OpenForms[0] as Form1).TextProg[162];
            //            currentLineNumber++;
            //            currentLineNumber++;

            //            // DC часть
            //            addTextOfChapterInReport((Application.OpenForms[0] as Form1).DCchapter, range, imagesForAddToWord);
            //            doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            //            currentLineNumber++;

            //            // АС часть
            //            range.Text += (Application.OpenForms[0] as Form1).TextProg[163];
            //            currentLineNumber++;
            //            addTextOfChapterInReport((Application.OpenForms[0] as Form1).ACchapter, range, imagesForAddToWord);
            //            doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            //            currentLineNumber++;

            //            // часть магнетизм
            //            range.Text += (Application.OpenForms[0] as Form1).TextProg[164];
            //            currentLineNumber++;
            //            addTextOfChapterInReport((Application.OpenForms[0] as Form1).MagnetChapter, range, imagesForAddToWord);
            //            doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            //            currentLineNumber++;

            //            // часть цифновая электроника
            //            range.Text += (Application.OpenForms[0] as Form1).TextProg[165];
            //            currentLineNumber++;
            //            addTextOfChapterInReport((Application.OpenForms[0] as Form1).DigitChapter, range, imagesForAddToWord);
            //            doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            //            currentLineNumber++;

            //            // часть Електрические машины
            //            range.Text += (Application.OpenForms[0] as Form1).TextProg[166];
            //            currentLineNumber++;
            //            addTextOfChapterInReport((Application.OpenForms[0] as Form1).MachinChapter, range, imagesForAddToWord);
            //            //doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
            //            currentLineNumber++;
            //        }

            //        // Добавить в конец текста, прошел студент тест или нет
            //        range.Text += "\n";
            //        range.Text += (Application.OpenForms[0] as Form1).textBox2.Text + " " + (Application.OpenForms[0] as Form1).textBox3.Text + " " + labelPassOrNot.Text;

            //        // добавление картинок в отчёт
            //        if (imagesForAddToWord.Count > 0)
            //        {
            //            var count = 0;
            //            if (!(Application.OpenForms[0] as Form1).checkBox1.Checked) count = -2;

            //            foreach(ImageAndPositionInDoc image in imagesForAddToWord)
            //            {
            //                object missing = Type.Missing;
            //                Clipboard.SetImage(image.image);   
            //                word.ActiveDocument.Paragraphs[image.position - count].Range.Paste();
            //                count++;
            //            }
            //        }



            //        // добавление в отчет картинок практических заданий, которые нарисовал студент
            //        if (!(Application.OpenForms[0] as Form1).checkBox1.Checked)
            //        {
            //            Object missing = Type.Missing;

            //            int numPsatPosition = 3;
            //            int numPicInCollection = 1;
            //            foreach (QuestionTest t in (Application.OpenForms[0] as Form1).ListOfPracticalQestion)
            //            {
            //                Clipboard.SetImage((Application.OpenForms[0] as Form1).bitmapPracticSolution[numPicInCollection]);
            //                word.ActiveDocument.Paragraphs[numPsatPosition].Range.Paste();
            //                numPsatPosition += 1;
            //                numPicInCollection += 1;
            //            }
            //        }

            //        // сохратить отчет и закрыть приложение.
            //        doc.SaveAs2(Application.StartupPath + "\\Report from test of student " + (Application.OpenForms[0] as Form1).textBox2.Text + " " + (Application.OpenForms[0] as Form1).textBox3.Text + ".doc");

            //        //показать кнопку закрытия приложения
            //        button7.Visible = true;

            //        //закрыть документ и word
            //        doc.Close();
            //        word.Quit();
            //        labelReportCreating.Visible = false;

            //        if (MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[167], "Close application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        {
            //            Application.Exit();
            //        }
            //    }
            //}catch(Exception e1)
            //{
            //    MessageBox.Show(e1.Message);

            //}

            //Location Path  
            string fileName = Application.StartupPath + "\\Report_student " + (Application.OpenForms[0] as Form1).textBox2.Text + " " + (Application.OpenForms[0] as Form1).textBox3.Text + ".docx";

            //Formatting Title1  
            Formatting titleFormat = new Formatting();
            //Specify font family  
            titleFormat.FontFamily = new Xceed.Words.NET.Font("Batang");
            //Specify font size  
            titleFormat.Size = 18;
            titleFormat.Position = 40;
            titleFormat.FontColor = System.Drawing.Color.Red;
            //titleFormat.UnderlineColor = System.Drawing.Color.Gray;
            titleFormat.Italic = true;

            //Formatting Title2  
            Formatting titleFormat2 = new Formatting();
            //Specify font family  
            titleFormat2.FontFamily = new Xceed.Words.NET.Font("Batang");
            //Specify font size  
            titleFormat2.Size = 10;
            //titleFormat2.Position = 40;
            //titleFormat2.FontColor = System.Drawing.Color.Gray;
            titleFormat2.Bold = true;
            //titleFormat.UnderlineColor = System.Drawing.Color.Gray;
            titleFormat2.Italic = false;

            //Formatting Text Paragraph  
            Formatting textParagraphFormat = new Formatting();
            //font family  
            textParagraphFormat.FontFamily = new Xceed.Words.NET.Font("Century Gothic");
            //font size  
            textParagraphFormat.Size = 10;
            //Spaces between characters  
            textParagraphFormat.Spacing = 1;

            //Create docx  
            var doc = DocX.Create(fileName);


            //Insert text
            //Insert summary

            var textAboutPsaaetOrNot = (Application.OpenForms[0] as Form1).TextProg[159] + (Application.OpenForms[0] as Form1).textBox2.Text + " " + (Application.OpenForms[0] as Form1).textBox3.Text;
            Paragraph paragraphSummary = doc.InsertParagraph("Summary.", false, titleFormat);
            paragraphSummary.Alignment = Alignment.center;

            doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[161] ,false, titleFormat2);
            doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[162] + ": " + label4.Text + " - Total/Right", false, textParagraphFormat);
            doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[163] + ": " + label5.Text + " - Total/Right", false, textParagraphFormat);
            doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[164] + ": " + label6.Text + " - Total/Right", false, textParagraphFormat);
            doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[165] + ": " + label7.Text + " - Total/Right", false, textParagraphFormat);
            doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[166] + ": " + label8.Text + " - Total/Right", false, textParagraphFormat);
            var totalSum = 0;
            var totalRight = 0;

            string[] totalAndRight = label4.Text.Split(new char[] { '/' });
            totalSum += int.Parse(totalAndRight[0]);
            totalRight += int.Parse(totalAndRight[1]);
            totalAndRight = label5.Text.Split(new char[] { '/' });
            totalSum += int.Parse(totalAndRight[0]);
            totalRight += int.Parse(totalAndRight[1]);
            totalAndRight = label6.Text.Split(new char[] { '/' });
            totalSum += int.Parse(totalAndRight[0]);
            totalRight += int.Parse(totalAndRight[1]);
            totalAndRight = label7.Text.Split(new char[] { '/' });
            totalSum += int.Parse(totalAndRight[0]);
            totalRight += int.Parse(totalAndRight[1]);
            totalAndRight = label8.Text.Split(new char[] { '/' });
            totalSum += int.Parse(totalAndRight[0]);
            totalRight += int.Parse(totalAndRight[1]);

            var percents = (int)((double)totalRight / (double)totalSum * 100.0);

            doc.InsertParagraph("General Total: " + totalSum.ToString() + "/" + totalRight + " it is:" + percents.ToString() + " percents.", false, titleFormat2);
            doc.InsertParagraph("Goal is: " + (Application.OpenForms[0] as Form1).textBox1.Text, false, textParagraphFormat);
            doc.InsertParagraph((Application.OpenForms[0] as Form1).textBox2.Text + " " + (Application.OpenForms[0] as Form1).textBox3.Text + " " + labelPassOrNot.Text, false, textParagraphFormat);


            //Insert practical           
            if (!(Application.OpenForms[0] as Form1).checkBox1.Checked)
            {
                //Insert title practical test
                Paragraph paragraphTitle = doc.InsertParagraph("Practical test.", false, titleFormat);
                paragraphTitle.Alignment = Alignment.center;

                if ((Application.OpenForms[0] as Form1).ListOfPracticalQestion.Count > 0)
                {
                    int numQuestion = 0;
                    foreach (QuestionTest t in (Application.OpenForms[0] as Form1).ListOfPracticalQestion)
                    {
                        var TextQ = (numQuestion + 1).ToString() + ". " + (Application.OpenForms[0] as Form1).TextProg[160] + " " + (Application.OpenForms[0] as Form1).ListOfPracticalQestion[numQuestion].name;
                        //doc.InsertParagraph(TextQ, false, textParagraphFormat);//text

                        //Insert picture
                        string path = Application.StartupPath + @"\img" + numQuestion.ToString() + ".jpg";
                        (Application.OpenForms[0] as Form1).bitmapPracticSolution[numQuestion+1].Save(path);//сохранить в файл
                        //Create a picture  
                        Xceed.Words.NET.Image img = doc.AddImage(path);
                        Picture p = img.CreatePicture();
                        p.Width = 600;
                        p.Height = 400;

                        //Create a new paragraph  
                        doc.InsertParagraph(TextQ, false, titleFormat2);
                        Paragraph par = doc.InsertParagraph("");
                        par.AppendPicture(p);

                        //Delete file
                        FileInfo fileInf = new FileInfo(path);
                        if (fileInf.Exists) { fileInf.Delete(); }

                        //doc.InsertParagraph(" ------------- ", false, textParagraphFormat);
                        doc.InsertSectionPageBreak();//separator
                        numQuestion += 1;

                    }
                }
            }

            //Insert theoretical
            if (!(Application.OpenForms[0] as Form1).checkBox3.Checked)
            {
                //Insert title theoretical test
                Paragraph paragraphTitle = doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[161], false, titleFormat);
                paragraphTitle.Alignment = Alignment.center;

                doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[162], false, titleFormat2);
                addContextQuestionOfChapter((Application.OpenForms[0] as Form1).DCchapter, doc, textParagraphFormat);
                doc.InsertSectionPageBreak();//separator

                doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[163], false, titleFormat2);
                addContextQuestionOfChapter((Application.OpenForms[0] as Form1).ACchapter, doc, textParagraphFormat);
                doc.InsertSectionPageBreak();//separator

                doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[164], false, titleFormat2);
                addContextQuestionOfChapter((Application.OpenForms[0] as Form1).MagnetChapter, doc, textParagraphFormat);
                doc.InsertSectionPageBreak();//separator

                doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[165], false, titleFormat2);
                addContextQuestionOfChapter((Application.OpenForms[0] as Form1).DigitChapter, doc, textParagraphFormat);
                doc.InsertSectionPageBreak();//separator

                doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[166], false, titleFormat2);
                addContextQuestionOfChapter((Application.OpenForms[0] as Form1).MachinChapter, doc, textParagraphFormat);

            }

            doc.Save();

            //показать кнопку закрытия приложения
            button7.Visible = true;

            if (MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[167], "Close application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }



        // добавление тескста вопросов в отчет
        private void addContextQuestionOfChapter(List<TheorQuestion> chapter, DocX doc, Formatting format)
        {
            int numberQuestion = 1;
            foreach (TheorQuestion question in chapter)
            {
                doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[168] + numberQuestion.ToString() + " " + question.Name, false, format);
                if (question.Picture.Image != null)
                {

                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    question.Picture.Image.Save(stream, ImageFormat.Jpeg);
                    stream.Seek(0, SeekOrigin.Begin);

                    ////Insert picture
                    //string path = Application.StartupPath + @"\img.jpg";
                    //try
                    //{                     
                    //    question.Picture.Image.Save(path);//сохранить в файл
                    //}
                    //catch
                    //{
                    //    //MessageBox.Show("Can not create file.");

                    //}

                    try
                    {
                        //Create a picture  
                        Xceed.Words.NET.Image img = doc.AddImage(stream); ;
                        Picture p = img.CreatePicture();
                        var w = question.Picture.Image.Width;
                        var h = question.Picture.Image.Height;
                        if (w > 400)
                        {
                            p.Width = 400;
                            p.Height = 400 * h / w;
                        }
                        else if (h > 280)
                        {
                            p.Height = 280;
                            p.Width = 280 * w / h;
                        }

                        //Create a new paragraph  
                        Paragraph par = doc.InsertParagraph("");
                        par.AppendPicture(p);
                    } catch
                    {
                        MessageBox.Show("Occuped error in during adding picture into report.");
                    };                                                                                  
                    

                    ////Delete file
                    //try
                    //{
                    //    FileInfo fileInf = new FileInfo(path);
                    //    if (fileInf.Exists) { fileInf.Delete(); }
                    //}
                    //catch
                    //{
                    //    //MessageBox.Show("Can not delete file.");
                    //}
                    
                }

                int num = 1;
                if (question.itIsCheckBox)
                {
                    foreach (CheckBox cb in question.QuestionCB)
                    {
                        doc.InsertParagraph(num.ToString() + ". " + cb.Text, false, format);
                        num++;
                    }

                }
                else
                {
                    foreach (RadioButton rb in question.QuestionRB)
                    {
                        doc.InsertParagraph(num.ToString() + ". " + rb.Text, false, format);
                        num++;
                    }
                }
                if (question.timeSpendOnQuestion < 60)
                {
                    doc.InsertParagraph("Aproximate time for answering a question = " + question.timeSpendOnQuestion.ToString() + " sec.;", false, format);
                }
                else
                {
                    int min = question.timeSpendOnQuestion / 60;
                    int sec = question.timeSpendOnQuestion - (min * 60);
                    doc.InsertParagraph("Aproximate time for answering a question = " + min.ToString() + "min. " + sec.ToString() + " sec.;", false, format);
                }

                doc.InsertParagraph(" --------------------------- ", false, format);

                //Formatting Text Green  
                Formatting Green = new Formatting();
                //font family  
                Green.FontFamily = new Xceed.Words.NET.Font("Century Gothic");
                //font size  
                Green.Size = 10;
                Green.FontColor = Color.Green;
                //Spaces between characters  
                Green.Spacing = 1;

                //Formatting Text Red  
                Formatting Red = new Formatting();
                //font family  
                Red.FontFamily = new Xceed.Words.NET.Font("Century Gothic");
                //font size  
                Red.Size = 10;
                Red.FontColor = Color.Red;
                //Spaces between characters  
                Red.Spacing = 1;

                if (question.RightOrNotAnswer)
                {

                    doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[169] + " " + question.Answer + (Application.OpenForms[0] as Form1).TextProg[170], false, Green);
                }
                else
                {

                    doc.InsertParagraph((Application.OpenForms[0] as Form1).TextProg[169] + " " + question.Answer + (Application.OpenForms[0] as Form1).TextProg[171], false, Red);
                }

                numberQuestion++;
            }
            
        }

        //// добавление тескста вопросов в отчет
        //private void addTextOfChapterInReport (List<TheorQuestion> chapter, Microsoft.Office.Interop.Word.Range range, List<ImageAndPositionInDoc> images)
        //{
        //    int numberQuestion = 1;
        //    foreach (TheorQuestion question in chapter)
        //    {

        //        range.Text += (Application.OpenForms[0] as Form1).TextProg[168] + numberQuestion.ToString();
        //        range.Text += question.Name;
        //        currentLineNumber = currentLineNumber + 2;

        //        if (question.Picture.Image != null)
        //        {
        //            //Bitmap bitMap = new Bitmap(question.Picture.Image);
        //            //Object missing = Type.Missing;
        //            //word.Documents.Add(ref missing, ref missing, ref missing, ref missing);
        //            //Clipboard.SetImage(bitMap);
        //            //Clipboard.SetImage((Application.OpenForms[0] as Form1).bitmapPracticSolution[numPicInCollection]);
        //            //word.ActiveDocument.Paragraphs[1].Range.Paste();
        //            //word.Visible = true;
        //            range.Text += "<" + images.Count.ToString() + ">";
        //            range.Text += "\n";
        //            currentLineNumber++;
                    
        //            images.Add(new ImageAndPositionInDoc (question.Picture.Image, currentLineNumber));
        //            currentLineNumber++;

        //        }


        //        int num = 1;
        //        if (question.itIsCheckBox)
        //        {
        //            foreach (CheckBox cb in question.QuestionCB)
        //            {
        //                range.Text += num.ToString() + ". " + cb.Text;
        //                num++;
        //                currentLineNumber++;
        //            }
                    
        //        }
        //        else
        //        {
        //            foreach (RadioButton rb in question.QuestionRB)
        //            {
        //                range.Text += num.ToString() + ". " + rb.Text;
        //                num++;
        //                currentLineNumber++;
        //            }
        //        }
        //        if (question.timeSpendOnQuestion < 60)
        //        {
        //            range.Text += "Aproximate time for answering a question = " + question.timeSpendOnQuestion.ToString() + " sec.;";
        //        }
        //        else
        //        {
        //            int min = question.timeSpendOnQuestion / 60;
        //            int sec = question.timeSpendOnQuestion - (min * 60);
        //            range.Text += "Aproximate time for answering a question = " + min.ToString() + "min. " + sec.ToString() + " sec.;";
        //        }
                
        //        range.Text += " --------------------------- ";
        //        currentLineNumber++;
        //        currentLineNumber++;
        //        if (question.RightOrNotAnswer)
        //        {
                    
        //            range.Text += (Application.OpenForms[0] as Form1).TextProg[169] + " " + question.Answer + (Application.OpenForms[0] as Form1).TextProg[170];
        //            range.Font.Color = Word.WdColor.wdColorGreen;
        //            currentLineNumber++;
        //        }
        //        else
        //        {
                    
        //            range.Text += (Application.OpenForms[0] as Form1).TextProg[169] + " " + question.Answer + (Application.OpenForms[0] as Form1).TextProg[171];
        //            range.Font.Color = Word.WdColor.wdColorRed;
        //            currentLineNumber++;
        //        }
        //        numberQuestion++;
        //        range.Text += "\n";
        //        //numberQuestion++;
        //        currentLineNumber++;
        //        range.Font.Color = Word.WdColor.wdColorBlack;
        //    }
        //}

        private void EnableButton8_Tick(object sender, EventArgs e)
        {
            if (!button1.Enabled && !button2.Enabled && !button3.Enabled && !button4.Enabled && !button5.Enabled)
            {
                button8.Enabled = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void labelPassOrNot_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            clickButton(4, button4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clickButton(5, button5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            GoBack = true;
            Application.OpenForms[0].Visible = true;
            Close();
        }

        private void Result_Click(object sender, EventArgs e)
        {

        }

        private void GeneralTheorTimer_Tick(object sender, EventArgs e)
        {
            int min, sec;
            LastTime++;
            if (GeneralTime * 60 - LastTime == 0)
            {
                GeneralTheorTimer.Enabled = false;
                return;
            } else
            {
                string mins, secs;
                min = (GeneralTime*60 - LastTime)/60;
                sec = 60 - LastTime % 60 - 1;
                if (min < 10)
                {
                    mins = "0" + min.ToString();
                }else
                {
                    mins = min.ToString();
                }
                if (sec < 10)
                {
                    secs = "0" + sec.ToString();
                }else
                {
                    secs = sec.ToString();
                }
                label17.Text = mins + ":" + secs;
            }
            if (min == 0 && sec <= 0)
            {
                GeneralTheorTimer.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
            }
            if (!button1.Enabled && !button2.Enabled && !button3.Enabled && !button4.Enabled && !button5.Enabled && !TestFinished)
            {
                TestFinished = true;
                GeneralTheorTimer.Enabled = false;
                String[] results = label2.Text.Split(new char[] { '/' });
                int procents = 0;
                if (int.Parse(results[1]) > 0)
                {
                    procents = (int)(double.Parse(results[1]) / double.Parse(results[0]) * 100);
                }
                labelPassOrNot.Visible = true;
                if (procents >= (Application.OpenForms[0] as Form1).RightAnswerInProcents)
                {
                    labelPassOrNot.ForeColor = Color.Green;
                    labelPassOrNot.Text = (Application.OpenForms[0] as Form1).TextProg[156];
                }else
                {
                    labelPassOrNot.ForeColor = Color.Red;
                    labelPassOrNot.Text = (Application.OpenForms[0] as Form1).TextProg[157];
                }

                MessageBox.Show((Application.OpenForms[0] as Form1).TextProg[158], "Finish!", MessageBoxButtons.OK);
                button8.Visible = true;           
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
