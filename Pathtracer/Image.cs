using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace Pathtracer
{
    internal class Image
    {
        private Bitmap image;
        private VirtualCamera camera;
        public List<Primitive> scene = new List<Primitive>();

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

        public void RenderImage()
        {
            float pixelSize = 1.0f;
            LightIntensity backgroundColor = new LightIntensity(0.0f, 0.0f, 0.0f);
            LightIntensity objectsColor = new LightIntensity(1.0f, 0.0f, 0.0f);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    Point pixelPosition = new Point(((-image.Width * 0.5f) + (x + 0.5f) * pixelSize) + camera.position.X, ((image.Height * 0.5f) - (y + 0.5f) * pixelSize) + camera.position.Y, camera.position.Z);

                    Ray ray;

                    if (camera.projection == ProjectionType.Ortogonal)
                    {
                        ray = new Ray(pixelPosition, camera.front);
                    }
                    else
                    {
                        Vector vec = camera.position - camera.front * camera.focalLength;
                        Point cameraRayOrigin = new Point(vec.X, vec.Y, vec.Z);
                        ray = new Ray(cameraRayOrigin, pixelPosition - cameraRayOrigin);
                    }

                    List<Point> hits = new List<Point>();

                    for (int i = 0; i < scene.Count; i++)
                    {
                        List<Point> intersectionPoints = IntersectWith.IntersectionLineSphere(ray, (Sphere)scene[i]);
                        if (intersectionPoints != null)
                        {
                            hits.AddRange(intersectionPoints);
                        }
                    }

                    if (hits.Count > 0)
                    {
                        SetPixel(x, y, objectsColor);
                    }
                    else
                    {
                        SetPixel(x, y, backgroundColor);
                    }
                }
            }

            image.Save("output.jpg");
        }
    }
}
