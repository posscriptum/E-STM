using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{

    class AndNew: ElementNew
    {
        private ConnectorNew InternalConnector;
        //private ConnectorNew ConnectorIn2ToOut;
        //private ConnectorNew ConnectorZeroToOut;
        public AndNew(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(0, 11));
            Conectors.Add(new Point(0, 34));
            Conectors.Add(new Point(75, 23));

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
            if (ElementConectors[0].Value > 0 && ElementConectors[1].Value > 0)
            {
                if (InternalConnector == null)
                {
                    InternalConnector = new ConnectorNew(ElementConectors[0], ElementConectors[2], PlaceForElement, this, this);
                    InternalConnector.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Add(InternalConnector);
                }
                //if (ConnectorIn2ToOut == null)
                //{
                //    ConnectorIn2ToOut = new ConnectorNew(ElementConectors[1], ElementConectors[2], PlaceForElement, this, this);
                //    ConnectorIn2ToOut.NotDrawNothing = true;
                //    (Application.OpenForms[0] as Form1).ConectorPrograming.Add(ConnectorIn2ToOut);
                //}
            }
            else
            {
                if (InternalConnector != null)
                {
                    InternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(InternalConnector);
                    InternalConnector = null;
                }
                //if (ConnectorIn2ToOut != null)
                //{
                //    ConnectorIn2ToOut.deliteConnector();
                //    (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(ConnectorIn2ToOut);
                //    ConnectorIn2ToOut = null;
                //}
            }
        }

    }
}
