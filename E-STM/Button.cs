using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    class Button: ElementNew
    {
        public bool ButtonOPN;
        private ConnectorNew InternalConnector;
        public Button(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
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
            //if (ButtonOPN)
            //{
            //    ElementConectors[0].Value = ElementConectors[1].Value = -1;
            //}
        }

        public void implementationB(bool createConnection = false)
        {
            if(createConnection == true)
            {
                ButtonOPN = false;
            }
            if (!ButtonOPN)
            {
                if (InternalConnector == null)
                {
                    InternalConnector = new ConnectorNew(ElementConectors[0], ElementConectors[1], PlaceForElement, this, this);
                    InternalConnector.NotDrawNothing = true;
                    if (PlaceForElement.Name == "panelPractical")
                    {
                        (Application.OpenForms[0] as Form1).ConectorPractical.Add(InternalConnector);
                    }
                    else if (PlaceForElement.Name == "panelPrograming")
                    {
                        (Application.OpenForms[0] as Form1).ConectorPrograming.Add(InternalConnector);
                    }
                    
                    PlaceForElement.Invalidate();
                }
                ImageElement = (Image)Resources.ResourceManager.GetObject("Button_closed");
            }
            else
            {
                if (InternalConnector != null)
                {
                    InternalConnector.deliteConnector();
                    if (PlaceForElement.Name == "panelPractical")
                    {
                        (Application.OpenForms[0] as Form1).ConectorPractical.Remove(InternalConnector);
                    }
                    else if (PlaceForElement.Name == "panelPrograming")
                    {
                        (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(InternalConnector);
                    }
                    
                    PlaceForElement.Invalidate();
                }
                InternalConnector = null;
                ImageElement = (Image)Resources.ResourceManager.GetObject("Button");
            }

        }
    }
}
