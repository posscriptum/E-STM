using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Data;
using Word = Microsoft.Office.Interop.Word;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Drawing.Imaging;

namespace E_STM
{
    public partial class Form1 : Form
    {
        public bool PassToReport;
        public string PassForReport = "";
        public int NrQuestions1, NrQuestions2, NrQuestions3, NrQuestions4, NrQuestions5;
        public bool PushedNext;
        public bool ShortCircuit;

        //массив с картинками решений практичкских задач
        public List<Bitmap> bitmapPracticSolution = new List<Bitmap>();
        // массиы с вопросами и ответами для теоретических заданий
        public List<TheorQuestion> DCchapter = new List<TheorQuestion>();
        public List<TheorQuestion> ACchapter = new List<TheorQuestion>();
        public List<TheorQuestion> MagnetChapter = new List<TheorQuestion>();
        public List<TheorQuestion> DigitChapter = new List<TheorQuestion>();
        public List<TheorQuestion> MachinChapter = new List<TheorQuestion>();

        // времена выполнения заданий для практических заданий
        public List<DateAndTime> TimeOfPractic = new List<DateAndTime>();
        // времена выполнения теор. заданий
        public List<DateAndTime> TimeOfTheor = new List<DateAndTime>();
 
        OleDbConnection dbc;
        OleDbDataAdapter da;

        public string Lang = "English";

        private string PracticalQuestion = "practical_question_en";
        private bool SetTextSecondTime;

        public List<ElementNew> ElemensPractical = new List<ElementNew>();
        public List<ConnectorNew> ConectorPractical = new List<ConnectorNew>();

        public List<ElementNew> ElemensPrograming = new List<ElementNew>();
        public List<ConnectorNew> ConectorPrograming = new List<ConnectorNew>();

        private ConnectPoint prevPointConnect;
        private ElementNew prevElement;
        private Point PointPlaceOnPanel = new Point(100, 50);

        private ConnectPoint prevPointConnectProg;
        private ElementNew prevElementProg;
        private Point PointPlaceOnPanelProg = new Point(100, 50);

        private bool ButtonMoved;
        private bool ClickDownOnPanel;

        private bool ButtonMovedProg;
        private bool ClickDownOnPanelProg;

        private Fase U, V, W;
        private double timeFase;
        private bool KZ;
        private List<List<ConnectorNew>> PrevList = new List<List<ConnectorNew>>();

        public ElementNew ElementForChangeTime;
        private bool DrawLineFromPointToMouse;
        private Point PointOfMouse;

        Point PrewMousePoint;
        Graphics graph;

        string username;
        public bool ViewLogout = true;
        public int RightAnswerInProcents;
        public List<string> fliesnameOfTasks = new List<string>();
        public string pathToTaskFile;



        // Коллекция вопросов теор. теста
        List<String> CollectionOfQuestion = new List<String>();
        List<bool> CollectionOfAnswers = new List<bool>();
        int countQestion = 0;
        Timer TimerForTheorQuestion = new Timer();
        //массив конторолов для вопроса
        List<Control> ControlsForQestion = new List<Control>();
        string rightAnsver, Answer;
        int howMuchTimeToEndQuestion, howMuchTimeToEndQuestionProgTest, howMuchTimeToEndQuestionElTest;
        int timeForAnswer, timeForAnswerProg, timeForAnswerEl;
        bool theorTestInProgress, ProgTestInProgress;
        public bool ElTestInProgress;
        string fileName;
        bool TestMode = true;
        int calculateTickDisapier;
        // bool elementConnected;
        bool pass;
        TabPage Tab1, Tab2, Tab3, Tab4;
        public List<QuestionTest> ListOfPracticalQestion = new List<QuestionTest>();
        List<int> ListOfTimeForQestion = new List<int>();
        int numberQuestion;
        public List<string> TextProg = new List<string>();
        public int NumberOfTypeQuestion;
        public bool buttonQ1, buttonQ2, buttonQ3, buttonQ4, buttonQ5;
        private int NumberLang = 0;

        //главный таймер в отдельном потоке
        public System.Timers.Timer GeneralTimer = new System.Timers.Timer();
        public string fileNameTexts;

        public int returnedNumberSelectedQuestion;
        public List<QuestionTest> listAllPracticalQuestion = new List<QuestionTest>();

        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedDialog;

        }

        // Создание соединения на новой панели
        public void createConnector(ConnectPoint pointConnect, ElementNew element)
        {
            if (prevPointConnect == pointConnect)
            {
                prevPointConnect = null;
                return;
            }
            if (prevPointConnect == null)
            {
                prevPointConnect = pointConnect;
                prevElement = element;
                DrawLineFromPointToMouse = true;
            }
            else
            {
                ConectorPractical.Add(new ConnectorNew(prevPointConnect, pointConnect, panelPractical, prevElement, element));
                prevPointConnect = null;
                DrawLineFromPointToMouse = false;
            }
        }

        // Создание соединения на новой панели программирования
        public void createConnectorProg(ConnectPoint pointConnect, ElementNew element)
        {
            if (prevPointConnect == pointConnect)
            {
                prevPointConnect = null;
                return;
            }
            if (prevPointConnect == null)
            {
                prevPointConnect = pointConnect;
                prevElement = element;
                DrawLineFromPointToMouse = true;
            }
            else
            {
                ConectorPrograming.Add(new ConnectorNew(prevPointConnect, pointConnect, panelPrograming, prevElement, element));
                prevPointConnect = null;
                DrawLineFromPointToMouse = false;
            }
        }

        private void contextMenu(object sender, MouseEventArgs e)
        {
            // проверяем, не кликнули-ли правой кнопкой на каком-нибудь элементе
            if (ElemensPractical.Count > 0 && e.Button == MouseButtons.Right)
            {
                foreach (ElementNew element in ElemensPractical)
                {
                    Point point = new Point(e.X, e.Y);
                    if (element.RectOfElement.Contains(point) && e.X > 75)
                    {
                        //создать контекстное меню для этого элемента
                        ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
                        //создать пункт Delete для этого контекстного меню
                        ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("Delete");
                        // что делать если это меню выбрали
                        tsmiDelete.Click += (object sender1, EventArgs e1) => DeleteElementNew(element);
                        ContextMenuForPicToElement.Items.Add(tsmiDelete);
                        //добавляем контекстное меню к элементу
                        panelPractical.ContextMenuStrip = ContextMenuForPicToElement;
                    }
                }

                foreach (ConnectorNew connector in ConectorPractical)
                {
                    Point point = new Point(e.X, e.Y);
                    if (checkClickonConnector(point, connector.CurrentSetPoint))
                    {
                        //создать контекстное меню для этого элемента
                        ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
                        //создать пункт Delete для этого контекстного меню
                        ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("Delete");
                        // что делать если это меню выбрали
                        tsmiDelete.Click += (object sender1, EventArgs e1) => deliteConnector(connector);
                        ContextMenuForPicToElement.Items.Add(tsmiDelete);
                        //добавляем контекстное меню к элементу
                        panelPractical.ContextMenuStrip = ContextMenuForPicToElement;
                    }
                }
            }
            else
            {
                ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
                panelPractical.ContextMenuStrip = ContextMenuForPicToElement;
                panelPractical.Invalidate();
            }

            ClickDownOnPanel = true;

        }

        // удаляем соединение
        private void deliteConnector(ConnectorNew connector)
        {
            ConectorPractical.Remove(connector);
            connector.deliteConnector();
            connector = null;
            ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
            panelPractical.ContextMenuStrip = ContextMenuForPicToElement;
            panelPractical.Invalidate();

        }

        // удаляем соединение
        private void deliteConnectorProg(ConnectorNew connector)
        {
            ConectorPrograming.Remove(connector);
            connector.deliteConnector();
            connector = null;
            ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
            panelPrograming.ContextMenuStrip = ContextMenuForPicToElement;
            panelPrograming.Invalidate();

        }

