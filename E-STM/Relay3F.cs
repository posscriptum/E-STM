using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    class Relay3F: ElementNew
    {
        private EdgeSelect Edge;
        private ConnectorNew InternalConnector1, InternalConnector2, InternalConnector3;
        private System.Timers.Timer TimerDelay = new System.Timers.Timer();
        private bool UpdateScreen;
        public Relay3F(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            this.RectOfElement = new Rectangle(this.RectOfElement.X, this.RectOfElement.Y, 110, 90);
            SizeElement = new Size(110, 90);
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(0, 15));
            Conectors.Add(new Point(0, 47));
            Conectors.Add(new Point(0, 65));
            Conectors.Add(new Point(0, 83));
            Conectors.Add(new Point(110, 15));
            Conectors.Add(new Point(110, 47));
            Conectors.Add(new Point(110, 65));
            Conectors.Add(new Point(110, 83));

            ConectorPoints = Conectors;

            foreach (Point point in ConectorPoints)
            {
                ElementConectors.Add(new ConnectPoint(-1, point));
                PreValue.Add(-1);
            }
            CalculatePositionAreaPoint();
            this.Edge = new EdgeSelect(false);
            TimerDelay.Interval = 250;
            TimerDelay.AutoReset = false;
            TimerDelay.Elapsed += TimerDelay_Elapsed;
        }

        private void TimerDelay_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!(ElementConectors[0].Value > 10 && ElementConectors[4].Value == 0 || ElementConectors[0].Value == 0 && ElementConectors[4].Value > 10))
            {
                if (InternalConnector1 != null)
                {
                    InternalConnector1.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPractical.Remove(InternalConnector1);
                    InternalConnector2.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPractical.Remove(InternalConnector2);
                    InternalConnector3.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPractical.Remove(InternalConnector3);
                }
                InternalConnector1 = null;
                InternalConnector2 = null;
                InternalConnector3 = null;
                ImageElement = (Image)Resources.ResourceManager.GetObject("Reley3FOPN");
            }
            if (UpdateScreen)
            {
                PlaceForElement.Invalidate();
                UpdateScreen = false;
            }
            //проверить какие соединения подключены к 
        }

        public override void implementation()
        {
            if (ElementConectors[0].Value > 10 && ElementConectors[4].Value == 0 || ElementConectors[0].Value == 0 && ElementConectors[4].Value > 10)
            {
                if (InternalConnector1 == null)
                {
                    InternalConnector1 = new ConnectorNew(ElementConectors[1], ElementConectors[5], PlaceForElement, this, this);
                    InternalConnector1.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPractical.Add(InternalConnector1);
                    InternalConnector2 = new ConnectorNew(ElementConectors[2], ElementConectors[6], PlaceForElement, this, this);
                    InternalConnector2.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPractical.Add(InternalConnector2);
                    InternalConnector3 = new ConnectorNew(ElementConectors[3], ElementConectors[7], PlaceForElement, this, this);
                    InternalConnector3.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPractical.Add(InternalConnector3);
                }
                ImageElement = (Image)Resources.ResourceManager.GetObject("Reley3FCL");
                UpdateScreen = true;
            }
            else
            {
                TimerDelay.Start();
            }
        }
    }
}
