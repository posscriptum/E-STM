using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    public enum faseName
    {
        U,
        V,
        W,
        no,
    }
    class Fase: ElementNew
    {
        public faseName NameFase;
        public Fase(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(75, 22));

            ConectorPoints = Conectors;

            foreach (Point point in ConectorPoints)
            {
                ElementConectors.Add(new ConnectPoint(24, point));
            }
            CalculatePositionAreaPoint();
            PlaceForElement.MouseDown -= Element_MouseDown;
            PlaceForElement.MouseDown += PlusTventyFour_MouseDown;
            //(Application.OpenForms[0] as Form1).timerCalculateCircuit.Tick += implementationFase;

            NameFase = faseName.no;
        }

        private void PlusTventyFour_MouseDown(object sender, MouseEventArgs e)
        {
            if (RectOfElement.Contains(new Point(e.X, e.Y)))
            {
                CoordinatePushedMouse = new Point(e.X, e.Y);
                CoordinateElementWhenPushedMouse = new Point(RectOfElement.X, RectOfElement.Y);
            }
            if (mouseInArea)
            {
                (Application.OpenForms[0] as Form1).createConnector(ConectorSelected, this);
                PlaceForElement.Invalidate();
            }
        }

        public void implementationFase(int value)
        {
            ElementConectors[0].Value = value;
        }
    }
}
