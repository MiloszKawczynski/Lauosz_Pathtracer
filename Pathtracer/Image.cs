using System.Drawing;
using System.Drawing.Imaging;

namespace Pathtracer
{
    internal class Image
    {
        private Bitmap image;

        public Image(int width, int height)
        {
            image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        }

        public void SetPixel(int x, int y, LightIntensity pixel)
        {
            int r, g, b;
            r = (int)(pixel.R * 255);
            g = (int)(pixel.G * 255);
            b = (int)(pixel.B * 255);
            image.SetPixel(x, y, Color.FromArgb(r, g, b));
        }
    }
}
