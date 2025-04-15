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

        private LightIntensity TraceRay(Ray ray, int depth)
        {
            if (depth <= 0)
            {
                return backgroundColor;
            }

            Point closestHit = default;
            Primitive hitPrimitive = null;
            Vector hitNormal = new Vector(0.0f, 0.0f, 0.0f);
            float closestDistance = float.MaxValue;

            foreach (var primitive in scene)
            {
                var intersections = IntersectWith.Intersect(ray, primitive);
                if (intersections != null && intersections.Count > 0)
                {
                    intersections.Sort((a, b) => (a - ray.V).Length().CompareTo((b - ray.V).Length()));
                    var candidateHit = intersections[0];
                    float distance = (candidateHit - ray.V).Length();
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestHit = candidateHit;
                        hitPrimitive = primitive;

                        if (primitive is Sphere sphere)
                            hitNormal = sphere.NormalAtPointOnSphere(candidateHit);
                        else if (primitive is Triangle triangle)
                            hitNormal = triangle.N;
                    }
                }
            }

            if (hitPrimitive == null)
            {
                return backgroundColor;
            }

            var material = hitPrimitive.material;

            if (!material.isReflective && !material.isRefractive)
            {
                LightIntensity intensity = material.Ka * ambientLightColor;

                foreach (var light in lightSources)
                {
                    if (!IsInShadow(closestHit, light))
                    {
                        Vector lightDir = light.GetDirectionFrom(closestHit);
                        float lightDist = light.GetDistanceFrom(closestHit);
                        Vector viewDir = ray.V.Invert().UnitVector();
                        intensity += Phong(material, hitNormal, false, light, lightDir, lightDist, viewDir);
                    }
                }

                return hitPrimitive.color * intensity;
            }

            Vector incident = ray.V.Invert().UnitVector();

            if (material.isReflective)
            {
                Vector reflectedDir = Vector.Reflect(ray.V, hitNormal);
                Ray reflectedRay = new Ray(closestHit + reflectedDir * 0.01f, reflectedDir);
                return TraceRay(reflectedRay, depth - 1);
            }

            if (material.isRefractive)
            {
                float ior = material.indexOfRefraction;

                float cosTheta = -(incident * hitNormal);
                bool entering = cosTheta < 0;
                Vector normal = entering ? hitNormal : hitNormal.Invert();
                cosTheta = Math.Abs(cosTheta);

                float refractionRatio = entering ? (1.0f / ior) : ior;
                float k = 1 - refractionRatio * refractionRatio * (1 - cosTheta * cosTheta);

                if (k >= 0)
                {
                    Vector refractedDir = ray.V * refractionRatio +
                                       (refractionRatio * cosTheta - MathF.Sqrt(k)) * normal;
                    return TraceRay(new Ray(closestHit + refractedDir * 0.01f, refractedDir), depth - 1);
                }
                
                Vector reflectedDir = Vector.Reflect(ray.V, normal);
                return TraceRay(new Ray(closestHit + reflectedDir * 0.01f, reflectedDir), depth - 1);
            }

            return backgroundColor;
        }

        private bool IsInShadow(Point point, LightSource light)
        {
            Vector lightDir = light.GetDirectionFrom(point);
            float lightDist = light.GetDistanceFrom(point);
            Ray shadowRay = new Ray(point + lightDir * 0.01f, lightDir);

            foreach (var primitive in scene)
            {
                var intersections = IntersectWith.Intersect(shadowRay, primitive);
                if (intersections != null)
                {
                    if (primitive.material.isRefractive)
                    { 
                        continue;
                    }
                    foreach (var hit in intersections)
                    {
                        if ((hit - point).Length() < lightDist)
                            return true;
                    }
                }
            }

            return false;
        }


        private void CalculatePixel(int x, int y, Point pixelPosition, Ray ray, int recursiveRayCount)
        {
            LightIntensity finalColor = TraceRay(ray, recursiveRayCount);
            SetPixel(image, x, y, finalColor);
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
