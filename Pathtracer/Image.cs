using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
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
        private LightIntensity shadowColor = new LightIntensity(0.0f, 0.0f, 0.0f);
        private LightIntensity ambientLightColor = new LightIntensity(1.0f, 1.0f, 1.0f);
        private int numberOfRecurciveRay = 3;
        public List<Primitive> scene = new List<Primitive>();
        public List<LightSource> lightSources = new List<LightSource>();

        public Image(int width, int height, VirtualCamera camera)
        {
            image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            this.camera = camera;
        }

        public void SetPixel(Bitmap bitmap, int x, int y, LightIntensity pixel)
        {
            pixel = pixel.Clamp();

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

                    int recurciveRay = numberOfRecurciveRay;

                    if (camera.projection == ProjectionType.Ortogonal)
                    {
                        ray = new Ray(pixelPosition, camera.front);
                    }
                    else
                    {
                        Vector vec = camera.position - camera.front * camera.focalLength;
                        Point cameraRayOrigin = new Point(vec.X, vec.Y, vec.Z);
                        ray = new Ray(cameraRayOrigin, (pixelPosition - cameraRayOrigin).UnitVector());
                    }

                    CalculatePixel(x, y, pixelPosition, ray, recurciveRay);
                }
            }

            image = BlurOnEdges(DetectEdges());
            //image = DetectEdges();
            SaveImage();
        }

        private void CalculatePixel(int x, int y, Point pixelPosition, Ray ray, int recurciveRay)
        {
            List<Point> intersectionPoints;
            Primitive hitPrimitive = scene[0];
            Point hit = new Point(float.MaxValue, float.MaxValue, float.MaxValue);
            bool isAnythingHit = false;
            Vector hitNormal = new Vector(0.0f, 0.0f, 0.0f);

            for (int i = 0; i < scene.Count; i++)
            {
                intersectionPoints = IntersectWith.Intersect(ray, scene[i]);
                if (intersectionPoints != null && intersectionPoints.Count > 0)
                {
                    intersectionPoints.Sort((a, b) => (a - pixelPosition).Length().CompareTo((b - pixelPosition).Length()));

                    if ((intersectionPoints[0] - pixelPosition).Length() < (hit - pixelPosition).Length())
                    {
                        hit = intersectionPoints[0];
                        hitPrimitive = scene[i];
                        isAnythingHit = true;
                        if (scene[i] is Sphere)
                        {
                            hitNormal = ((Sphere)scene[i]).NormalAtPointOnSphere(hit);
                        }

                        if (scene[i] is Triangle)
                        {
                            hitNormal = ((Triangle)scene[i]).N;
                        }
                    }
                }
            }

            if (isAnythingHit)
            {
                if (hitPrimitive.material.isReflective)
                {
                    if (recurciveRay > 0)
                    {
                        Vector reflectDir = Vector.Reflect(ray.V, hitNormal);
                        Ray reflectRay = new Ray(hit + reflectDir * 0.01f, reflectDir);
                        CalculatePixel(x, y, pixelPosition, reflectRay, recurciveRay - 1);
                    }
                }
                else if (hitPrimitive.material.isRefractive)
                {
                    if (recurciveRay > 0)
                    {
                        float cosAngle = -(ray.V.Invert() * hitNormal);
                        float refractionRatio = 1 / hitPrimitive.material.indexOfRefraction;

                        float k = 1 - MathF.Pow(refractionRatio, 2) * (1 - MathF.Pow(cosAngle, 2));
                        Vector refractedDir = ray.V * refractionRatio + (refractionRatio * cosAngle - MathF.Sqrt(k)) * hitNormal;
                        Point refractPosition = hit + refractedDir * 0.01f;
                        Ray refractRay = new Ray(refractPosition, refractedDir);

                        for (int i = 0; i < scene.Count; i++)
                        {
                            intersectionPoints = IntersectWith.Intersect(refractRay, scene[i]);
                            if (intersectionPoints != null && intersectionPoints.Count > 0)
                            {
                                intersectionPoints.Sort((a, b) => (a - refractPosition).Length().CompareTo((b - refractPosition).Length()));

                                if ((intersectionPoints[0] - refractPosition).Length() < (hit - refractPosition).Length())
                                {
                                    hit = intersectionPoints[0];
                                    hitPrimitive = scene[i];
                                    if (scene[i] is Sphere)
                                    {
                                        hitNormal = ((Sphere)scene[i]).NormalAtPointOnSphere(hit);
                                    }

                                    if (scene[i] is Triangle)
                                    {
                                        hitNormal = ((Triangle)scene[i]).N;
                                    }
                                }
                            }
                        }

                        cosAngle = -(refractRay.V.Invert() * hitNormal);
                        refractionRatio = hitPrimitive.material.indexOfRefraction;

                        k = 1 - MathF.Pow(refractionRatio, 2) * (1 - MathF.Pow(cosAngle, 2));
                        refractedDir = refractRay.V * refractionRatio + (refractionRatio * cosAngle - MathF.Sqrt(k)) * hitNormal;
                        refractRay = new Ray(hit + refractedDir * 0.01f, refractedDir);

                        CalculatePixel(x, y, pixelPosition, refractRay, recurciveRay - 1);
                    }
                }
                else
                {
                    var ambient = hitPrimitive.material.Ka * ambientLightColor;
                    var calculatedLight = shadowColor;
                    Vector viewDir = ray.V.Invert().UnitVector();
                    foreach (var light in lightSources)
                    {
                        if (light is SurfaceLight surfaceLight)
                        {
                            foreach (var pointLightPos in surfaceLight.PointLights)
                            {
                                calculatedLight += CalculateLight(light, hit, hitPrimitive, hitNormal, viewDir);
                            }
                        }
                        else
                        {
                            calculatedLight += CalculateLight(light, hit, hitPrimitive, hitNormal, viewDir);
                        }
                    }

                    calculatedLight += ambient;
                    calculatedLight = hitPrimitive.color * calculatedLight;

                    SetPixel(image, x, y, calculatedLight);
                }
            }
            else
            {
                SetPixel(image, x, y, backgroundColor);
            }
        }

        private LightIntensity CalculateLight(LightSource light, Point hit, Primitive hitPrimitive, Vector hitNormal, Vector viewDir)
        {
            Vector lightDir = light.GetDirectionFrom(hit);
            float lightDistance = light.GetDistanceFrom(hit);

            Ray shadowRay = new Ray(hit + lightDir * 0.01f, lightDir);
            bool isInShadow = false;

            foreach (var primitive in scene)
            {
                List<Point> shadowIntersections = IntersectWith.Intersect(shadowRay, primitive);
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

            return Phong(hitPrimitive.material, hitNormal, isInShadow, light, lightDir, lightDistance, viewDir);
        }
        private LightIntensity Phong(Material objectMaterial,
            Vector normal, bool isInShadow, LightSource light, Vector lightDir, float lightDistance, Vector viewDir)
        {
            normal = normal.UnitVector();
            lightDir = lightDir.UnitVector();
            viewDir = viewDir.UnitVector();

            if (isInShadow)
            {
                return shadowColor;
            }

            float attenuation = 1.0f / (1.0f + 0.01f * lightDistance + 0.001f * lightDistance * lightDistance);

            float diffuseFactor = MathF.Max(normal * lightDir, 0.0f);
            LightIntensity diffuse = objectMaterial.Kd * diffuseFactor * light.LightIntensity * attenuation;

            Vector reflectDir = Vector.Reflect(lightDir.Invert(), normal);

            float specularFactor = MathF.Pow(MathF.Max(reflectDir * viewDir, 0.0f), objectMaterial.n);

            LightIntensity specular = objectMaterial.Ks * specularFactor * light.LightIntensity * attenuation;

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
