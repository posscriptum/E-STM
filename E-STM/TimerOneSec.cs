using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using E_STM.Properties;

namespace E_STM
{
    class TimerOneSec: ElementNew
    {
        private bool TimeIsFinished;
        public System.Timers.Timer Time = new System.Timers.Timer();
        private ConnectorNew InternalConnector;
        public TimerOneSec(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            this.RectOfElement = new Rectangle(this.RectOfElement.X, this.RectOfElement.Y, 110, 65);
            SizeElement = new Size(110, 65);
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(0, 32));
            Conectors.Add(new Point(110, 32));

            ConectorPoints = Conectors;

            foreach (Point point in ConectorPoints)
            {
                ElementConectors.Add(new ConnectPoint(-1, point));
                PreValue.Add(-1);
            }
            CalculatePositionAreaPoint();
            Time.Enabled = false;
            Time.Interval = 1000;
            Time.AutoReset = false;
            Time.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeIsFinished = true;
        }

        public override void implementation()
        {

            if (ElementConectors[0].Value == 24)
            {
                Time.Start();               
            }
            if (TimeIsFinished && ElementConectors[0].Value == 24)
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
                    
                }
            }else
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
                }
                InternalConnector = null;
                TimeIsFinished = false;
            }

        }
    }
}
