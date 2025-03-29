using System.Drawing;
using System.Drawing.Imaging;

namespace Pathtracer
{
    internal class Image
    {
        private Bitmap image;
        private VirtualCamera camera;

        public Image(int width, int height, VirtualCamera camera)
        {
            image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            this.camera = camera;
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
