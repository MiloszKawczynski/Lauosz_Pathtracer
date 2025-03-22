using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class IntersectWith
    {
        private static float CalculateCoefficientDefiningPotentialIntersection(Line a, Line b)
        {
            float t;

            Vector nominator1;
            Vector nominator2;
            float denominator;

            nominator1 = b.GetPoint() - a.GetPoint();
            nominator1 = Vector.CrossProduct(nominator1, b.GetVector());

            nominator2 = Vector.CrossProduct(a.GetVector(), b.GetVector());

            t = nominator1 * nominator2;

            denominator = (float)Math.Pow((double)nominator2.Length(), 2);

            t = t / denominator;

            return t;
        }

        private static bool IsCoefficientValid(float t, Line a)
        {
            if (a is Section)
            {
                return t >= 0 && t <= 1;
            }
            else if (a is Ray)
            {
                return t >= 0;
            }
            return true;
        }

        public static Point? Intersetion(Line a, Line b)
        {
            float t1 = CalculateCoefficientDefiningPotentialIntersection(a, b);
            float t2 = CalculateCoefficientDefiningPotentialIntersection(b, a);

            Point Pa = (Point)(a.GetPoint() + a.GetVector() * t1);
            Point Pb = (Point)(b.GetPoint() + b.GetVector() * t2);

            if (Pa == Pb && IsCoefficientValid(t1, a) && IsCoefficientValid(t2, b))
            {
                Console.WriteLine("Przecięcie w punkcie: " + Pa);
                return Pa;
            }
            else
            {
                Console.WriteLine("Brak przecięcia");
                return null;
            }
        }

        public void Intersection(Line line, Sphere sphere)
        {
            float a = (float)Math.Pow(line.GetVector().Length(), 2);
            float b = 2 * (line.GetVector() * (line.GetPoint() - sphere.c));
            float c = (float)Math.Pow((line.GetPoint() - sphere.c).Length(), 2) - (float)Math.Pow(sphere.r, 2);

            float word = (float)Math.Sqrt(Math.Pow(b, 2) - (4 * a * c));

            if (word >= 0)
            {
                float t1 = (float)Math.Round((-b - word) / (2 * a), 2);
                float t2 = (float)Math.Round((-b + word) / (2 * a), 2);

                if (IsCoefficientValid(t1, line))
                {
                    Vector Pa = line.GetPoint() + line.GetVector() * t1;

                    Console.WriteLine("Punkt przeciecia " + Pa);
                }


                if (t1 != t2)
                {
                    if (IsCoefficientValid(t2, line))
                    {
                        Vector Pb = line.GetPoint() + line.GetVector() * t2;

                        Console.WriteLine("Punkt przeciecia " + Pb);
                    }
                }
            }
            else
            {
                Console.WriteLine("Sfera i prosta nie przecinaja sie");
            }
        }

    }
}
