using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{
    class CoinNew:ElementNew
    {
        public CoinNew(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
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
        }
    }
}
