using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using E_STM.Properties;

namespace E_STM
{

    class Motor3F: ElementNew
    {
        private faseName F1, F2, F3;
        public Motor3F(string Name, Point Location, Panel PlaceForElement):base(Name, Location, PlaceForElement)
        {
            this.RectOfElement = new Rectangle(this.RectOfElement.X, this.RectOfElement.Y, 110, 65);
            SizeElement = new Size(110, 65);
            List<Point> Conectors = new List<Point>();
            Conectors.Add(new Point(0, 1));
            Conectors.Add(new Point(0, 33));
            Conectors.Add(new Point(0, 64));

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
            if (ElementConectors[0].fase == faseName.U && ElementConectors[1].fase == faseName.V && ElementConectors[2].fase == faseName.W)
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Motor3FTL");
            }
            else if (ElementConectors[0].fase == faseName.V && ElementConectors[1].fase == faseName.U && ElementConectors[2].fase == faseName.W)
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Motor3FTR");
            }
            else if (ElementConectors[0].fase == faseName.V && ElementConectors[1].fase == faseName.W && ElementConectors[2].fase == faseName.U)
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Motor3FTL");
            }
            else if (ElementConectors[0].fase == faseName.W && ElementConectors[1].fase == faseName.V && ElementConectors[2].fase == faseName.U)
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Motor3FTR");
            }
            else if (ElementConectors[0].fase == faseName.W && ElementConectors[1].fase == faseName.U && ElementConectors[2].fase == faseName.V)
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Motor3FTL");
            }
            else if (ElementConectors[0].fase == faseName.U && ElementConectors[1].fase == faseName.W && ElementConectors[2].fase == faseName.V)
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Motor3FTR");
            }
            else
            {
                ImageElement = (Image)Resources.ResourceManager.GetObject("Motor3F");
            }
            // прорисовка элемента, когда изменилось состояние.
            if (ElementConectors[0].fase != F1 || ElementConectors[1].fase != F2 || ElementConectors[2].fase != F3)
            {
                PlaceForElement.Invalidate();
                F1 = ElementConectors[0].fase;
                F2 = ElementConectors[1].fase;
                F3 = ElementConectors[2].fase;
            }
        }
        private void RotateImage(ref Bitmap image, float angle)
        {
            using (Bitmap clone = (Bitmap)image.Clone())
            {
                using (Graphics gbmp = Graphics.FromImage(clone))
                {
                    gbmp.Clear(Color.Transparent);
                    gbmp.TranslateTransform(image.Width / 2f, image.Height / 2f);
                    gbmp.RotateTransform(angle);
                    gbmp.DrawImage(image, -image.Width / 2f, -image.Height / 2f);
                }
                image = (Bitmap)clone.Clone();
            }
        }
    }
}