        //проверка не кликнули-ли на соединении
        private bool checkClickonConnector(Point pointClick, List<Point> listPointFormConnector)
        {
            if (listPointFormConnector != null && listPointFormConnector.Count > 0)
            {
                for (int i = 0; i < listPointFormConnector.Count - 1; i++)
                {
                    Rectangle scanRect = new Rectangle();
                    if (listPointFormConnector[i].X == listPointFormConnector[i + 1].X)
                    {
                        scanRect = new Rectangle(listPointFormConnector[i].X - 5, Math.Min(listPointFormConnector[i].Y, listPointFormConnector[i + 1].Y), 10, Math.Abs(listPointFormConnector[i].Y - listPointFormConnector[i + 1].Y));
                    }
                    if (listPointFormConnector[i].Y == listPointFormConnector[i + 1].Y)
                    {
                        scanRect = new Rectangle(Math.Min(listPointFormConnector[i].X, listPointFormConnector[i + 1].X), listPointFormConnector[i].Y - 5, Math.Abs(listPointFormConnector[i].X - listPointFormConnector[i + 1].X), 10);
                    }
                    if (scanRect.Contains(pointClick))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        // Удаление элемента вместе с соединениями к нему подключенными
        private void DeleteElementNew(ElementNew element)
        {

            if (ConectorPractical.Count > 0)
            {
                List<ConnectorNew> tempListForDelite = new List<ConnectorNew>();
                foreach (ConnectorNew conector in ConectorPractical)
                {
                    if (conector.Element1 == element || conector.Element2 == element)
                    {
                        tempListForDelite.Add(conector);
                        conector.deliteConnector();
                        panelPractical.Invalidate();
                    }
                }
                if (tempListForDelite.Count > 0)
                {
                    foreach (ConnectorNew connector in tempListForDelite)
                    {
                        ConectorPractical.Remove(connector);
                    }
                }
            }
            ElemensPractical.Remove(element);
            element.deliteElement();
            element = null;
            ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
            panelPractical.ContextMenuStrip = ContextMenuForPicToElement;
            panelPractical.Invalidate();
        }

        // Удаление элемента вместе с соединениями к нему подключенными из панели программирования
        private void DeleteElementNewProg(ElementNew element)
        {

            if (ConectorPrograming.Count > 0)
            {
                List<ConnectorNew> tempListForDelite = new List<ConnectorNew>();
                foreach (ConnectorNew conector in ConectorPrograming)
                {
                    if (conector.Element1 == element || conector.Element2 == element)
                    {
                        tempListForDelite.Add(conector);
                        conector.deliteConnector();
                        panelPrograming.Invalidate();
                    }
                }
                if (tempListForDelite.Count > 0)
                {
                    foreach (ConnectorNew connector in tempListForDelite)
                    {
                        ConectorPrograming.Remove(connector);
                    }
                }
            }
            ElemensPrograming.Remove(element);
            element.deliteElement();
            element = null;
            ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
            panelPrograming.ContextMenuStrip = ContextMenuForPicToElement;
            panelPrograming.Invalidate();
        }

        // расчет точки появления элемента на панели практики
        private void changePointOnPanel(ElementNew element)
        {
           // PointPlaceOnPanel.X += 100;
            foreach (ElementNew el in ElemensPractical)
            {
                if (element.RectOfElement.IntersectsWith(el.RectOfElement) && element != el)
                {
                    PointPlaceOnPanel.X = el.RectOfElement.X + el.RectOfElement.Width + 10;
                    PointPlaceOnPanel.Y = element.RectOfElement.Y;
                    if (PointPlaceOnPanel.X >= panelPractical.Width - 100)
                    {
                        PointPlaceOnPanel.X = 100;
                        PointPlaceOnPanel.Y += 100;
                    }
                    element.RectOfElement = new Rectangle(PointPlaceOnPanel.X, PointPlaceOnPanel.Y, element.RectOfElement.Width, element.RectOfElement.Height);
                    //panelDiagram.Invalidate();
                }
            }
            if (PointPlaceOnPanel.X >= panelPractical.Width - 100)
            {
                PointPlaceOnPanel.X = 100;
                PointPlaceOnPanel.Y += 100;
            }
        }

        // расчет точки появления элемента на панели практики
        private void changePointOnPanelPrograming(ElementNew element)
        {
            // PointPlaceOnPanel.X += 100;
            foreach (ElementNew el in ElemensPrograming)
            {
                if (element.RectOfElement.IntersectsWith(el.RectOfElement) && element != el)
                {
                    PointPlaceOnPanelProg.X = el.RectOfElement.X + el.RectOfElement.Width + 10;
                    PointPlaceOnPanelProg.Y = element.RectOfElement.Y;
                    if (PointPlaceOnPanelProg.X >= panelPrograming.Width - 100)
                    {
                        PointPlaceOnPanelProg.X = 100;
                        PointPlaceOnPanelProg.Y += 100;
                    }
                    element.RectOfElement = new Rectangle(PointPlaceOnPanelProg.X, PointPlaceOnPanelProg.Y, element.RectOfElement.Width, element.RectOfElement.Height);
                    panelPrograming.Invalidate();
                }
            }
            if (PointPlaceOnPanelProg.X >= panelPrograming.Width - 100)
            {
                PointPlaceOnPanelProg.X = 100;
                PointPlaceOnPanelProg.Y += 100;
            }
        }


        public void timer1_Tick(object sender, EventArgs e)
        {

            //только если прогресс запущен, включаем таймер
            if (theorTestInProgress == true)
            {
                int min, sec;
                string minutes, secundes;

                // даем время на ответ
                timeForAnswer--;

                min = timeForAnswer / 600;
                if (min < 10)
                {
                    minutes = "0" + min.ToString();
                }
                else
                {
                    minutes = min.ToString();
                }

                sec = (timeForAnswer - min * 600) / 10;
                if (sec < 10)
                {
                    secundes = "0" + sec.ToString();
                }
                else
                {
                    secundes = sec.ToString();
                }

                //TimerForTheorTest.Text = minutes + ":" + secundes;
                // время вышло
                if (timeForAnswer <= 0)
                {
                    timeForAnswer = howMuchTimeToEndQuestion;
                    CollectionOfAnswers.Add(false);
                    Answer = "";
                    rightAnsver = "";

                    // удалить элементы вопроса
                    foreach (Control i in ControlsForQestion)
                    {
                        tabTheoretical.Controls.Remove(i);
                    }
                    ControlsForQestion.Clear();
                    ImplementationTheorTest();
                }
            }

                //только если прогресс запущен, включаем таймер
                if (ProgTestInProgress == true)
                {
                    int min, sec;
                    string minutes, secundes;

                    // даем время на ответ
                    if(timeForAnswerProg >= 0) timeForAnswerProg--;

                    min = timeForAnswerProg / 600;
                    if (min < 10)
                    {
                        minutes = "0" + min.ToString();
                    }
                    else
                    {
                        minutes = min.ToString();
                    }

                    sec = (timeForAnswerProg - min * 600) / 10;
                    if (sec < 10)
                    {
                        secundes = "0" + sec.ToString();
                    }
                    else
                    {
                        secundes = sec.ToString();
                    }

                   // TimerForProgTest.Text = minutes + ":" + secundes;

                    // время вышло
                    if (timeForAnswerProg <= 0)
                    {
                    NextQuestion();
                    //button7.Enabled = false;
                }
                }

            //только если прогресс запущен, включаем таймер
            if (ElTestInProgress == true)
            {
                int min, sec;
                string minutes, secundes;

                // даем время на ответ
                if (timeForAnswerEl >= 0) timeForAnswerEl--;

                min = timeForAnswerEl / 600;
                if (min < 10)
                {
                    minutes = "0" + min.ToString();
                }
                else
                {
                    minutes = min.ToString();
                }

                sec = (timeForAnswerEl - min * 600) / 10;
                if (sec < 10)
                {
                    secundes = "0" + sec.ToString();
                }
                else
                {
                    secundes = sec.ToString();
                }

                //TimerForElTest.Text = minutes + ":" + secundes;

                // время вышло
                if (timeForAnswerEl <= 0)
                {
                    
                    NextQuestion();
                    //button7.Enabled = false;
                    CalculateSignal.Enabled = false;
                }
            }
        }


        private void panelPractical_MouseUp(object sender, MouseEventArgs e)
        {
            // cостояния кнопок изменяю, если что
            foreach (ElementNew element in ElemensPractical)
            {
                if (element is Button)
                {
                    Rectangle rect = new Rectangle(element.RectOfElement.X + 20, element.RectOfElement.Y + 5, element.RectOfElement.Width - 40, element.RectOfElement.Height - 25);
                    if (rect.Contains(e.X, e.Y) && !ButtonMoved)
                    {
                        (element as Button).ButtonOPN = !(element as Button).ButtonOPN;
                        (element as Button).implementationB();
                        

                    }
                }
            }
            ClickDownOnPanel = false;
            ButtonMoved = false;
            panelPractical.Invalidate();
        }


        private void panelPractical_MouseMove(object sender, MouseEventArgs e)
        {
            if (ClickDownOnPanel) { ButtonMoved = true; }
        }

        private object SenderLoad;
        private EventArgs ELoad;
        private ToolTip Hint = new ToolTip();
        private void Form1_Load(object sender, EventArgs e)
        {
            //button9.Click += button17.Click;
            //comboBoxLang.DataBindings.Add("Text", BindingModelForComboBox, "Value", false, DataSourceUpdateMode.OnPropertyChanged);
            checkBox1.Click += (object sender1, EventArgs e1) => checkPractical();
            checkBox4.Click += (object sender1, EventArgs e1) => checkPractical();
            checkBox3.Click += (object sender1, EventArgs e1) => checkTheoretical();
            checkBox5.Click += (object sender1, EventArgs e1) => checkTheoretical();

            textBox4.UseSystemPasswordChar = true;
            int.TryParse(textBox1.Text, out RightAnswerInProcents);

            // контекстное меню для элементов
            Hint.SetToolTip(pictureBox23, "Relay NO");
            Hint.SetToolTip(Lamp, "Lamp");
            Hint.SetToolTip(pictureBox22, "Switch");
            Hint.SetToolTip(pictureBox21, "Timer of Delay");
            Hint.SetToolTip(pictureBox20, "Relay NC");
            Hint.SetToolTip(pictureBox19, "Diod");
            Hint.SetToolTip(pictureBox18, "Capacity");
            Hint.SetToolTip(pictureBox17, "Inductor");
            Hint.SetToolTip(pictureBox25, "3 fases Motor");
            Hint.SetToolTip(pictureBox26, "3 fases Relay NO");
            calculateGeneralTime();
            SenderLoad = sender;
            ELoad = e;
            username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string user = "";
            for (int i = 0; i < username.Length; i++)
            {
                if (!username[username.Length - i - 1].Equals('\\'))
                {
                    user = username[username.Length - i - 1] + user;
                } else
                {
                    break;
                }
            }
            user = user.ToLower();

            // проверка того, что в логине содержится часть имени из бд
            var usersFromDB = users();
            bool validationTrue = false;
            foreach (var loginFromDB in usersFromDB)
            {
                if (user.Contains(loginFromDB))
                {
                    validationTrue = true;
                }
            }
            validationTrue = true;
            if (validationTrue ||
                user == "izmgunduh" ||user.Contains("gund")|| user.Contains("nduh") ||
                user == "izmersiner" || (user.Contains("zmers") && user.Contains("ner")) ||
                user == "izmyuksee"  || user.Contains("zmyuksee")
                )
            {
                pass = true;
                tabControl.Visible = true;
                button4.Visible = false;
                label12.Visible = false;
                password.Visible = false;
                tabControl.SelectedIndex = 1;
            }
            ElemensPractical.Add(new PlusTwentyFour("DigConstSign", new Point(0, 0), panelPractical));
            ElemensPractical.Add(new GroundNew("Ground", new Point(0, panelPractical.Height - 45), panelPractical));
            // создать 3-и фазы
            U = new Fase("Usource", new Point(0, 90), panelPractical);
            U.NameFase = faseName.U;
            ElemensPractical.Add(U);
            V = new Fase("Vsource", new Point(0, 135), panelPractical);
            V.NameFase = faseName.V;
            ElemensPractical.Add(V);
            W = new Fase("Wsource", new Point(0, 180), panelPractical);
            W.NameFase = faseName.W;
            ElemensPractical.Add(W);
            graph = panelPractical.CreateGraphics();

            //добавление элементов для панели программирования
            ElemensPrograming.Add(new PlusTwentyFour("DigConstSign", new Point(0, 0), panelPrograming));
            ElemensPrograming.Add(new GroundNew("Ground", new Point(0, panelPrograming.Height - 45), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 10), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 60), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 110), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 160), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 210), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 260), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 310), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 360), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 410), panelPrograming));
            ElemensPrograming.Add(new LampNewStatic("Lamp", new Point(panelPrograming.Width - 75, 460), panelPrograming));


            if (Tab1 == null)
            {
                Tab1 = tabControl.TabPages[0];
            }
            if (Tab2 == null)
            {
                Tab2 = tabControl.TabPages[1];
            }
            if (Tab3 == null)
            {
                Tab3 = tabControl.TabPages[2];
            }
            if (Tab4 == null)
            {
                Tab4 = tabControl.TabPages[3];
            }
            tabControl.TabPages.Remove(Tab4);

            changeTextsInProgram();

            NrQuestions1 = NumberOfQuestion("DC electric circuits");
            NrQuestions2 = NumberOfQuestion("AC electric circuits");
            NrQuestions3 = NumberOfQuestion("Theory of magnetism");
            NrQuestions4 = NumberOfQuestion("Digital electronics");
            NrQuestions5 = NumberOfQuestion("Electrical machines");

            textBox5.Text = NrQuestions1.ToString();
            textBox6.Text = NrQuestions2.ToString();
            textBox7.Text = NrQuestions3.ToString();
            textBox8.Text = NrQuestions4.ToString();
            textBox9.Text = NrQuestions5.ToString();

            //чтение из файла конфигурации
            string path = Directory.GetCurrentDirectory();
            FileInfo fi1 = new FileInfo(path + "\\config.ini");

            if (fi1.Exists)
            {
                using (StreamReader sr = fi1.OpenText())
                {
                    var lang = "English";
                    string[] questions = null;
                    List<string> ReadedLines = new List<string>();
                    try
                    {
                        var lines = File.ReadAllLines(path + "\\config.ini");
                        lang = lines[0];
                        questions = lines[1].Split('#');
                        var time1 = 20;
                        int.TryParse(lines[2], out time1);
                        var time2 = 10;
                        int.TryParse(lines[3], out time2);
                        var time3 = 10;
                        int.TryParse(lines[4], out time3);
                        var time4 = 10;
                        int.TryParse(lines[5], out time4);
                        var time5 = 10;
                        int.TryParse(lines[6], out time5);
                        var count1 = 12;
                        int.TryParse(lines[7], out count1);
                        var count2 = 5;
                        int.TryParse(lines[8], out count2);
                        var count3 = 5;
                        int.TryParse(lines[9], out count3);
                        var count4 = 5;
                        int.TryParse(lines[10], out count4);
                        var count5 = 5;
                        int.TryParse(lines[11], out count5);
                        var count6 = 70;
                        int.TryParse(lines[12], out count6);
                        if (lines[13] == "TRUE")
                        {
                            checkBox1.Checked = true;
                            checkBox3.Checked = false;
                        }
                        else if (lines[14] == "TRUE")
                        {
                            checkBox1.Checked = false;
                            checkBox3.Checked = true;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                            checkBox3.Checked = false;
                        }
                        textTimeDC.Text = time1.ToString();
                        textTimeAC.Text = time2.ToString();
                        textTimeMagn.Text = time3.ToString();
                        textTimeDigEl.Text = time4.ToString();
                        textTimeElMach.Text = time5.ToString();
                        textBox5.Text = count1.ToString();
                        textBox6.Text = count2.ToString();
                        textBox7.Text = count3.ToString();
                        textBox8.Text = count4.ToString();
                        textBox9.Text = count5.ToString();
                        textBox1.Text = count6.ToString();
                    }
                    catch(Exception exe)
                    {
                        MessageBox.Show(exe.Message);
                    }


                    try
                    {
                        ListQCurrent.Items.Clear();
                        ListOfPracticalQestion.Clear();
                        for (int i = 0; i < questions.Count()-1; i = i + 3)
                        {
                            QuestionTest q = new QuestionTest(int.Parse(questions[i]), questions[i + 1], int.Parse(questions[i + 2]));
                            ListOfPracticalQestion.Add(q);
                            ListQCurrent.Items.Add(q.name);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Not right format config file. Try delete config.ini file and run application again." + ex.Message);
                    }

                    linkTaskToQuestion();

                    var langs = comboBoxLang.Items;
                    var countItem = 0;
                    foreach (var l in langs)
                    {
                        var tempSelectedItem = 0;
                        var k = l.ToString();
                        if(k == lang)
                        {
                            comboBoxLang.SelectedIndex = countItem;
                            LangComboBoxOnStartPage.SelectedIndex = countItem;
                            tempSelectedItem = countItem;
                        }
                        countItem++;
                    }
                    changeTextsInProgram();
                    loadListFromDB();
                    Lang = lang;

                }

            }

            // перевести курсор на вкладку старт
            foreach(var tab in tabControl.TabPages)
            {
                if (((TabPage)tab).Name == "tabPageStart")
                {
                    tabControl.SelectedTab = (TabPage)tab;
                }
            }

            checkBox4.Checked = checkBox1.Checked;
            checkBox5.Checked = checkBox3.Checked;

        }

        private void linkTaskToQuestion ()
        {
            int numberTask = 0;
            try
            {
                // прикрепить к вопросам графичиские задания (в xml файлах)
                List<string> filesnames = Directory.GetFiles(@".\Tasks").ToList<string>();
                foreach (string filename in filesnames)
                {
                    if ((filename.Contains(@"\Task")) && (filename.Contains(@".xml")))
                    {
                        //var str = (filename[filename.Length - 5]).ToString();
                        var arrayFilename = filename.Split(new char[] { '.' });
                        var midStr = arrayFilename[arrayFilename.Length - 2];
                        midStr = midStr.Replace("\\Tasks\\Task", "");
                        if (int.TryParse(midStr, out numberTask) && (listAllPracticalQuestion.Count >= numberTask) && (numberTask != 0))
                        {
                            listAllPracticalQuestion.Select(p => p).Where(p => p.id == numberTask).ToList()[0].nameTask = filename; //@".\Tasks\Task" + numberTask.ToString() + @".xml";
                                                                                                                                    //listAllPracticalQuestion[numberTask].nameTask = @".\Tasks\Task" + numberTask.ToString() + @".xml";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot right linked visual information to the question number " + numberTask.ToString() + ". " + ex.Message);
            }
        }

        private void checkTheoretical()
        {
            if (tabControl.SelectedIndex == 3)
            {
                checkBox3.Checked = checkBox5.Checked;
                if (checkBox5.Checked == checkBox4.Checked == true)
                {
                    checkBox1.Checked = checkBox4.Checked = false;
                }
            }
            if (tabControl.SelectedIndex == 1)
            {
                checkBox5.Checked = checkBox3.Checked;
                if (checkBox1.Checked == checkBox3.Checked == true)
                {
                    checkBox1.Checked = false;
                    checkBox4.Checked = false;
                }
            }
        }

        private void checkPractical()
        {
            if (tabControl.SelectedIndex == 3)
            {
                checkBox1.Checked = checkBox4.Checked;
                if(checkBox4.Checked == checkBox5.Checked == true)
                {
                    checkBox3.Checked = checkBox5.Checked = false;
                }
            }
            if (tabControl.SelectedIndex == 1)
            {
                checkBox4.Checked = checkBox1.Checked;
                if (checkBox1.Checked == checkBox3.Checked == true)
                {
                    checkBox3.Checked = false;
                    checkBox5.Checked = false;
                }
            }
        }

        private List<string> users()
        {

            try
            {
                dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
                dbc.Open();

                DataTable table = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT User FROM users", dbc);
                adapter.Fill(table);
                DataRow[] foundRows;
                foundRows = table.Select();
                List<string> users = new List<string>();
                foreach (var t in foundRows)
                {
                    users.Add(t.ItemArray[0].ToString());
                }
                
                dbc.Close();
                return users;
            }
            catch
            {
                MessageBox.Show("Not found file with database (qbdjti.mdb) or table of users");
                return null;
            }
        }

        private int NumberOfQuestion(string nameChapter)
        {
            try
            {
                dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
                dbc.Open();

                DataTable table = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT ID FROM Questions_en WHERE chapter='" + nameChapter + "'", dbc);
                adapter.Fill(table);
                DataRow[] foundRows;
                foundRows = table.Select();

                dbc.Close();

                return foundRows.Length;
            }
            catch
            {
                MessageBox.Show("Not found file with database (qbdjti.mdb)!");
                return 0;
            }
        }

        private string getTextByNr(int i, string table)
        {
            DataTable table4 = new DataTable();
            OleDbDataAdapter adapter4 = new OleDbDataAdapter("SELECT " + table + " FROM Text_prog WHERE Nr=" + i.ToString(), dbc);
            adapter4.Fill(table4);
            DataRow[] RowsText4;
            RowsText4 = table4.Select();
            return RowsText4[0].ItemArray[0].ToString();
        }

        private void blokedChangWindows(bool unloked)
        {
            groupBox5.Enabled = unloked;
            groupBoxDBwork.Enabled = unloked;
            groupBox9.Enabled = unloked;
            button2.Enabled = unloked;
            OpenDBwork.Enabled = unloked;
        }
        private void changeTextsInProgram()
        {
            button15.Enabled = false;
            try
            {
                dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
                dbc.Open();

                DataTable table = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT Languidge FROM Languidge", dbc);
                adapter.Fill(table);
                DataRow[] foundRows;
                foundRows = table.Select();

                string selectedItem;
                if (comboBoxLang.SelectedItem == null)
                {
                    selectedItem = "";
                }
                else
                {
                    selectedItem = comboBoxLang.SelectedItem.ToString();
                }
                bool firstTime;
                if (selectedItem == "")
                {
                    firstTime = true;
                }
                else
                {
                    firstTime = false;
                }
                comboBoxLang.Items.Clear();
                LangComboBoxOnStartPage.Items.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    comboBoxLang.Items.Add(foundRows[i].ItemArray[0].ToString());
                    LangComboBoxOnStartPage.Items.Add(foundRows[i].ItemArray[0].ToString());
                }
                dbc.Close();
                if (firstTime)
                {
                    comboBoxLang.SelectedIndex = 0;
                    LangComboBoxOnStartPage.SelectedIndex = 0;
                    selectedItem = comboBoxLang.SelectedItem.ToString();
                }


                //записть текста из БД
                dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
                dbc.Open();

                DataTable table4 = new DataTable();
                OleDbDataAdapter adapter4 = new OleDbDataAdapter("SELECT text_en FROM Text_prog", dbc);
                adapter4.Fill(table4);
                DataRow[] RowsText4;
                RowsText4 = table4.Select();

                DataTable table5 = new DataTable();
                OleDbDataAdapter adapter5 = new OleDbDataAdapter("SELECT Name_column FROM Languidge WHERE Languidge='" + selectedItem + "'", dbc);
                adapter5.Fill(table5);
                DataRow[] RowsText5;
                RowsText5 = table5.Select();
                string lang = RowsText5[0].ItemArray[0].ToString();

                TextProg.Clear();
                TextProg.Add("start line");

                progressBar1.Visible = true;
                double stepProgressBar = 100.0 / (table4.Rows.Count - 1);
                double currentValueProgressBar = 0;
                for (int i = 0; i < table4.Rows.Count; i++)
                {
                    currentValueProgressBar += stepProgressBar;
                    TextProg.Add(getTextByNr((i + 1), lang));
                    progressBar1.Value = (int)currentValueProgressBar;
                }

                dbc.Close();
            }
            catch
            {
                MessageBox.Show("Not found file with database (qbdjti.mdb)!");
                Close();
            }

            // текст для всех элементов на форме
            label7.Text = TextProg[114];
            button4.Text = TextProg[14];
            label12.Text = TextProg[13];
            label3.Text = TextProg[107];
            StartTheoreticalTest.Text = TextProg[31];
            label6.Text = TextProg[67];
            groupBox1.Text = TextProg[69];
            label13.Text = TextProg[70];
            label14.Text = TextProg[71];
            label15.Text = TextProg[41];
            button5.Text = TextProg[42];
            button6.Text = TextProg[43];
            button9.Text = TextProg[44];
            button17.Text = TextProg[44];
            foreach(var tab in tabControl.TabPages)
            {
                if (((TabPage)tab).Name == "tabPageStart")
                {
                    ((TabPage)tab).Text = TextProg[14];
                }
            }
            

            fileToolStripMenuItem.Text = TextProg[108];
            printToolStripMenuItem.Text = TextProg[109];
            saveToolStripMenuItem.Text = TextProg[110];
            openToolStripMenuItem.Text = TextProg[111];
            infoToolStripMenuItem.Text = TextProg[112];
            aboutToolStripMenuItem.Text = TextProg[113];

            //заполнение вопросами список
            //записть текста из БД
            loadListFromDB();

            groupBox5.Text = TextProg[65];
            groupBox6.Text = TextProg[66];
            label6.Text = TextProg[67];
            button15.Text = TextProg[68];
            groupBox1.Text = TextProg[69];
            //label13.Text = TextProg[70];
            //label13.Text = TextProg[71];
            button5.Text = TextProg[72];
            label15.Text = TextProg[73];
            button6.Text = TextProg[74];
            groupBox4.Text = TextProg[75];
            button9.Text = TextProg[76];
            label19.Text = TextProg[55];
            label18.Text = TextProg[56];
            label20.Text = TextProg[57];
            label21.Text = TextProg[58];
            label22.Text = TextProg[59];
            Tab1.Text = TextProg[77];
            Tab3.Text = TextProg[78];
            Tab4.Text = TextProg[79];
            Tab2.Text = TextProg[65];
            label23.Text = TextProg[80];
            label24.Text = TextProg[80];
            label25.Text = TextProg[80];
            label26.Text = TextProg[80];
            label27.Text = TextProg[80];
            button10.Text = button13.Text = TextProg[14];
            button11.Text = button12.Text = TextProg[16];
            button14.Text = TextProg[81];
            groupBox2.Text = groupBox7.Text = TextProg[82];
            groupBox3.Text = groupBox8.Text = TextProg[83];
            label29.Text = TextProg[84];
            label31.Text = TextProg[80];
            label32.Text = TextProg[87];
            groupBoxDBwork.Text = TextProg[88];
            button1.Text = TextProg[89];
            button3.Text = TextProg[90];
            button7.Text = TextProg[91];
            button8.Text = TextProg[92];
            groupBox9.Text = TextProg[93];
            checkBox1.Text = TextProg[94];
            checkBox2.Text = TextProg[95];
            checkBox3.Text = TextProg[96];
            label1.Text = TextProg[97];
            label2.Text = TextProg[98];
            button2.Text = TextProg[99];
            OpenDBwork.Text = TextProg[100];
            label10.Text = TextProg[172];
            label9.Text = TextProg[173];
            label34.Text = TextProg[174];
            label35.Location = new Point(label34.Location.X + label34.Width - 4, label34.Location.Y);
            checkBox4.Text = TextProg[175];
            checkBox5.Text = TextProg[176];
            buttonSaveTask.Text = TextProg[177];
            buttonLoadTasks.Text = TextProg[178];
            button16.Text = TextProg[179];

            progressBar1.Value = 0;
            //значение по умолчанию для выбора языка
            if (!SetTextSecondTime)
            {
                comboBoxLang.SelectedIndex = NumberLang;
                LangComboBoxOnStartPage.SelectedIndex = NumberLang;
                SetTextSecondTime = true;
            }
            //значение по умолчанию для выбора вопроса
            ListQ.SelectedIndex = 0;
            //добавить один вопрос по умолчанию
            int time;
            if (!int.TryParse(TimeQ.Text, out time))
            {
                time = 0;
            }

            // проверяем, что файл конфигурации существует
            string path = Directory.GetCurrentDirectory();
            FileInfo fi1 = new FileInfo(path + "\\config.ini");

            

            if (ListQCurrent.Items.Count == 0)
            {
                QuestionTest q = new QuestionTest(ListOfPracticalQestion.Count, ListQ.Text, time);
                ListOfPracticalQestion.Add(q);
                ListQCurrent.Items.Add(ListQ.Text);
            }
            else if (!fi1.Exists)
            {
                ListOfPracticalQestion.Clear();
                QuestionTest q = new QuestionTest(ListOfPracticalQestion.Count, ListQ.Text, time);
                ListOfPracticalQestion.Add(q);
                ListQCurrent.Items.Clear();
                ListQCurrent.Items.Add(ListQ.Text);
            }
            //значение по умолчанию для времени ответа на вопрос
            //int numberInt;

            GeneralTimer.Interval = 100;
            GeneralTimer.Start();
            progressBar1.Visible = false;
            button15.Enabled = true;
        }

        private void StartTheoreticalTest_Click(object sender, EventArgs e)
        {

            Theoretical_test ThoerTest = new Theoretical_test();
            ThoerTest.ShowDialog();
        }

        private void ImplementationTheorTest()
        {
            //выводим вопросы на экран
            if (countQestion < CollectionOfQuestion.Count)
            {
                int x = 80;
                int y = 120;
                label5.Text = CollectionOfQuestion[countQestion];
                countQestion = countQestion + 1;
                int countControls = 0;
                int numberCheckBox = 1;
                while (!(CollectionOfQuestion[countQestion].Substring(0, 2) == "??"))
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.Location = new Point(x, y);
                    string name = "00" + (40 + numberCheckBox).ToString();
                    name = ((char)int.Parse(name, System.Globalization.NumberStyles.AllowHexSpecifier)).ToString();
                    checkbox.Name = name;
                    ControlsForQestion.Add(checkbox);

                    ((CheckBox)ControlsForQestion[ControlsForQestion.Count - 1]).CheckedChanged += Form1_CheckedChanged; ;
                    Label lable = new Label();
                    lable.Location = new Point(x + 100, y);
                    lable.AutoSize = true;
                    lable.Text = CollectionOfQuestion[countQestion];
                    ControlsForQestion.Add(lable);
                    y = y + 30;
                    countQestion = countQestion + 1;
                    countControls = countControls + 2;
                    numberCheckBox++;
                }
                numberCheckBox = 0;

                for (int i = 0; i<countControls; i++)
                {
                    tabTheoretical.Controls.Add(ControlsForQestion[i]);                    
                }

                // обработать правильный ответ
                rightAnsver = CollectionOfQuestion[countQestion].Substring(2, CollectionOfQuestion[countQestion].Length - 2);
                countQestion = countQestion + 1;
            }
            else
            {
                theorTestInProgress = false;
                label5.Text = "Finish";
                int summ = 0;
                foreach (bool i in CollectionOfAnswers)
                {
                    if (i == true) summ++;
                }
                MessageBox.Show(TextProg[23] + summ.ToString() + TextProg[24] + CollectionOfAnswers.Count.ToString());
            }

        }

        // формируем ответ
        private void Form1_CheckedChanged(object sender, EventArgs e)
        {
            string temp = (((CheckBox)sender).Name).ToString();
            if (((CheckBox)sender).Checked == true)
            {
                Answer += temp;
            }
            else
            {
                if (Answer.Contains(temp))
                {
                    Answer = Answer.Remove(Answer.IndexOf(temp), 1);
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tabSettings_Click(object sender, EventArgs e)
        {

        }
      
        private void DisapierPoint_Tick(object sender, EventArgs e)
        {
            
            if (calculateTickDisapier == 1)
            {
                //OutputPoint.Visible = false;
                //InputPoint.Visible = false;
                //OutputPointEl.Visible = false;
                //InputPointEl.Visible = false;
                DisapierPoint.Enabled = false;
                calculateTickDisapier = 0;             
                return;
            }
            calculateTickDisapier++;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GeneralTimer.Dispose();
            //ArrayElementOnForm.Clear();
        }


        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool qIsIncludeInList = false;
            foreach(QuestionTest question in ListOfPracticalQestion)
            {
                if (question.name == ListQ.Text)
                {
                    qIsIncludeInList = true;
                }
            }
            if (!qIsIncludeInList)
            {
                int time;
                if (!int.TryParse(TimeQ.Text, out time))
                {
                    time = 0;
                }
                QuestionTest q = new QuestionTest(ListOfPracticalQestion.Count , ListQ.Text, time);
                ListOfPracticalQestion.Add(q);
                ListQCurrent.Items.Add(ListQ.Text);
            }
            
            //ListQCurrent.DataSource = ListOfPracticalQestion;
           // ListQCurrent.ValueMember = "id";
            //ListQCurrent.DisplayMember = "name";

        }

        private void loadTextFromDB (string languidge)
        {
            Lang = languidge;
            //записть текста из БД
            dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
            dbc.Open();

            DataTable table = new DataTable();
            string tableName;
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT Name_column FROM Languidge WHERE Languidge = '" + languidge + "'", dbc);
            adapter.Fill(table);
            DataRow[] foundRows;
            foundRows = table.Select();
            tableName = foundRows[0].ItemArray[0].ToString();

            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT " + tableName + " FROM Text_prog", dbc);
            adapter1.Fill(table1);
            DataRow[] RowsText;
            RowsText = table1.Select();
            TextProg.Clear();
            TextProg.Add("start line");
            for (int i = 0; i < table1.Rows.Count; i++)
            {
                TextProg.Add(RowsText[i].ItemArray[0].ToString());
            }

            DataTable table2 = new DataTable();
            OleDbDataAdapter adapter2 = new OleDbDataAdapter("SELECT Practical FROM Languidge WHERE Languidge = '" + languidge + "'", dbc);
            adapter2.Fill(table2);
            DataRow[] foundRows2;
            foundRows2 = table2.Select();
            PracticalQuestion = foundRows2[0].ItemArray[0].ToString();

            dbc.Close();
        }

        private string LangTemp;
        private void comboBoxLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 1)
            {
                loadTextFromDB(comboBoxLang.SelectedItem.ToString());
                LangTemp = comboBoxLang.SelectedItem.ToString();
                LangComboBoxOnStartPage.SelectedIndex = comboBoxLang.SelectedIndex;
            }
            //if (LangComboBoxOnStartPage.SelectedIndex != comboBoxLang.SelectedIndex)
            //{
            //    LangComboBoxOnStartPage.SelectedIndex = comboBoxLang.SelectedIndex;
            //}   
            //LangComboBoxOnStartPage.Text = "Select language...";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int index = ListQCurrent.SelectedIndex;
            if (index != -1)
            {
                ListOfPracticalQestion.RemoveAt(index);
                ListQCurrent.Items.RemoveAt(index);
            }
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4_Click(sender, e);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //this.Visible = false;
            //this.Enabled = false;
            //Introduction introduction = new Introduction();
            //introduction.Show();
            //LangComboBoxOnStartPage.Items.Clear();
            //foreach (var item in comboBoxLang.Items)
            //{
            //    LangComboBoxOnStartPage.Items.Add(item);
            //}
            //LangComboBoxOnStartPage.SelectedIndex = comboBoxLang.SelectedIndex;
        }

        // сохранение файла конфигурации
        private void SaveConfig(FileInfo fi1)
        {
            //Create a file to write to.
            using (StreamWriter sw = fi1.CreateText())
            {
                sw.WriteLine(comboBoxLang.Text);
                var q = "";
                //int index = 1;
                //foreach (var question in ListQ.Items)
                //{
                //    q += (index.ToString() + "#" + question.ToString() + "#" + question.time.ToString()) + "#";
                //    index++;
                //}
                foreach (QuestionTest question in ListOfPracticalQestion)
                {
                    q += (question.id.ToString() + "#" + question.name + "#" + question.time.ToString()) + "#";
                }
                sw.WriteLine(q);

                sw.WriteLine(textTimeDC.Text);
                sw.WriteLine(textTimeAC.Text);
                sw.WriteLine(textTimeMagn.Text);
                sw.WriteLine(textTimeDigEl.Text);
                sw.WriteLine(textTimeElMach.Text);
                sw.WriteLine(textBox5.Text);
                sw.WriteLine(textBox6.Text);
                sw.WriteLine(textBox7.Text);
                sw.WriteLine(textBox8.Text);
                sw.WriteLine(textBox9.Text);
                sw.WriteLine(textBox1.Text);
                if (checkBox1.Checked == true)
                {
                    sw.WriteLine("TRUE");
                }
                else
                {
                    sw.WriteLine("FALSE");
                }
                if (checkBox3.Checked == true)
                {
                    sw.WriteLine("TRUE");
                }
                else
                {
                    sw.WriteLine("FALSE");
                }
            }
        }

        // кнопка начала тестирования
        private void button9_Click(object sender, EventArgs e)
        {
            buttonSaveTask.Visible = false;
            buttonLoadTasks.Visible = false;
            //запись конфигурации в файл
            string path = Directory.GetCurrentDirectory();
            FileInfo fi1 = new FileInfo(path + "\\config.ini");

            if (!fi1.Exists)
            {
                SaveConfig(fi1);
            }else
            {
                File.Delete(path + "\\config.ini");
                SaveConfig(fi1);
            }

            button10.Enabled = true;
            this.Visible = false;
            this.Enabled = false;
            Introduction introduction = new Introduction();
            introduction.Show();
            password.Text = "";
            //button4_Click(sender, e);
           // tabControl.Visible = true;
            Tab1 = tabControl.TabPages[0];
            Tab2 = tabControl.TabPages[1];
            Tab3 = tabControl.TabPages[2];
            if (tabControl.TabPages.Count == 4)
            {
                Tab4 = tabControl.TabPages[3];
            }
            if (checkBox1.Checked == false)
            {
                tabControl.TabPages.Remove(Tab1);
                tabControl.TabPages.Remove(Tab2);
            }
            else
            {
                tabControl.TabPages.Remove(Tab2);
                tabControl.TabPages.Remove(Tab3);
            }
            panelPractical.Enabled = false;
            groupBox3.Enabled = false;
            ViewLogout = false;
            if (tabControl.TabPages.Contains(Tab4))
            {
                tabControl.TabPages.Remove(Tab4);
            }
            
        }

        private void TimeQ_TextChanged(object sender, EventArgs e)
        {
            int numberInt;
            if (!int.TryParse(TimeQ.Text, out numberInt) && !(TimeQ.Text == ""))
            {
                MessageBox.Show(TextProg[33]);
                TimeQ.Text = "0";
            }
            //else if (TimeQ.Text == "")
              //  TimeQ.Text = "0";
           // else
             //   howMuchTimeToEndQuestionProgTest = numberInt * 10;
        }

        private void ListQCurrent_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox23_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new Relay("ReleyOPN", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count-1]);
     
        }

        private void Lamp_Click_1(object sender, EventArgs e)
        {
            ElemensPractical.Add(new LampNew("Lamp", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count-1]);
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new Button("Button", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count-1]);
        }

        private void pictureBox21_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new TimerOneSec("Timer1s", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count-1]);
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new RelayNC("ReleyCL", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count-1]);
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new DiodNew("Diod", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count-1]);
        }


        private bool pointsList1ContainInList2 (List<ConnectorNew> list1, List<ConnectorNew> list2)
        {
            foreach (ConnectorNew connector1 in list1)
            {
                foreach(ConnectorNew connector2 in list2)
                {
                    if (!list1.Contains(connector2))
                    {
                        if (connector1.Point1 == connector2.Point1 || connector1.Point1 == connector2.Point2 || connector1.Point2 == connector2.Point1 || connector1.Point2 == connector2.Point2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private List<ConnectorNew> addToList1 (List<ConnectorNew> list1, List<ConnectorNew> list2)
        {
            List<ConnectorNew> list3 = new List<ConnectorNew>(list1);
            foreach (ConnectorNew connector1 in list1)
            {
                foreach (ConnectorNew connector2 in list2)
                {
                    if (!list1.Contains(connector2))
                    {
                        if (connector1.Point1 == connector2.Point1 || connector1.Point1 == connector2.Point2 || connector1.Point2 == connector2.Point1 || connector1.Point2 == connector2.Point2)
                        {
                            list3.Add(connector2);
                        }
                    }
                }
            }
            return list3;
        }

        private List<ConnectorNew> createList (List<ConnectorNew> list1, List<ConnectorNew> list2)
        {
            if (pointsList1ContainInList2(list1, list2))
            {
                list1 = addToList1(list1, list2);
                return list1 = createList(list1, list2);
            }else
            {
                return addToList1(list1, list2);
            }

        }

        private void CalculateNew_Tick(object sender, EventArgs e)
        {
            calculateAllConnectorValue(ConectorPractical, panelPractical);
            calculateAllConnectorValue(ConectorPrograming, panelPrograming);
        }

        private void calculateAllConnectorValue(List<ConnectorNew> connectors, Panel panel)
        {
            //-- обсчет всех соединений--
            List<List<ConnectorNew>> listOfPoolConnectors = new List<List<ConnectorNew>>();
            List<ConnectorNew> CopyOfConectorPractical = new List<ConnectorNew>(connectors);
            // собираем все соединенные коннекторы в один лист

            while (CopyOfConectorPractical.Count > 0)
            {
                List<ConnectorNew> tempList = new List<ConnectorNew>();
                tempList.Add(CopyOfConectorPractical[0]);
                CopyOfConectorPractical.Remove(CopyOfConectorPractical[0]);

                //выдрать все соединенные коннекторы в tempList
                tempList = createList(tempList, CopyOfConectorPractical);
                listOfPoolConnectors.Add(tempList);

                // удалить tempList из CopyOfConectorPractical
                foreach (ConnectorNew connector in tempList)
                {
                    if (CopyOfConectorPractical.Contains(connector))
                    {
                        CopyOfConectorPractical.Remove(connector);
                    }
                }
            }

            //--------------------

            foreach (List<ConnectorNew> list in listOfPoolConnectors)
            {
                bool plus = false, minus = false;
                // когда коннекторы подключены к 24 вольтам
                foreach (ConnectorNew connector in list)
                {
                    if (connector.Element1 is PlusTwentyFour || connector.Element2 is PlusTwentyFour)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.Point1.Value = 24;
                            connect.Point2.Value = 24;
                            setFase(connector, faseName.no);
                            plus = true;
                            connect.CololrRand = Color.Red;
                        }
                    }
                }
                //когда коннекторы подключеннны к земле
                foreach (ConnectorNew connector in list)
                {
                    if (connector.Element1 is GroundNew || connector.Element2 is GroundNew)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.Point1.Value = 0;
                            connect.Point2.Value = 0;
                            setFase(connector, faseName.no);
                            minus = true;
                            connect.CololrRand = Color.Blue;
                        }
                    }
                }
                // когда коннектор подключен к фазе U
                foreach (ConnectorNew connector in list)
                {
                    if (connector.Element1 is Fase && (connector.Element1 as Fase).NameFase == faseName.U)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.CololrRand = Color.DarkRed;
                            setFase(connect, faseName.U);
                        }
                    }
                    else if (connector.Element2 is Fase && (connector.Element2 as Fase).NameFase == faseName.U)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.CololrRand = Color.DarkRed;
                            setFase(connect, faseName.U);
                        }
                    }
                }
                // когда коннектор подключен к фазе V
                foreach (ConnectorNew connector in list)
                {
                    if (connector.Element1 is Fase && (connector.Element1 as Fase).NameFase == faseName.V)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.CololrRand = Color.Green;
                            setFase(connect, faseName.V);
                        }
                    }
                    else if (connector.Element2 is Fase && (connector.Element2 as Fase).NameFase == faseName.V)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.CololrRand = Color.Green;
                            setFase(connect, faseName.V);
                        }
                    }
                }
                // когда коннектор подключен к фазе W
                foreach (ConnectorNew connector in list)
                {
                    if (connector.Element1 is Fase && (connector.Element1 as Fase).NameFase == faseName.W)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.CololrRand = Color.FromArgb(248,148,29);
                            setFase(connect, faseName.W);
                        }
                    }
                    else if (connector.Element2 is Fase && (connector.Element2 as Fase).NameFase == faseName.W)
                    {
                        foreach (ConnectorNew connect in list)
                        {
                            connect.CololrRand = Color.FromArgb(248, 148, 29);
                            setFase(connect, faseName.W);
                        }
                    }
                }
                //когда коннекторы ни к чему не подключены
                bool notConnected = true;
                foreach (ConnectorNew connector in list)
                {
                    if (connector.Element1 is PlusTwentyFour || connector.Element2 is PlusTwentyFour || connector.Element1 is GroundNew || connector.Element2 is GroundNew || connector.Element1 is Fase || connector.Element2 is Fase)
                    {
                        notConnected = false;
                    }
                }
                if (notConnected && panel.Name == "panelPractical")
                {
                    foreach (ConnectorNew connector in list)
                    {
                        connector.Point1.Value = -1;
                        connector.Point2.Value = -1;
                        setFase(connector, faseName.no);
                        connector.CololrRand = Color.Gray;
                    }
                }
                else if (notConnected && panel.Name == "panelPrograming")
                {
                    foreach (ConnectorNew connector in list)
                    {
                        connector.Point1.Value = -1;
                        connector.Point2.Value = -1;
                        setFase(connector, faseName.no);
                        connector.CololrRand = Color.Blue;
                    }
                }

                //if (PrevList.Equals(listOfPoolConnectors))
                //{
                //    panel.Invalidate();
                //}
                //PrevList = listOfPoolConnectors;

                //предупреждение о коротком замыкании
                if (plus && minus && KZ == false)
                {
                    KZ = true;
                    if (panel.Name == "panelPractical")
                    {
                        //MessageBox.Show("Short circuit!", "Attention!", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                        //var attention = new ShurtCircuit();
                        //attention.Show();
                        labelShortCircuit.Visible = true;
                    }
                }
                if (((!plus && minus) || (plus && !minus) || !(plus && minus)) )
                {
                    KZ = false;
                    labelShortCircuit.Visible = false;
                }
            }
            if (connectors.Count == 0 && panel.Name == "panelPractical")
            {
                KZ = false;
            }

            try
            {
                // рисование кружочка при пересечении коннекторов
                foreach (ConnectorNew connector1 in connectors)
                {
                    foreach (ConnectorNew connector2 in connectors)
                    {
                        Point point = crossConectors(connector1, connector2);
                        if (!(point).IsEmpty)
                        {
                            foreach (List<ConnectorNew> list in listOfPoolConnectors)
                            {
                                if (list.Contains(connector1) && list.Contains(connector2) && connectedTwoConnectors(connector1, connector2))
                                {
                                    Graphics graph = panel.CreateGraphics();
                                    graph.FillEllipse(new SolidBrush(connector1.CololrRand), new Rectangle(point.X - 4, point.Y - 4, 8, 8));
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                //MessageBox.Show("1");
            }
        }

        private void setFase(ConnectorNew connector, faseName F)
        {
            connector.Point1.fase = F;
            connector.Point2.fase = F;
        }

        /// <summary>
        /// определение пересечения у двух коннекторов
        /// </summary>
        /// <param name="connector1"></param>
        /// <param name="connector2"></param>
        /// <returns></returns>
        private Point crossConectors (ConnectorNew connector1, ConnectorNew connector2)
        {
            if (connector1.CurrentSetPoint != null && connector2.CurrentSetPoint != null)
            {
                for (int i = 0; i < connector1.CurrentSetPoint.Count - 1; i++)
                {
                    for (int j = 0; j < connector2.CurrentSetPoint.Count - 1; j++)
                    {
                        if (connector1 != connector2)
                        {
                            if (connector1.CurrentSetPoint[i].Y == connector1.CurrentSetPoint[i + 1].Y && connector2.CurrentSetPoint[j].X == connector2.CurrentSetPoint[j + 1].X)
                            {
                                if (connector2.CurrentSetPoint[j].X >= connector1.CurrentSetPoint[i].X && connector2.CurrentSetPoint[j].X <= connector1.CurrentSetPoint[i + 1].X)
                                {
                                    if (connector1.CurrentSetPoint[i].Y >= connector2.CurrentSetPoint[j].Y && connector1.CurrentSetPoint[i].Y <= connector2.CurrentSetPoint[j + 1].Y)
                                    {
                                        return new Point(connector2.CurrentSetPoint[j].X, connector1.CurrentSetPoint[i].Y);
                                    }
                                }
                            }
                            if (connector2.CurrentSetPoint[j].Y == connector2.CurrentSetPoint[j + 1].Y && connector1.CurrentSetPoint[i].X == connector1.CurrentSetPoint[i + 1].X)
                            {
                                if (connector1.CurrentSetPoint[i].X >= connector2.CurrentSetPoint[j].X && connector1.CurrentSetPoint[i].X <= connector2.CurrentSetPoint[j + 1].X)
                                {
                                    if (connector2.CurrentSetPoint[j].Y >= connector1.CurrentSetPoint[1].Y && connector2.CurrentSetPoint[j].Y <= connector1.CurrentSetPoint[i + 1].Y)
                                    {
                                        return new Point(connector1.CurrentSetPoint[i].X, connector2.CurrentSetPoint[j].Y);
                                    }
                                }
                            }
                            if (connector1.CurrentSetPoint[i].Y == connector1.CurrentSetPoint[i + 1].Y && connector2.CurrentSetPoint[j].Y == connector2.CurrentSetPoint[j + 1].Y && connector1.CurrentSetPoint[i].Y == connector2.CurrentSetPoint[j].Y)
                            {
                                if (Math.Min(connector1.CurrentSetPoint[i].X, connector1.CurrentSetPoint[i+1].X) != Math.Min(connector2.CurrentSetPoint[j].X, connector2.CurrentSetPoint[j + 1].X))
                                {
                                    return new Point((Math.Max(Math.Min(connector1.CurrentSetPoint[i].X, connector1.CurrentSetPoint[i + 1].X), Math.Min(connector2.CurrentSetPoint[j].X, connector2.CurrentSetPoint[j + 1].X))), connector1.CurrentSetPoint[i].Y);
                                }
                                if (Math.Max(connector1.CurrentSetPoint[i].X, connector1.CurrentSetPoint[i + 1].X) != Math.Max(connector2.CurrentSetPoint[j].X, connector2.CurrentSetPoint[j + 1].X))
                                {
                                    return new Point((Math.Min(Math.Max(connector1.CurrentSetPoint[i].X, connector1.CurrentSetPoint[i + 1].X), Math.Max(connector2.CurrentSetPoint[j].X, connector2.CurrentSetPoint[j + 1].X))), connector1.CurrentSetPoint[i].Y);
                                }
                            }
                        }
                    }
                }
            }
            return new Point();
        }

        private bool connectedTwoConnectors (ConnectorNew connector1, ConnectorNew connector2)
        {
            if (connector1.Point1 == connector2.Point1 || connector1.Point1 == connector2.Point2 || connector1.Point2 == connector2.Point1 || connector1.Point2 == connector2.Point2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void pictureBox25_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new Motor3F("Motor3F", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count - 1]);
        }

        private void pictureBox26_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new Relay3F("Reley3FOPN", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count - 1]);
        }

        private bool PracticTestInProgress;
        //новая кнопка старт
        private void button10_Click(object sender, EventArgs e)
        {

            //добавляем картинку для отчета
            Bitmap bitmap = new Bitmap((Application.OpenForms[0] as Form1).panelPractical.Size.Width, (Application.OpenForms[0] as Form1).panelPractical.Size.Height + groupBox2.Size.Height + (button11.Size.Height/5));
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.CopyFromScreen(groupBox2.PointToScreen(Point.Empty), Point.Empty, new Size(1800,1800));
            }
            bitmapPracticSolution.Add(bitmap);

            panelPractical.Enabled = true;
            groupBox3.Enabled = true;
            button10.Enabled = false;
            button11.Enabled = true;
            deletAllElementAndConnectorsOnPanel();// удалить все элементы с панели
            if (ListOfPracticalQestion.Count == 0)
            {
                return;
            }
            PracticTestInProgress = true;
            if (numberQuestion >= ListOfPracticalQestion.Count)
            {
                NextLevel();
                return;
            }
            if (ListOfPracticalQestion.Count > 0 && numberQuestion < ListOfPracticalQestion.Count)
            {
                timeForAnswerEl = ListOfPracticalQestion[numberQuestion].time * 10;
            }
            else
            {
                timeForAnswerEl = 0;
            }
            button10.Text = TextProg[48];
            CalculateSignal.Enabled = true;
            Description description = new Description();
            description.Owner = this;
            description.descript.Text = TextProg[27] + (ListOfPracticalQestion[numberQuestion].id + 1).ToString() + "\n" + ListOfPracticalQestion[numberQuestion].name;
            description.ShowDialog();

            //отобразить графическое задание к вопросу если оно есть
            //linkTaskToQuestion();
            var q = ListOfPracticalQestion[numberQuestion].name;
            var q1 = listAllPracticalQuestion.Select(p => p).Where(p => p.name == q);
            string pathToTask = q1.ToList()[0].nameTask;
            if (pathToTask != null)
            {
                displayTaskOnScreen(pathToTask);
            }
            

            // запустить таймер обратного отсчета
            timeInSec = 0;
        }

        //таймер обратного отсчета
        private int timeInSec;
        private void timerOnQuestion_Tick(object sender, EventArgs e)
        {
            try
            {
                timeInSec++;
                if (timeInSec >= ListOfPracticalQestion[numberQuestion].time)//время вышло
                {
                    label17.Text = "00:00";
                    button10.Enabled = true;
                    button11.Enabled = false;
                    timerOnQuestion.Enabled = false;
                    panelPractical.Enabled = false;
                    groupBox3.Enabled = false;
                }
                else
                {
                    int min, sec;
                    string minutes, secundes;

                    // вычисление времени для отображения
                    int time = ListOfPracticalQestion[numberQuestion].time - timeInSec;

                    min = time / 60;
                    if (min < 10)
                    {
                        minutes = "0" + min.ToString();
                    }
                    else
                    {
                        minutes = min.ToString();
                    }

                    sec = (time - min * 60);
                    if (sec < 10)
                    {
                        secundes = "0" + sec.ToString();
                    }
                    else
                    {
                        secundes = sec.ToString();
                    }

                    label17.Text = minutes + ":" + secundes;
                }
            }
            catch
            { 
                //MessageBox.Show("2"); 
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e) // кнопка READY
        {
            CheckAnswer ca = new CheckAnswer();
            ca.Owner = this;
            ca.ShowDialog();
            if (PushedNext)
            {

                ////добавляем картинку для отчета
                //Bitmap bitmap = new Bitmap(panelPractical.Size.Width, panelPractical.Size.Height);
                //using (Graphics gr = Graphics.FromImage(bitmap))
                //{
                //    gr.CopyFromScreen(panelPractical.PointToScreen(Point.Empty), Point.Empty, panelPractical.Size);
                //}
                //bitmap.Save("screenshot.bmp");
                PushedNext = false;

                (Application.OpenForms[0] as Form1).Enabled = true;
                (Application.OpenForms[0] as Form1).NextQuestion();
                (Application.OpenForms[0] as Form1).button10.Enabled = true;
                (Application.OpenForms[0] as Form1).button11.Enabled = false;
                (Application.OpenForms[0] as Form1).timerOnQuestion.Enabled = false;
                (Application.OpenForms[0] as Form1).CalculateSignal.Enabled = false;
                (Application.OpenForms[0] as Form1).panelPractical.Enabled = false;
                (Application.OpenForms[0] as Form1).groupBox3.Enabled = false;
                (Application.OpenForms[0] as Form1).label4.Text = "";
            }
        }
        private void panelPractical_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach(ElementNew element in ElemensPractical)//если кликнули два раза на таймере, изменить его время срабатывания
            {
                if (element is TimerOneSec)
                {
                    if (element.RectOfElement.Contains(e.Location))
                    {
                        SetTime setTime = new SetTime();
                        ElementForChangeTime = element;
                        setTime.ShowDialog();
                    }
                }
            }
        }

        private void TimerForElTest_Click(object sender, EventArgs e)
        {

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            calculateGeneralTime();
        }
        private void password_TextChanged(object sender, EventArgs e)
        {

        }
        private void panelPractical_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox31_Click(object sender, EventArgs e)
        {
            ElemensPrograming.Add(new Button("Button", PointPlaceOnPanelProg, panelPrograming));
            changePointOnPanelPrograming(ElemensPrograming[ElemensPrograming.Count - 1]);
        }

        private void panelPrograming_MouseUp(object sender, MouseEventArgs e)
        {
            // cостояния кнопок изменяю, если что
            foreach (ElementNew element in ElemensPrograming)
            {
                if (element is Button)
                {
                    Rectangle rect = new Rectangle(element.RectOfElement.X + 20, element.RectOfElement.Y + 5, element.RectOfElement.Width - 40, element.RectOfElement.Height - 25);
                    if (rect.Contains(e.X, e.Y) && !ButtonMovedProg)
                    {
                        (element as Button).ButtonOPN = !(element as Button).ButtonOPN;
                        (element as Button).implementationB();


                    }
                }
            }
            ClickDownOnPanelProg = false;
            ButtonMovedProg = false;
            panelPrograming.Invalidate();
        }

        private void panelPrograming_Paint(object sender, PaintEventArgs e)
        {
            //if (ClickDownOnPanel) { ButtonMoved = true; }
        }

        private void panelPrograming_MouseMove(object sender, MouseEventArgs e)
        {
            if (ClickDownOnPanelProg) { ButtonMovedProg = true; }
        }

        private void panelPrograming_MouseDown(object sender, MouseEventArgs e)
        {
            // проверяем, не кликнули-ли правой кнопкой на каком-нибудь элементе
            if (ElemensPrograming.Count > 0 && e.Button == MouseButtons.Right)
            {
                foreach (ElementNew element in ElemensPrograming)
                {
                    Point point = new Point(e.X, e.Y);
                    if (element.RectOfElement.Contains(point) && e.X > 75)
                    {
                        //создать контекстное меню для этого элемента
                        ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
                        //создать пункт Delete для этого контекстного меню
                        ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("Delete");
                        // что делать если это меню выбрали
                        tsmiDelete.Click += (object sender1, EventArgs e1) => DeleteElementNewProg(element);
                        ContextMenuForPicToElement.Items.Add(tsmiDelete);
                        //добавляем контекстное меню к элементу
                        panelPrograming.ContextMenuStrip = ContextMenuForPicToElement;
                    }
                }

                foreach (ConnectorNew connector in ConectorPrograming)
                {
                    Point point = new Point(e.X, e.Y);
                    if (checkClickonConnector(point, connector.CurrentSetPoint))
                    {
                        //создать контекстное меню для этого элемента
                        ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
                        //создать пункт Delete для этого контекстного меню
                        ToolStripMenuItem tsmiDelete = new ToolStripMenuItem("Delete");
                        // что делать если это меню выбрали
                        tsmiDelete.Click += (object sender1, EventArgs e1) => deliteConnectorProg(connector);
                        ContextMenuForPicToElement.Items.Add(tsmiDelete);
                        //добавляем контекстное меню к элементу
                        panelPrograming.ContextMenuStrip = ContextMenuForPicToElement;
                    }
                }
            }
            else
            {
                ContextMenuStrip ContextMenuForPicToElement = new ContextMenuStrip();
                panelPrograming.ContextMenuStrip = ContextMenuForPicToElement;
                panelPrograming.Invalidate();
            }

            ClickDownOnPanelProg = true;
        }

        private void pictureBox34_Click(object sender, EventArgs e)
        {
            ElemensPrograming.Add(new TimerOneSec("Timer1s", PointPlaceOnPanelProg, panelPrograming));
            changePointOnPanelPrograming(ElemensPrograming[ElemensPrograming.Count - 1]);
        }

        private void panelPrograming_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (ElementNew element in ElemensPrograming)//если кликнули два раза на таймере, изменить его время срабатывания
            {
                if (element is TimerOneSec)
                {
                    if (element.RectOfElement.Contains(e.Location))
                    {
                        SetTime setTime = new SetTime();
                        ElementForChangeTime = element;
                        setTime.ShowDialog();
                    }
                }
            }
        }

        private void pictureBox37_Click(object sender, EventArgs e)
        {
            ElemensPrograming.Add(new AndNew("And", PointPlaceOnPanelProg, panelPrograming));
            changePointOnPanelPrograming(ElemensPrograming[ElemensPrograming.Count - 1]);
        }

        private void pictureBox36_Click(object sender, EventArgs e)
        {
            ElemensPrograming.Add(new OrNew("Or", PointPlaceOnPanelProg, panelPrograming));
            changePointOnPanelPrograming(ElemensPrograming[ElemensPrograming.Count - 1]);
        }

        private void pictureBox35_Click(object sender, EventArgs e)
        {
            ElemensPrograming.Add(new NotNew("Not", PointPlaceOnPanelProg, panelPrograming));
            changePointOnPanelPrograming(ElemensPrograming[ElemensPrograming.Count - 1]);
        }

        private void pictureBox33_Click(object sender, EventArgs e)
        {
            ElemensPrograming.Add(new RSNew("RSTrig", PointPlaceOnPanelProg, panelPrograming));
            changePointOnPanelPrograming(ElemensPrograming[ElemensPrograming.Count - 1]);
        }

        private void calculateGeneralTime()
        {
            int result, GeneralTime = 0;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeDC.Text), out result);
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeAC.Text), out result);
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeMagn.Text), out result);
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeDigEl.Text), out result);
            GeneralTime += result;
            int.TryParse(((Application.OpenForms[0] as Form1).textTimeElMach.Text), out result);
            GeneralTime += result;
            string min = GeneralTime.ToString();
            if (GeneralTime < 10)
            {
                min = "0" + GeneralTime.ToString();
            }
            label30.Text = min + ":00";
        }

        private void textTimeDC_TextChanged(object sender, EventArgs e)
        {
            calculateGeneralTime();
        }
        // удлрение всех добавленных элементов и соединений с панели
        public void deleteAllFromPanel()
        {
            for (int i = ElemensPractical.Count - 1; i >= 5; i--)
            {
                DeleteElementNew(ElemensPractical[i]);
            }
            for (int i = ConectorPractical.Count - 1; i >= 0; i--)
            {
                deliteConnector(ConectorPractical[i]);
            }
            panelPractical.Invalidate();
        }

        // удаление всех, без исключения элементов с панели
        public void deleteAllFromPracticalPanel()
        {
            //ElemensPractical.Clear();
            //ConectorPractical.Clear();
            for (int i = ElemensPractical.Count - 1; i >= 0; i--)
            {
                DeleteElementNew(ElemensPractical[i]);
            }
            for (int i = ConectorPractical.Count - 1; i >= 0; i--)
            {
                deliteConnector(ConectorPractical[i]);
            }
            panelPractical.Invalidate();
        }
        // кнопка удаление всех элементов с панели
        private void button14_Click(object sender, EventArgs e)
        {
            DeleteAllElements del = new DeleteAllElements();
            del.ShowDialog();
            //deleteAllFromPanel();
        }

        // кнопка смены языка
        private void button15_Click(object sender, EventArgs e)
        {
            changeTextsInProgram();
            //Form1_Load(SenderLoad, ELoad);
            loadListFromDB();
            Lang = LangTemp;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ChangePractQuestion winForm = new ChangePractQuestion();
            winForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EditLanguage editLang = new EditLanguage();
            editLang.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EditTheorQuestions editTheor = new EditTheorQuestions();
            editTheor.ShowDialog();
        }

        
        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        // проверка, что количество вопросов было задано корректно
        private int checkRightInputValue (TextBox textBox, int count, string chapter)
        {
            int result = 0;
            if (int.TryParse(textBox.Text, out result) && result != 0)
            {
                if (result <= count)
                {
                    return result;
                }
                else
                {
                    MessageBox.Show(TextProg[115] + " " + NumberOfQuestion(chapter).ToString());
                    textBox.Text = count.ToString();
                    return count;
                }
            }
            else
            {
                MessageBox.Show(TextProg[20]);
                textBox.Text = count.ToString();
                return count;
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            NrQuestions1 = checkRightInputValue(textBox5, NrQuestions1, "DC electric circuits");
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            NrQuestions2 = checkRightInputValue(textBox6, NrQuestions2, "AC electric circuits");
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            NrQuestions3 = checkRightInputValue(textBox7, NrQuestions3, "Theory of magnetism");
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            NrQuestions4 = checkRightInputValue(textBox8, NrQuestions4, "Digital electronics");
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            NrQuestions5 = checkRightInputValue(textBox9, NrQuestions5, "Electrical machines");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bitmap = new Bitmap(tabControl.Size.Width, tabControl.Size.Height);
            tabControl.DrawToBitmap(bitmap, new Rectangle(new Point(0, 0), tabControl.Size));
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                this.printDocument1.Print();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        // без практической части
        private void checkBox1_Click(object sender, EventArgs e)
        {
            //if (checkBox3.Checked)
            //{
            //    checkBox3.Checked = false;
            //    checkBox1.Checked = true;
            //}
            //checkBox4.Checked = checkBox1.Checked;
            //checkBox5.Checked = checkBox3.Checked;
        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            PassForReport = textBox4.Text;
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            groupBoxDBwork.Enabled = false;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void LangComboBoxOnStartPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 3)
            {
                comboBoxLang.SelectedIndex = LangComboBoxOnStartPage.SelectedIndex;
                changeTextsInProgram();
                loadListFromDB();
                Lang = LangTemp;
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label35.Text =  trackBar1.Value.ToString() + "%";
            changeValueOnSetingScreen();
        }

        // выставление значений в зависимости от движка
        private void button17_Click(object sender, EventArgs e)
        {
            buttonSaveTask.Visible = false;
            buttonLoadTasks.Visible = false;
            changeValueOnSetingScreen();
            button9_Click(sender, e);
        }
        private void changeValueOnSetingScreen()
        {
            double minPerQuestion = 4.8;
            //checkBox1.Checked = true;
            //checkBox3.Checked = false;
            var QuontityQuestionsN1 = (int)((double)NrQuestions1 * ((double)trackBar1.Value / 100));
            var QuontityQuestionsN2 = (int)((double)NrQuestions2 * ((double)trackBar1.Value / 100));
            var QuontityQuestionsN3 = (int)((double)NrQuestions3 * ((double)trackBar1.Value / 100));
            var QuontityQuestionsN4 = (int)((double)NrQuestions4 * ((double)trackBar1.Value / 100));
            var QuontityQuestionsN5 = (int)((double)NrQuestions5 * ((double)trackBar1.Value / 100));
            if (QuontityQuestionsN1 == 0) { QuontityQuestionsN1 = 1; }
            if (QuontityQuestionsN2 == 0) { QuontityQuestionsN2 = 1; }
            if (QuontityQuestionsN3 == 0) { QuontityQuestionsN3 = 1; }
            if (QuontityQuestionsN4 == 0) { QuontityQuestionsN4 = 1; }
            if (QuontityQuestionsN5 == 0) { QuontityQuestionsN5 = 1; }
            textBox5.Text = QuontityQuestionsN1.ToString();
            textBox6.Text = QuontityQuestionsN2.ToString();
            textBox7.Text = QuontityQuestionsN3.ToString();
            textBox8.Text = QuontityQuestionsN4.ToString();
            textBox9.Text = QuontityQuestionsN5.ToString();
            textTimeDC.Text = ((int)(QuontityQuestionsN1 * (minPerQuestion - (minPerQuestion * ((double)trackBar1.Value / 100))))).ToString();
            textTimeAC.Text = ((int)(QuontityQuestionsN2 * (minPerQuestion - (minPerQuestion * ((double)trackBar1.Value / 100))))).ToString();
            textTimeMagn.Text = ((int)(QuontityQuestionsN3 * (minPerQuestion - (minPerQuestion * ((double)trackBar1.Value / 100))))).ToString();
            textTimeDigEl.Text = ((int)(QuontityQuestionsN4 * (minPerQuestion - (minPerQuestion * ((double)trackBar1.Value / 100))))).ToString();
            textTimeElMach.Text = ((int)(QuontityQuestionsN5 * (minPerQuestion - (minPerQuestion * ((double)trackBar1.Value / 100))))).ToString();
            textBox1.Text = (trackBar1.Value).ToString();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            OleDbEnumerator enumerator = new OleDbEnumerator();
            DataTable tablen = enumerator.GetElements();
            foreach (DataRow r in tablen.Rows)
            {
                MessageBox.Show(r["SOURCES_NAME"].ToString());
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            //checkBox1.Checked = checkBox4.Checked;
            //checkBox5.Checked = checkBox3.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            //checkBox3.Checked = checkBox5.Checked;
            //checkBox4.Checked = checkBox1.Checked;
        }

        private void buttonSaveTask_Click(object sender, EventArgs e)
        {
            SerializationTemplateData data = new SerializationTemplateData();
            data.type = new List<string>();
            data.namePicture = new List<string>();
            data.point = new List<Point>();
            data.ConnectorPointOne = new List<Point>();
            data.ConnectorPointTwo = new List<Point>();

            // элементы
            foreach (ElementNew element in ElemensPractical)
            {
                // название класса в переменную
                var type = element.GetType().ToString();
                // параметры класса в переменные для дальнейшего востановления
                var namePicture = element.NamePicture;
                var point = element.RectOfElement.Location;

                data.type.Add(type);
                data.namePicture.Add(namePicture);                
                data.point.Add(point);
            }

            // соединения
            foreach(ConnectorNew connector in ConectorPractical)
            {
                // сохранение параметров для дальнейшего востановления
                data.ConnectorPointOne.Add(connector.Point1.point);
                data.ConnectorPointTwo.Add(connector.Point2.point);
            }

            //сохранение в файл
            Directory.CreateDirectory("Tasks");
            XmlSerializer formatter = new XmlSerializer(typeof(SerializationTemplateData)); // , new Type[] { typeof(List<string>), typeof(List<string>), typeof(List<Point>) }
            // проверка наличия заданий и привязка задания к вопросу
            List<string> filesnames = Directory.GetFiles(@".\Tasks").ToList<string>();
            string pathForCreateFile = "";
            if (filesnames.Count == 0)
            {
                pathForCreateFile = @".\Tasks\Task0.xml";
            }
            else
            {
                List<int> numbersOfTasks = new List<int>();
                try
                {
                    //foreach (string filename in filesnames)
                    //{
                    //    if ((filename.Contains(@"\Task")) && (filename.Contains(@".xml")))
                    //    {
                    //        var str = (filename[filename.Length - 5]).ToString();
                    //        numbersOfTasks.Add(int.Parse(str));
                    //    }
                    //}
                    //var maxNumberTasks = numbersOfTasks.Max();
                    //pathForCreateFile = @".\Tasks\Task" + (maxNumberTasks + 1).ToString() + @".xml";

                    var QuestionForTask = new ChoiceQuestionForTask();
                    QuestionForTask.ShowDialog();
                    
                    pathForCreateFile = @".\Tasks\Task" + (returnedNumberSelectedQuestion).ToString() + @".xml";
                    using (FileStream fs = new FileStream(pathForCreateFile, FileMode.Create))
                    {
                        formatter.Serialize(fs, data);
                    }
                }
                catch
                {
                    MessageBox.Show("Error in during create file.");
                }

            }


        }

        //развернуть схему из файла
        private void buttonLoadTasks_Click(object sender, EventArgs e)
        {
            displayTaskOnScreen();
        }
        
        private void displayTaskOnScreen(string namePathTask = "")
        {
            deleteAllFromPracticalPanel();

            fliesnameOfTasks.Clear();
            fliesnameOfTasks = Directory.GetFiles(@".\Tasks").ToList<string>();

            if (namePathTask.Equals(""))
            {
                ChoiceFileForOpenTask FormForOpenFileTask = new ChoiceFileForOpenTask();
                FormForOpenFileTask.ShowDialog();
                if (pathToTaskFile == null)
                {
                    return;
                }
            }
            else
            {
                pathToTaskFile = namePathTask;
            }
            

            SerializationTemplateData data = new SerializationTemplateData();
            //выгрузка из файла
            XmlSerializer formatter = new XmlSerializer(typeof(SerializationTemplateData)); // , new Type[] { typeof(List<string>), typeof(List<string>), typeof(List<Point>) }
            using (FileStream fs = new FileStream(pathToTaskFile, FileMode.OpenOrCreate))
            {
                data = (SerializationTemplateData)formatter.Deserialize(fs);

            }

            // востанавливаем элементы по данным из xml
            for (int index = 0; index <= data.type.Count - 1; index++)
            {

                // тип класса из переменной
                Type t = Type.GetType(data.type[index]);
                // создать класс с полученным типом
                if (t != null)
                {
                    //получаем конструктор
                    Type[] types = new Type[3];
                    types[0] = typeof(string);
                    types[1] = typeof(Point);
                    types[2] = typeof(Panel);
                    System.Reflection.ConstructorInfo ci = t.GetConstructor(types);

                    //вызываем конструтор
                    object Obj = ci.Invoke(new object[] { data.namePicture[index], data.point[index], panelPractical });
                    //принудительно назначить фазы при создании новых элементов на панеле
                    if (Obj is Fase)
                    {
                        if(data.namePicture[index] == "Usource")
                        {
                            ((Fase)Obj).NameFase = faseName.U;
                        }
                        else if (data.namePicture[index] == "Vsource")
                        {
                            ((Fase)Obj).NameFase = faseName.V;
                        }
                        else if (data.namePicture[index] == "Wsource")
                        {
                            ((Fase)Obj).NameFase = faseName.W;
                        }
                    }
                    ElemensPractical.Add((ElementNew)Obj);
                }
            }

            // востанавлмваем соединеиня, по данным из xml
            for (int index = 0; index <= data.ConnectorPointOne.Count - 1; index++)
            {
                var element1 = checkIncludingPointToElement(ElemensPractical, data.ConnectorPointOne[index]);
                var element2 = checkIncludingPointToElement(ElemensPractical, data.ConnectorPointTwo[index]);
                ConnectPoint p1 = new ConnectPoint(0, data.ConnectorPointOne[index]);
                ConnectPoint p2 = new ConnectPoint(0, data.ConnectorPointTwo[index]);

                //имитация нажатия мышки при создания соединения
                if (!(element1.Equals(element2)))
                {
                    //var button = new MouseButtons();
                    //var value = new MouseEventArgs(button, 1, p1.point.X, p1.point.Y, 0);
                    //element1.Element_MouseMove(null, value);
                    //element1.Element_MouseDown(null, value);
                    //element1.Element_MouseUp(null, value);

                    //value = new MouseEventArgs(button, 1, p2.point.X, p2.point.Y, 0);
                    //element2.Element_MouseMove(null, value);
                    //element2.Element_MouseDown(null, value);
                    //element2.Element_MouseUp(null, value);

                    imitationClickButtonOnPoint(p1, element1);
                    imitationClickButtonOnPoint(p2, element2);
                }
                else // иначе создать внутренне соединение согласно тому, что заполнено
                {
                    //создание внутреннего соединения для кнопки
                    if ((element1 is Button) && (element2 is Button))
                    {
                        ((Button)element1).implementationB(true);
                    }
                }
            }
        }

        void imitationClickButtonOnPoint (ConnectPoint p, ElementNew element)
        {
            var button = new MouseButtons();
            var value = new MouseEventArgs(button, 1, p.point.X, p.point.Y, 0);
            element.Element_MouseMove(null, value);
            element.Element_MouseDown(null, value);
            element.Element_MouseUp(null, value);
        }

        // проверка, входит-ли точка в элемент
        ElementNew checkIncludingPointToElement(List<ElementNew> listOfElements, Point point)
        {
            Rectangle rect = new Rectangle(point.X - 1, point.Y - 1, 3, 3);
            foreach (ElementNew el in listOfElements)
            {
                if (el.RectOfElement.IntersectsWith(rect))
                {
                    return el;
                }
            }
            return null;
        }

        private void ListQ_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        // без теоретической части
        private void checkBox3_Click(object sender, EventArgs e)
        {
            //if (checkBox1.Checked)
            //{
            //    checkBox1.Checked = false;
            //    checkBox3.Checked = true;
            //}
        }

        private void loadListFromDB(string language = "eng")
        {

            //заполнение вопросами список
            //записть текста из БД
            List<int> TimesPracticalQuestion = new List<int>();
            dbc = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Data Source=" + Application.StartupPath + "\\qbdjti.accdb; Jet OLEDB:Database Password=ыфифлф");
            dbc.Open();
            DataTable table1 = new DataTable();
            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT " + PracticalQuestion + " FROM Practical", dbc);
            adapter1.Fill(table1);
            DataRow[] RowsText;
            RowsText = table1.Select();

            //получем колонку с продолжительностью вопросов
            DataTable tableTimes = new DataTable();
            OleDbDataAdapter adapterTimes = new OleDbDataAdapter("SELECT Time FROM Practical", dbc);
            adapterTimes.Fill(tableTimes);
            DataRow[] RowsTextTimes;
            RowsTextTimes = tableTimes.Select();

            ListQ.Items.Clear();
            listAllPracticalQuestion.Clear();
            for (int i = 0; i < table1.Rows.Count; i++)
            {
                ListQ.Items.Add(RowsText[i].ItemArray[0].ToString());
                var question = new QuestionTest(i+1, RowsText[i].ItemArray[0].ToString(), int.Parse(RowsTextTimes[i].ItemArray[0].ToString()));
                listAllPracticalQuestion.Add(question);
            }
            linkTaskToQuestion();
            dbc.Close();

        }

        private void ListQ_DropDown(object sender, EventArgs e)
        {
            loadListFromDB();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ChangeText changeText = new ChangeText();
            changeText.ShowDialog();
        }

        private void textTimeMagn_TextChanged(object sender, EventArgs e)
        {
            calculateGeneralTime();
        }

        private void OpenDBwork_Click(object sender, EventArgs e)
        {
            PassToDBwork passToDb = new PassToDBwork();
            passToDb.ShowDialog();
        }

        private void textTimeDigEl_TextChanged(object sender, EventArgs e)
        {
            calculateGeneralTime();
        }


        private void textTimeElMach_TextChanged(object sender, EventArgs e)
        {
            calculateGeneralTime();
        }

        private void pictureBox32_Click(object sender, EventArgs e)
        {
            ElemensPrograming.Add(new SRNew("SRTrig", PointPlaceOnPanelProg, panelPrograming));
            changePointOnPanelPrograming(ElemensPrograming[ElemensPrograming.Count - 1]);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out RightAnswerInProcents))
            {
                if (textBox1.Text != "")
                {
                    textBox1.Text = "0";
                    MessageBox.Show("Not numerical Value!");
                }else
                {
                    RightAnswerInProcents = 0;
                }
            }
        }

        // создание конденсатора на панели
        private void pictureBox18_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new CapacityNew("Capasity", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count - 1]);
        }
        //создание катушки на панели
        private void pictureBox17_Click(object sender, EventArgs e)
        {
            ElemensPractical.Add(new CoinNew("Coin", PointPlaceOnPanel, panelPractical));
            changePointOnPanel(ElemensPractical[ElemensPractical.Count - 1]);
        }


        // улаление всех элементов и соединений
        private void deletAllElementAndConnectorsOnPanel()
        {
            if (ElemensPractical.Count > 0)
            {
                int count = ElemensPractical.Count;
                for (int i = 5; i < count; i++)
                {
                    DeleteElementNew(ElemensPractical[ElemensPractical.Count - 1]);
                }
            }
        }

        // пароль на вход в тест.
        private void button4_Click(object sender, EventArgs e)
        {
            if (password.Text == "jfttic")
            {
                pass = true;
                tabControl.Visible = true;
                button4.Visible = false;
                label12.Visible = false;
                password.Visible = false;
                tabControl.SelectedIndex = 1;
            }
            else
            {
                pass = false;
                MessageBox.Show("Password incorrect");
            }
            
        }

        private void NextLevel()
        {
            pass = false;
            tabControl.Visible = true;
            if (Tab3 != null && checkBox3.Checked == false)
            {
                tabControl.TabPages.Add(Tab1);
                Tab1 = tabControl.TabPages[0];
                tabControl.TabPages[0].Parent = null;
                button4.Visible = false;
                label12.Visible = false;
                password.Visible = false;
            }else
            {
                MessageBox.Show(TextProg[154], "Finish!", MessageBoxButtons.OK);
                button11.Enabled = false;
                button14.Enabled = false;
                return;
            }
        }


        public void NextQuestion()
        {
            
            if (numberQuestion > ListOfPracticalQestion.Count)
            {
               // NextLevel();
            }
            else
            {
                numberQuestion++;
                //button3.Enabled = true;
            }
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 1 && TestMode == false)
            {
                e.Cancel = true;
            }
            if (e.TabPageIndex == 2 && TestMode == false)
            {
                e.Cancel = true;
            }
        }

        //кнопка Test Mode
        private void button2_Click(object sender, EventArgs e)
        {
            if (!tabControl.TabPages.Contains(Tab4))
            {
                tabControl.TabPages.Add(Tab4);
            }
            else
            {
                tabControl.TabPages.Remove(Tab4);
            }
        }

        private void test_Tick(object sender, EventArgs e)
        {

        }
    }


}
