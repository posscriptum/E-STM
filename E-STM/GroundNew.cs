using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_STM.Properties;
using System.Windows.Forms;

namespace E_STM
{
    class GroundNew: ElementNew
    {
        public GroundNew(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(75, 23));

            ConectorPoints = Conectors;

            foreach (Point point in ConectorPoints)
            {
                ElementConectors.Add(new ConnectPoint(0, point));
            }
            CalculatePositionAreaPoint();
            PlaceForElement.MouseDown -= Element_MouseDown;
            PlaceForElement.MouseDown += PlusTventyFour_MouseDown;
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
                if (PlaceForElement.Name == "panelPractical")
                {
                    (Application.OpenForms[0] as Form1).createConnector(ConectorSelected, this);
                }
                else if (PlaceForElement.Name == "panelPrograming")
                {
                    (Application.OpenForms[0] as Form1).createConnectorProg(ConectorSelected, this);
                }
                PlaceForElement.Invalidate();
            }
        }

        public override void implementation()
        {
            ElementConectors[0].Value = 0;
        }
    }
}
