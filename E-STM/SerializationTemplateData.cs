using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_STM
{
    [Serializable]
    public class SerializationTemplateData
    {
        public List<string> type;
        public List<string> namePicture;
        public List<Point> point;
        public List<Point> ConnectorPointOne;
        public List<Point> ConnectorPointTwo;
    }
}
