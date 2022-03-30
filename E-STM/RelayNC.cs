using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    public class RelayNC: ElementNew
    {
        private EdgeSelect Edge;
        private ConnectorNew InternalConnector;
        private System.Timers.Timer TimerDelay = new System.Timers.Timer();
        private bool UpdateScreen;
        public RelayNC(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            this.NamePicture = Name;
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
            this.Edge = new EdgeSelect(false);
            TimerDelay.Interval = 250;
            TimerDelay.AutoReset = false;
            TimerDelay.Elapsed += TimerDelay_Elapsed;
        }

        private void TimerDelay_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!(ElementConectors[0].Value > 0 && ElementConectors[1].Value == 0 || ElementConectors[0].Value == 0 && ElementConectors[1].Value > 0))
            {
                if (InternalConnector == null)
                {
                    InternalConnector = new ConnectorNew(ElementConectors[2], ElementConectors[3], PlaceForElement, this, this);
                    InternalConnector.NotDrawNothing = true;
                    (Application.OpenForms[0] as Form1).ConectorPractical.Add(InternalConnector);
                }
                ImageElement = (Image)Resources.ResourceManager.GetObject("ReleyCL");
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
            if (ElementConectors[0].Value > 0 && ElementConectors[1].Value == 0 || ElementConectors[0].Value == 0 && ElementConectors[1].Value > 0)
            {

                if (InternalConnector != null)
                {
                    InternalConnector.deliteConnector();
                    (Application.OpenForms[0] as Form1).ConectorPractical.Remove(InternalConnector);
                }
                InternalConnector = null;
                ImageElement = (Image)Resources.ResourceManager.GetObject("ReleyOPN");

                UpdateScreen = true;
            }
            else
            {
                TimerDelay.Start();
            }
            //ElementConectors[0].Value = ElementConectors[1].Value = -1;
        }
    }
}
