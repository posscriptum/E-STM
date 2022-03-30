using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    class DiodNew: ElementNew
    {
        private ConnectorNew InternalConnector;
        public DiodNew(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(0, 22));
            Conectors.Add(new Point(75, 22));

            ConectorPoints = Conectors;

            foreach (Point point in ConectorPoints)
            {
                ElementConectors.Add(new ConnectPoint(-1, point));
                PreValue.Add(-1);
            }
            CalculatePositionAreaPoint();

        }


        public override void implementation()
        {

            if (ElementConectors[0].Value > 0)
            {
                if (InternalConnector == null)
                {
                    InternalConnector = new ConnectorNew(ElementConectors[0], ElementConectors[1], PlaceForElement, this, this);
                    InternalConnector.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPractical.Add(InternalConnector);
                    PlaceForElement.Invalidate();
                }
                //ImageElement = (Image)Resources.ResourceManager.GetObject("Button_closed");
            }
            else
            {
                if (InternalConnector != null)
                {
                    InternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPractical.Remove(InternalConnector);
                    PlaceForElement.Invalidate();
                }
                InternalConnector = null;
                //ImageElement = (Image)Resources.ResourceManager.GetObject("Button");
            }

        }
    }
}
