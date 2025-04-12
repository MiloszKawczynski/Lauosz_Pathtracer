using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Pathtracer.Light;
using Pathtracer.Primitives;
using static System.Net.Mime.MediaTypeNames;

namespace Pathtracer
{
    internal class Image
    {
        private Bitmap image;
        private VirtualCamera camera;
        private LightIntensity backgroundColor = new LightIntensity(0.0f, 0.0f, 0.0f);
        public List<Primitive> scene = new List<Primitive>();
        public List<LightSource> lightSources = new List<LightSource>();

        public Image(int width, int height, VirtualCamera camera)
        {
            image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            this.camera = camera;
        }

        public void SetPixel(Bitmap bitmap, int x, int y, LightIntensity pixel)
        {
            int r, g, b;
            r = (int)(pixel.R * 255);
            g = (int)(pixel.G * 255);
            b = (int)(pixel.B * 255);
            bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
        }

        public LightIntensity GetPixel(Bitmap bitmap, int x, int y)
        {
            Color pixel = bitmap.GetPixel(x, y);
            Color color = Color.FromArgb(
            pixel.A,
            (int)(pixel.R),
            (int)(pixel.G),
            (int)(pixel.B));

            return new LightIntensity((float)color.R / 255, (float)color.G / 255, (float)color.B / 255.0f);
        }

        public void RenderImage()
        {
            float pixelSize = 1.0f;

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

                    List<Point> intersectionPoints;
                    Primitive hitPrimitive = scene[0];
                    Point hit = new Point(float.MaxValue, float.MaxValue, float.MaxValue);
                    bool isAnythingHit = false;
                    Vector hitNormal = new(0.0f, 0.0f, 0.0f);

                    for (int i = 0; i < scene.Count; i++)
                    {
                        intersectionPoints = IntersectWith.IntersectionLineSphere(ray, (Sphere)scene[i]);
                        if (intersectionPoints != null)
                        {
                            intersectionPoints.Sort((a, b) => (a - pixelPosition).Length().CompareTo((b - pixelPosition).Length()));

                            if ((intersectionPoints[0] - pixelPosition).Length() < (hit - pixelPosition).Length())
                            {
                                hit = intersectionPoints[0];
                                hitPrimitive = scene[i];
                                isAnythingHit = true;
                                hitNormal = ((Sphere)scene[i]).NormalAtPointOnSphere(hit);
                            }
                        }
                    }

                    if (isAnythingHit)
                    {
                        var ambient = hitPrimitive.color * hitPrimitive.material.Ka;
                        LightIntensity calculatedLight = ambient;
                        Vector viewDir = (camera.position - hit).UnitVector();
                        foreach (var light in lightSources)
                        {
                            if (light is SurfaceLight surfaceLight)
                            {
                                foreach (var pointLightPos in surfaceLight.PointLights)
                                {
                                    Vector lightDir = light.GetDirectionFrom(hit);
                                    float lightDistance = light.GetDistanceFrom(hit);

                                    Ray shadowRay = new Ray((hitNormal + hit), lightDir);
                                    bool isInShadow = false;

                                    foreach (var primitive in scene)
                                    {

                                        List<Point> shadowIntersections = IntersectWith.IntersectionLineSphere(shadowRay, (Sphere)primitive);
                                        if (shadowIntersections != null && shadowIntersections.Count > 0)
                                        {
                                            foreach (var shadowHit in shadowIntersections)
                                            {
                                                if ((shadowHit - hit).Length() < lightDistance)
                                                {
                                                    isInShadow = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (isInShadow)
                                        {
                                            break;
                                        }
                                    }
                                    calculatedLight += Phong(hitPrimitive.material, hitNormal, isInShadow, light, lightDir, viewDir);
                                }
                            }
                            else
                            {
                                Vector lightDir = light.GetDirectionFrom(hit);
                                float lightDistance = light.GetDistanceFrom(hit);

                                Ray shadowRay = new Ray(hit + lightDir * 0.01f, lightDir);
                                bool isInShadow = false;

                                foreach (var primitive in scene)
                                {

                                    List<Point> shadowIntersections = IntersectWith.IntersectionLineSphere(shadowRay, (Sphere)primitive);
                                    if (shadowIntersections != null && shadowIntersections.Count > 0)
                                    {
                                        foreach (var shadowHit in shadowIntersections)
                                        {
                                            if ((shadowHit - hit).Length() < lightDistance)
                                            {
                                                isInShadow = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (isInShadow)
                                    {
                                        break;
                                    }
                                }

                                calculatedLight += Phong(hitPrimitive.material, hitNormal, isInShadow, light, lightDir, viewDir);
                            }
                        }
                        SetPixel(image, x, y, calculatedLight);
                    }
                    else
                    {
                        SetPixel(image, x, y, backgroundColor);
                    }
                }
            }

            image = BlurOnEdges(DetectEdges());
            SaveImage();
        }

        private LightIntensity Phong(Material objectMaterial,
            Vector normal, bool isInShadow, LightSource light, Vector lightDir, Vector viewDir)
        {
            normal = normal.UnitVector();
            lightDir = lightDir.UnitVector();
            viewDir = viewDir.UnitVector();

            if (isInShadow)
            {
                return new(0f, 0f, 0f);
            }

            float diffuseFactor = MathF.Max(lightDir * normal, 0.0f);
            LightIntensity diffuse = objectMaterial.Kd * diffuseFactor * light.LightIntensity;


            Vector reflectDir = Vector.Reflect(lightDir.Invert(), normal);

            float specularFactor = MathF.Pow(MathF.Max(reflectDir * viewDir, 0.0f), objectMaterial.n);

            LightIntensity specular = objectMaterial.Ks * specularFactor * light.LightIntensity;


            return diffuse + specular;
        }

        public Bitmap Blur()
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);

            List<float> weights = new List<float> { 0.38774f, 0.24477f, 0.06136f };

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    LightIntensity sum = new LightIntensity(GetPixel(image, x, y) * weights[0]);

                    for (int i = 1; i < 3; i++)
                    {
                        if (x + i < image.Width)
                        {
                            sum = new LightIntensity(sum + (GetPixel(image, x + i, y) * weights[i]));
                        }

                        if (x - i >= 0)
                        {
                            sum = new LightIntensity(sum + (GetPixel(image, x - i, y) * weights[i]));
                        }
                    }

                    SetPixel(newImage, x, y, sum);
                }
            }

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    LightIntensity sum = new LightIntensity(GetPixel(newImage, x, y) * weights[0]);

                    for (int i = 1; i < 3; i++)
                    {
                        if (y + i < image.Height)
                        {
                            sum = new LightIntensity(sum + (GetPixel(newImage, x, y + i) * weights[i]));
                        }

                        if (y - i >= 0)
                        {
                            sum = new LightIntensity(sum + (GetPixel(newImage, x, y - i) * weights[i]));
                        }
                    }

                    SetPixel(newImage, x, y, sum);
                }
            }

