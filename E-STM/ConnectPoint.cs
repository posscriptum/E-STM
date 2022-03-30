using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_STM.Properties;

namespace E_STM
{
    public class ConnectPoint
    {
        public int Value;
        public Point point;
        public faseName fase;

        public ConnectPoint(int Value, Point point)
        {
            this.Value = Value;
            this.point = point;
            fase = faseName.no;
        }

    }
}
