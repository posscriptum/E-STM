using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    public class ElementNew
    {
        protected Size SizeElement = new Size(75, 45);
        private Size SizeConector = new Size(10, 10);
        public Rectangle RectOfElement;
        protected Panel PlaceForElement;
        protected Image ImageElement;
        private Graphics graphElement;
        protected bool MouseButtonPushed;
        protected Point CoordinatePushedMouse, CoordinateElementWhenPushedMouse;
        public List<Point> ConectorPoints = new List<Point>();
        private List<Rectangle> ConnectorAreaPoints = new List<Rectangle>();
        private bool trig;
        public List<ConnectPoint> ElementConectors = new List<ConnectPoint>();
        protected bool mouseInArea;
        protected ConnectPoint ConectorSelected;
        protected List<int> PreValue = new List<int>();
        protected Rectangle mouseIntoPointOfConnectionArea;
        protected bool clickedOnPointOfConnector;
        public string NamePicture;
        public ElementNew (string Name, Point Location, List<Point> Conectors, Panel PlaceForElement)
        {
            NamePicture = Name;
            RectOfElement = new Rectangle(Location, SizeElement);
            this.PlaceForElement = PlaceForElement;
            ImageElement = (Image)Resources.ResourceManager.GetObject(Name);
            PlaceForElement.Paint += drawElement;
            graphElement = PlaceForElement.CreateGraphics();
            PlaceForElement.Invalidate(RectOfElement);
            PlaceForElement.MouseDown += Element_MouseDown;
            PlaceForElement.MouseUp += Element_MouseUp;
            PlaceForElement.MouseMove += Element_MouseMove;
            ConectorPoints = Conectors;

            foreach(Point point in ConectorPoints)
            {
                ElementConectors.Add(new ConnectPoint(0, point));
            }
            CalculatePositionAreaPoint();
        }
        public ElementNew (string Name, Point Location, Panel PlaceForElement)
        {
            NamePicture = Name;
            RectOfElement = new Rectangle(Location, SizeElement);
            this.PlaceForElement = PlaceForElement;
            ImageElement = (Image)Resources.ResourceManager.GetObject(Name);
            PlaceForElement.Paint += drawElement;
            graphElement = PlaceForElement.CreateGraphics();
            PlaceForElement.Invalidate(RectOfElement);
            PlaceForElement.MouseDown += Element_MouseDown;
            PlaceForElement.MouseUp += Element_MouseUp;
            PlaceForElement.MouseMove += Element_MouseMove;
            (Application.OpenForms[0] as Form1).timerCalculateCircuit.Tick += runImplementation;
        }

        private void runImplementation(object sender, EventArgs e)
        {
            updatePanel();
            implementation();
        }

        /// <summary>
        /// вычисление положений точек соединения
        /// </summary>
        protected void CalculatePositionAreaPoint()
        {
            if (ConectorPoints.Count > 0)
            {
                ConnectorAreaPoints.Clear();

                for (int i = 0;  i < ConectorPoints.Count; i++)
                {
                    Point connector = new Point(RectOfElement.X + ConectorPoints[i].X - 5, RectOfElement.Y + ConectorPoints[i].Y - 5);
                    ConnectorAreaPoints.Add(new Rectangle(connector, SizeConector));
                    ElementConectors[i].point.X = RectOfElement.X + ConectorPoints[i].X;
                    ElementConectors[i].point.Y = RectOfElement.Y + ConectorPoints[i].Y;
                }
            }
        }

        // перемещение элемента
        public void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseButtonPushed)
            {
                bool crossElement = false;
                foreach (ElementNew element in (Application.OpenForms[0] as Form1).ElemensPractical)
                {
                    if (RectOfElement != element.RectOfElement && (new Rectangle(new Point(CoordinateElementWhenPushedMouse.X + (e.X - CoordinatePushedMouse.X), CoordinateElementWhenPushedMouse.Y + (e.Y - CoordinatePushedMouse.Y)), SizeElement)).IntersectsWith(element.RectOfElement))
                    {
                        crossElement = true;
                    }
                }
                foreach (ElementNew element in (Application.OpenForms[0] as Form1).ElemensPrograming)
                {
                    if (RectOfElement != element.RectOfElement && (new Rectangle(new Point(CoordinateElementWhenPushedMouse.X + (e.X - CoordinatePushedMouse.X), CoordinateElementWhenPushedMouse.Y + (e.Y - CoordinatePushedMouse.Y)), SizeElement)).IntersectsWith(element.RectOfElement))
                    {
                        crossElement = true;
                    }
                }
                if (!crossElement)
                {
                    RectOfElement = new Rectangle(new Point(CoordinateElementWhenPushedMouse.X + (e.X - CoordinatePushedMouse.X), CoordinateElementWhenPushedMouse.Y + (e.Y - CoordinatePushedMouse.Y)), SizeElement);
                    PlaceForElement.Invalidate();
                    CalculatePositionAreaPoint();
                }
            }

            // если мышь находится над областью коннектора, подсветить.
            if (ConnectorAreaPoints.Count > 0)
            {
                Point CurrentPosMouse = new Point(e.X, e.Y);
                mouseInArea = false;
                for (int i =0; i < ConnectorAreaPoints.Count; i++)
                {                   
                    if (ConnectorAreaPoints[i].Contains(CurrentPosMouse))
                    {
                        Graphics graphConector = PlaceForElement.CreateGraphics();
                        graphConector.DrawRectangle(new Pen(Color.Red, 2), ConnectorAreaPoints[i]);
                        mouseInArea = true;
                        trig = false;
                        ConectorSelected = ElementConectors[i];
                        mouseIntoPointOfConnectionArea = ConnectorAreaPoints[i];
                    }else
                    {
                        mouseIntoPointOfConnectionArea = new Rectangle();
                    }
                }
                if (!mouseInArea && !trig)
                {
                    PlaceForElement.Invalidate();
                    trig = true;
                }
            }
        }

        // отпустили кнопку мышки
        public void Element_MouseUp(object sender, MouseEventArgs e)
        {
            MouseButtonPushed = false;

            // проверка того, что нажали мышку на точке соединения
            clickedOnPointOfConnector = !clickedOnPointOfConnector;
            if (mouseIntoPointOfConnectionArea.Height != 0 && clickedOnPointOfConnector == true)
            {
                // !!!!!!! наприсовать красый квадрат не получилось.
                Graphics graphConector = PlaceForElement.CreateGraphics();
                graphConector.DrawRectangle(new Pen(Color.Red, 2), mouseIntoPointOfConnectionArea);
            }           
        }


        // нажали кнопку мышки
        public void Element_MouseDown(object sender, MouseEventArgs e)
        {
            if (RectOfElement.Contains(new Point(e.X, e.Y)))
            {
                CoordinatePushedMouse = new Point(e.X, e.Y);
                CoordinateElementWhenPushedMouse = new Point(RectOfElement.X, RectOfElement.Y);
                MouseButtonPushed = true;
            }
            if (mouseInArea)
            {
                if (PlaceForElement.Name == "panelPractical")
                {
                    (Application.OpenForms[0] as Form1).createConnector(ConectorSelected, this);
                }else if (PlaceForElement.Name == "panelPrograming")
                {
                    (Application.OpenForms[0] as Form1).createConnectorProg(ConectorSelected, this);
                }
                PlaceForElement.Invalidate();
            }
        }

        public void deliteElement()
        {
            PlaceForElement.Paint -= drawElement;
            PlaceForElement.MouseDown -= Element_MouseDown;
            PlaceForElement.MouseUp -= Element_MouseUp;
            PlaceForElement.MouseMove -= Element_MouseMove;
            graphElement.Dispose();
        }
        // прорисовка элемента привязана к прорисовке панели
        private void drawElement(object sender, PaintEventArgs e)
        {
            if (ImageElement != null)
            {
                graphElement.DrawImage(ImageElement, RectOfElement);
            }
        }

        // реализацию переопределить
        public virtual void implementation() { }
        private void updatePanel()
        {
            //---------------------------
            // принудительное обновление отображения панели, когда обновились значения.
            for (int i = 0; i < PreValue.Count; i++)
            {
                if (PreValue[i] != ElementConectors[i].Value)
                {
                    PlaceForElement.Invalidate();
                }
                PreValue[i] = ElementConectors[i].Value;
            }
        }

        /// <summary>
        /// подключена ли точка к какому-нибудь соединению
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected bool pointContainInConnectors(ConnectPoint point)
        {
            foreach (ConnectorNew connector in (Application.OpenForms[0] as Form1).ConectorPractical)
            {
                if (connector.Point1 == point || connector.Point2 == point)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// возвращает то соединения, что содержат заданную точку
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected List<ConnectorNew> connectorThatContainPoint(ConnectPoint point)
        {
            List<ConnectorNew> temp = new List<ConnectorNew>();
            foreach (ConnectorNew connector in (Application.OpenForms[0] as Form1).ConectorPractical)
            {
                if (connector.Point1 == point || connector.Point2 == point)
                {
                    temp.Add(connector);
                }
            }
            return temp;
        }
    }
}
