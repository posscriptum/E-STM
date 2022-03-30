using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace E_STM
{
    class RSNew: ElementNew
    {
        private ConnectorNew InternalConnector;
        private ConnectorNew InverseInternalConnector;
        public RSNew(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(0, 11));
            Conectors.Add(new Point(0, 34));
            Conectors.Add(new Point(75, 11));
            Conectors.Add(new Point(75, 34));

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
            if(ElementConectors[2].Value <= 0)
            {
                if (InverseInternalConnector == null)
                {
                    InverseInternalConnector = new ConnectorNew((Application.OpenForms[0] as Form1).ElemensPrograming[0].ElementConectors[0], ElementConectors[3], PlaceForElement, (Application.OpenForms[0] as Form1).ElemensPrograming[0], this);
                    InverseInternalConnector.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Add(InverseInternalConnector);
                }
            }
            else
            {
                if (InverseInternalConnector != null)
                {
                    InverseInternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(InverseInternalConnector);
                    InverseInternalConnector = null;
                }
            }
            if (ElementConectors[1].Value > 0)
            {
                if (InternalConnector != null)
                {
                    InternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Remove(InternalConnector);
                    InternalConnector = null;
                    
                }
                return;
            }
            if (ElementConectors[0].Value > 0 && ElementConectors[1].Value <= 0)
            {
                if (InternalConnector == null)
                {
                    InternalConnector = new ConnectorNew((Application.OpenForms[0] as Form1).ElemensPrograming[0].ElementConectors[0], ElementConectors[2], PlaceForElement, (Application.OpenForms[0] as Form1).ElemensPrograming[0], this);
                    InternalConnector.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPrograming.Add(InternalConnector);
                }
            }
        }
    }
}