            return newImage;
        }

        public Bitmap BlurOnEdges(Bitmap edges)
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);

            List<float> weights = new List<float> { 0.38774f, 0.24477f, 0.06136f };

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    LightIntensity sum = new LightIntensity(GetPixel(image, x, y) * weights[0]);

                    for (int i = 1; i < 3; i++)
                    {
                        if (x + i < image.Width)
                        {
                            sum = new LightIntensity(sum + (GetPixel(image, x + i, y) * weights[i]));
                        }

                        if (x - i >= 0)
                        {
                            sum = new LightIntensity(sum + (GetPixel(image, x - i, y) * weights[i]));
                        }
                    }

                    if (GetPixel(edges, x, y) == new LightIntensity(1.0f, 1.0f, 1.0f))
                    {
                        SetPixel(newImage, x, y, sum);
                    }
                    else
                    {
                        SetPixel(newImage, x, y, GetPixel(image, x, y));
                    }
                }
            }

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    LightIntensity sum = new LightIntensity(GetPixel(newImage, x, y) * weights[0]);

                    for (int i = 1; i < 3; i++)
                    {
                        if (y + i < image.Height)
                        {
                            sum = new LightIntensity(sum + (GetPixel(newImage, x, y + i) * weights[i]));
                        }

                        if (y - i >= 0)
                        {
                            sum = new LightIntensity(sum + (GetPixel(newImage, x, y - i) * weights[i]));
                        }
                    }

                    if (GetPixel(edges, x, y) == new LightIntensity(1.0f, 1.0f, 1.0f))
                    {
                        SetPixel(newImage, x, y, sum);
                    }
                    else
                    {
                        SetPixel(newImage, x, y, GetPixel(image, x, y));
                    }
                }
            }

            return newImage;
        }

        public Bitmap DetectEdges()
        {
            Bitmap newImage = new Bitmap(image.Width, image.Height);

            float[] sobelKernelX = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            float[] sobelKernelY = { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
            float maxGradient = MathF.Sqrt(8 * 8 + 8 * 8);
            float threshold = 0.15f;

            for (int y = 0; y < newImage.Height; y++)
            {
                for (int x = 0; x < newImage.Width; x++)
                {
                    float redEdgeX = 0.0f, redEdgeY = 0.0f;
                    float greenEdgeX = 0.0f, greenEdgeY = 0.0f;
                    float blueEdgeX = 0.0f, blueEdgeY = 0.0f;

                    for (int yy = -1; yy <= 1; yy++)
                    {
                        for (int xx = -1; xx <= 1; xx++)
                        {
                            int sampleX = Math.Clamp(x + xx, 0, newImage.Width - 1);
                            int sampleY = Math.Clamp(y + yy, 0, newImage.Height - 1);

                            LightIntensity pixel = GetPixel(image, sampleX, sampleY);
                            int index = (yy + 1) * 3 + (xx + 1);

                            redEdgeX += pixel.R * sobelKernelX[index];
                            redEdgeY += pixel.R * sobelKernelY[index];

                            greenEdgeX += pixel.G * sobelKernelX[index];
                            greenEdgeY += pixel.G * sobelKernelY[index];

                            blueEdgeX += pixel.B * sobelKernelX[index];
                            blueEdgeY += pixel.B * sobelKernelY[index];
                        }
                    }

                    float redGradient = (float)Math.Sqrt(redEdgeX * redEdgeX + redEdgeY * redEdgeY) / maxGradient;
                    float greenGradient = (float)Math.Sqrt(greenEdgeX * greenEdgeX + greenEdgeY * greenEdgeY) / maxGradient;
                    float blueGradient = (float)Math.Sqrt(blueEdgeX * blueEdgeX + blueEdgeY * blueEdgeY) / maxGradient;

                    float maxChannelGradient = Math.Max(Math.Max(redGradient, greenGradient), blueGradient);

                    float intensity = maxChannelGradient > threshold ? 1.0f : 0.0f;

                    SetPixel(newImage, x, y, new LightIntensity(intensity, intensity, intensity));
                }
            }
            return newImage;
        }

        private void SaveImage()
        {
            image.Save("output.jpg");
        }
    }
}
