using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace E_STM
{
    class OrNew: ElementNew
    {
        private ConnectorNew InternalConnector;
        //private ConnectorNew ConnectorIn2ToOut;
        //private ConnectorNew ConnectorZeroToOut;
        public OrNew(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
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
            if (ElementConectors[0].Value > 0)
            {
                if (ElementConectors[1].Value <= 0 && InternalConnector != null && (InternalConnector.Point1 == ElementConectors[1] || InternalConnector.Point2 == ElementConectors[1]))
                {
                    InternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(InternalConnector);
                    InternalConnector = null;
                }
                if (InternalConnector == null)
                {
                    InternalConnector = new ConnectorNew(ElementConectors[0], ElementConectors[2], PlaceForElement, this, this);
                    InternalConnector.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Add(InternalConnector);
                }
            }else if (ElementConectors[1].Value > 0)
            {
                if (ElementConectors[0].Value <= 0 && InternalConnector != null && (InternalConnector.Point1 == ElementConectors[0] || InternalConnector.Point2 == ElementConectors[0]))
                {
                    InternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(InternalConnector);
                    InternalConnector = null;
                }
                if (InternalConnector == null)
                {
                    InternalConnector = new ConnectorNew(ElementConectors[1], ElementConectors[2], PlaceForElement, this, this);
                    InternalConnector.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Add(InternalConnector);
                }
            }
            else
            {
                if (InternalConnector != null)
                {
                    InternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(InternalConnector);
                    InternalConnector = null;
                }
            }
        }

    }
}
