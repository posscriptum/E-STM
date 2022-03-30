using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    class LampNewStatic: ElementNew
    {
        private System.Timers.Timer TimerDelay = new System.Timers.Timer();
        public LampNewStatic(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(0, 11));
            Conectors.Add(new Point(0, 34));

            ConectorPoints = Conectors;

            foreach (Point point in ConectorPoints)
            {
                ElementConectors.Add(new ConnectPoint(-1, point));
                PreValue.Add(-1);
            }
            CalculatePositionAreaPoint();
            PlaceForElement.MouseDown -= Element_MouseDown;
            PlaceForElement.MouseDown += PlaceForElement_MouseDown;

        }

        private void PlaceForElement_MouseDown(object sender, MouseEventArgs e)
        {
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
            if (ElementConectors[0].Value > 12 & ElementConectors[1].Value == 0 || ElementConectors[0].Value == 0 & ElementConectors[1].Value > 12)
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Lamp_light");
            }
            else
            {
                //TimerDelay.Start();
                ImageElement = (Image)Resources.ResourceManager.GetObject("Lamp");
            }
            //ElementConectors[0].Value = ElementConectors[1].Value = -1;
        }
    }
}
