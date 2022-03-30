using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_STM
{
    class ImageAndPositionInDoc
    {
        public Image image { get; set; }
        public int position { get; set; }

        public ImageAndPositionInDoc(Image image, int position)
        {
            this.image = image;
            this.position = position;
        }
    }
}
