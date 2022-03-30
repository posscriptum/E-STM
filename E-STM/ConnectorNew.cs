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
    public class ConnectorNew
    {
        public ConnectPoint Point1, Point2;
        public ElementNew Element1 { get; }
        public ElementNew Element2 { get; }
        private Panel panel;
        public Color CololrRand;
        private float RandomVal;
        public List<Point> CurrentSetPoint;
        private bool ConnectorDelited;
        public bool NotDrawNothing;
        private int RandVal, RandShiftPoint;

        public ConnectorNew(ConnectPoint Point1, ConnectPoint Point2, Panel panel, ElementNew element1, ElementNew element2)
        {
            this.Point1 = Point1;
            this.Point2 = Point2;
            this.panel = panel;
            Element1 = element1;
            Element2 = element2;
            panel.Paint += Panel_Paint;

            Random random = new Random();
            int t = random.Next(20, 80);
            RandomVal = (float)t / 100; //случайное число от 0.2 до 0.8
            RandVal = (int)(10 * RandomVal); //случайное число от 2 до 8
            RandShiftPoint = (int)random.Next(0, 10);
            int r, g, b;
            r = random.Next(50, 255);
            g = random.Next(50, 255);
            b = random.Next(50, 255);
            //CololrRand = Color.FromArgb(r, g, b); // случайный цвет для линии
            CololrRand = Color.Gray;
            (Application.OpenForms[0] as Form1).timerCalculateCircuit.Tick += implementation;

        }

        private void implementation(object sender, EventArgs e)
        {
            //if (!ConnectorDelited)
            //{

            //    if (Point1.Value > 0 || Point2.Value > 0)
            //    {
            //        Point2.Value = Point1.Value = Math.Max(Point1.Value, Point2.Value);
            //    }
            //    else if (Point1.Value == 0 && Point2.Value > 0 || Point2.Value == 0 && Point1.Value > 0)
            //    {
            //        Point1.Value = 0;
            //        Point2.Value = 0;
            //    }else if (Point1.Value == -1)
            //    {
            //        Point1.Value = Point2.Value;
            //    }else if (Point2.Value == -1)
            //    {
            //        Point2.Value = Point1.Value;
            //    }
            //    // Point2.Value = Point1.Value = Math.Max(Point1.Value, Point2.Value);
            //}
        }

        // Удаление коннектора
        public void deliteConnector()
        {
            Point2.Value = Point1.Value = -1;
            Point2.fase = Point1.fase = faseName.no;
            panel.Paint -= Panel_Paint;
            Graphics graph = panel.CreateGraphics();
            Point[] tempPoint = new Point[2];
            tempPoint[0] = new Point(0, 0);
            tempPoint[1] = new Point(0, 0);
            graph.DrawLines(new Pen(CololrRand, 2), tempPoint);
            panel.Invalidate();
            ConnectorDelited = true;
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            if (NotDrawNothing) { return; }
            Graphics graph = panel.CreateGraphics();
            CurrentSetPoint = pointsForDrawingConnector(Point1.point, Point2.point);
            graph.DrawLines(new Pen(CololrRand, 2), CurrentSetPoint.ToArray());

        }

        //постороение соединения
        private List<Point> pointsForDrawingConnector (Point startPoint, Point endPoint)
        {
            // делаю так, чтобы первый элемент всегда был слева
            ElementNew el1, el2;
            Point p1, end;

            if (Element1.RectOfElement.X < Element2.RectOfElement.X)
            {
                el1 = Element1;
                el2 = Element2;
                p1 = startPoint;
                end = endPoint;
            }
            else
            {
                el1 = Element2;
                el2 = Element1;
                p1 = endPoint;
                end = startPoint;
            }

            // создаю базовое соединение
            List<Point> points = new List<Point>();
            points.Add(p1);

            //----------New realisation-------------
            if (!lineCrossElement(p1, end, el1) && !lineCrossElement(p1, end, el2))
            {
                points.Add(new Point((p1.X + ((end.X - p1.X) / 2) - 4 + (int)(((double)RandVal / 10) * ((end.X - p1.X) / 2))), p1.Y));
                points.Add(new Point((p1.X + ((end.X - p1.X) / 2) - 4 + (int)(((double)RandVal / 10) * ((end.X - p1.X) / 2))), end.Y));
            }
            if (lineCrossElement(p1, end, el1) && !lineCrossElement(p1, end, el2))
            {
                points.Add(new Point(points[points.Count - 1].X - RandShiftPoint, points[points.Count - 1].Y));
                points.Add(new Point(points[points.Count - 1].X, end.Y));
                if (lineCrossElement(points[points.Count - 1], end, el1))
                {
                    points.Remove(points[points.Count - 1]);
                    if (p1.Y < el1.RectOfElement.Y + el1.RectOfElement.Height / 2)
                    {
                        points.Add(new Point(points[points.Count - 1].X, el1.RectOfElement.Y - RandShiftPoint));
                    }
                    else
                    {
                        points.Add(new Point(points[points.Count - 1].X, el1.RectOfElement.Y + el1.RectOfElement.Height + RandShiftPoint));
                    }
                    points.Add(new Point(end.X, points[points.Count - 1].Y));
                }
            }
            if (!lineCrossElement(p1, end, el1) && lineCrossElement(p1, end, el2))
            {
                points.Add(new Point(end.X + RandShiftPoint, points[points.Count - 1].Y));
                points.Add(new Point(points[points.Count - 1].X, end.Y));
                if (lineCrossElement(points[0], points[1], el2))
                {
                    if (end.Y < el2.RectOfElement.Y + el2.RectOfElement.Height / 2)
                    {
                        points.Insert(1, new Point(points[0].X, el2.RectOfElement.Y - RandShiftPoint));
                    }
                    else
                    {
                        points.Insert(1, new Point(points[0].X, el2.RectOfElement.Y + el2.RectOfElement.Height + RandShiftPoint));
                    }
                    points.Remove(points[2]);
                    points.Insert(2, new Point(points[2].X, points[1].Y));
                }
            }
            if (lineCrossElement(p1, end, el1) && lineCrossElement(p1, end, el2))
            {
                points.Add(new Point(points[points.Count - 1].X - RandShiftPoint, points[points.Count - 1].Y));
                if (p1.Y < el1.RectOfElement.Y + el1.RectOfElement.Height / 2)
                {
                    points.Add(new Point(points[points.Count - 1].X, Math.Min(el1.RectOfElement.Y - RandShiftPoint, el2.RectOfElement.Y - RandShiftPoint)));
                }
                else
                {
                    points.Add(new Point(points[points.Count - 1].X, Math.Max(el1.RectOfElement.Y + el1.RectOfElement.Height + RandShiftPoint, el2.RectOfElement.Y + el2.RectOfElement.Height + RandShiftPoint)));
                }
                points.Add(new Point(end.X + RandShiftPoint, points[points.Count - 1].Y));
                points.Add(new Point(end.X + RandShiftPoint, end.Y));
            }
            if ((p1.X == el1.RectOfElement.X + el1.RectOfElement.Width && end.X == el2.RectOfElement.X) && (el2.RectOfElement.X > el1.RectOfElement.X - RandShiftPoint && el2.RectOfElement.X < el1.RectOfElement.X + el1.RectOfElement.Width + RandShiftPoint))
            {
                points.Clear();
                points.Add(p1);
                points.Add(new Point(points[points.Count - 1].X + RandShiftPoint, points[points.Count - 1].Y));
                if (el1.RectOfElement.Y + el1.RectOfElement.Height + 2 < el2.RectOfElement.Y)
                {
                    points.Add(new Point(points[points.Count - 1].X, el1.RectOfElement.Y + (el2.RectOfElement.Y - el1.RectOfElement.Y + el1.RectOfElement.Height) / 2));
                }
                else
                {
                    points.Add(new Point(points[points.Count - 1].X, el2.RectOfElement.Y + (el1.RectOfElement.Y - el2.RectOfElement.Y + el2.RectOfElement.Height) / 2));
                }
                points.Add(new Point(el2.RectOfElement.X - RandShiftPoint, points[points.Count - 1].Y));
                points.Add(new Point(points[points.Count - 1].X, end.Y));
            }
            //------------------------

            points.Add(end);

            //проверяем, не пересекает-ли какая-нибудь линия квадрат элемента и если пересекает, то отодвигаем ее.
            foreach (ElementNew element in (Application.OpenForms[0] as Form1).ElemensPractical)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (points[i].X == points[i + 1].X && lineCrossElement(points[i], points[i + 1], element) && element != Element1 && element != Element2)
                    {
                        if (i + 2 <= points.Count - 1 && points[i + 2].X > element.RectOfElement.X && points[i + 2].X < element.RectOfElement.X + element.RectOfElement.Width)
                        {
                            points[i] = new Point(element.RectOfElement.X - 5 - RandVal, points[i].Y);
                            points[i + 1] = new Point(element.RectOfElement.X - 5 - RandVal, points[i + 1].Y);
                        }
                        else
                        {
                            points[i] = new Point(element.RectOfElement.X + element.RectOfElement.Width + 5 + RandVal, points[i].Y);
                            points[i + 1] = new Point(element.RectOfElement.X + element.RectOfElement.Width + 5 + RandVal, points[i + 1].Y);
                        }
                    }
                    if (points[i].Y == points[i+1].Y && lineCrossElement(points[i], points[i + 1], element) && element != Element1 && element != Element2)
                    {
                        if (points[i].Y < element.RectOfElement.Y + element.RectOfElement.Height / 2)
                        {
                            points[i] = new Point(points[i].X, element.RectOfElement.Y - 5 - RandVal);
                            points[i + 1] = new Point(points[i + 1].X, element.RectOfElement.Y - 5 - RandVal);
                        }else
                        {
                            points[i] = new Point(points[i].X, element.RectOfElement.Y + element.RectOfElement.Height + 5 + RandVal);
                            points[i + 1] = new Point(points[i + 1].X, element.RectOfElement.Y + element.RectOfElement.Height + 5 + RandVal);
                        }
                        if (i+1 == points.Count-1)
                        {
                            points[i + 1] = new Point(points[i + 1].X - 5 - RandVal, points[i + 1].Y);
                            points.Add(new Point(points[i + 1].X, end.Y));
                            points.Add(end);
                        }
                        if (i == 0)
                        {
                            points.Insert(0, p1);
                            points[0] = new Point(points[0].X + RandVal + 5, points[0].Y);
                            points[1] = new Point(points[1].X + RandVal + 5, points[1].Y);
                            points.Insert(0, p1);
                            break;
                        }

                    }
                }

                if (points[points.Count - 2].X > Element1.RectOfElement.X && points[points.Count - 2].X < Element1.RectOfElement.X + Element1.RectOfElement.Width || points[points.Count - 2].X > Element2.RectOfElement.X && points[points.Count - 2].X < Element2.RectOfElement.X + Element2.RectOfElement.Width)
                {
                    points[points.Count - 3] = new Point(end.X, points[points.Count - 3].Y);
                    points[points.Count - 2] = new Point(end.X, points[points.Count - 2].Y);
                }
                if (element.RectOfElement.Contains(points[1]) || element.RectOfElement.Contains(points[1]))
                {
                    points[1] = new Point(points[1].X - RandVal - 5, points[1].Y);
                    points[2] = new Point(points[2].X - RandVal - 5, points[2].Y);
                }
            }
            //-------------------------
            return points;


            //---------------------------

            //---------------------------
        }

        /// <summary>
        /// проверка, не содержит-ли элемени прямую, заданную двумя точками
        /// </summary>
        /// <param name="p1">превая точка</param>
        /// <param name="p2">вторая точка</param>
        /// <param name="element">элемент, положение которого заданно прямоугольником</param>
        /// <returns></returns>
        private bool lineCrossElement(Point p1, Point p2, ElementNew element)
        {
            Point t;
            if (p1.X > p2.X)
            {
                t = p1;
                p1 = p2;
                p2 = t;
            }
            if (p2.X != p1.X)
            {
                double a = ((double)(p2.Y - p1.Y) / (double)(p2.X - p1.X));
                double b = (double)(p1.X * p2.Y - p2.X * p1.Y) / (double)(p2.X - p1.X);
                for (int x = p1.X + 1; x < p2.X; x++)
                {
                    int y = (int)(a * x - b);
                    if (element.RectOfElement.Contains(new Point(x, y)))
                    {
                        return true;
                    }
                }
            }
            if (p1.X == p2.X && p1.X >= element.RectOfElement.X && p1.X <= element.RectOfElement.X + element.RectOfElement.Width)
            {
                if ((Math.Min(p1.Y, p2.Y) < element.RectOfElement.Y && Math.Max(p1.Y, p2.Y) > element.RectOfElement.Y + element.RectOfElement.Height) || (element.RectOfElement.Contains(p1) || (element.RectOfElement.Contains(p2))))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
