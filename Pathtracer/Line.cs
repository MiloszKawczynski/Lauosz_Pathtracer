using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Line
    {
        public Vector P;
        public Vector v;

        public Line(Vector P, Vector v)
        {
            this.P = P;
            this.v = v;
        }

        public Vector? Intersetion(Line a, Line b)
        {
            float t1;
            float t2;

            Vector nominator1;
            Vector nominator2;
            float denominator;

            //----------------------------------------------------------------------

            nominator1 = b.P - a.P;
            nominator1 = nominator1.CrossProduct(nominator1, b.v);

            nominator2 = a.v.CrossProduct(a.v, b.v);

            t1 = nominator1 * nominator2;

            denominator = (float)Math.Pow((double)nominator2.Length(), 2);

            t1 = t1 / denominator;

            //----------------------------------------------------------------------

            nominator1 = b.P - a.P;
            nominator1 = nominator1.CrossProduct(nominator1, a.v);

            nominator2 = a.v.CrossProduct(b.v, a.v);

            t2 = nominator1 * nominator2;

            denominator = (float)Math.Pow((double)nominator2.Length(), 2);

            t2 = -t2 / denominator;

            //----------------------------------------------------------------------

            Vector Pa = a.P + a.v * t1;
            Vector Pb = b.P + b.v * t2;

            if (Pa == Pb)
            {
                Console.WriteLine("Proste przecinaja sie w punkcie: " + Pa);
                return Pa;
            }
            else
            {
                Console.WriteLine("Proste nie przecinaja sie");
                return null;
            }
        }

        public Vector? IntersetionSection(Line a, Line b)
        {
            float t1;
            float t2;

            Vector nominator1;
            Vector nominator2;
            float denominator;

            //----------------------------------------------------------------------

            nominator1 = b.P - a.P;
            nominator1 = nominator1.CrossProduct(nominator1, b.v);

            nominator2 = a.v.CrossProduct(a.v, b.v);

            t1 = nominator1 * nominator2;

            denominator = (float)Math.Pow((double)nominator2.Length(), 2);

            t1 = t1 / denominator;

            //----------------------------------------------------------------------

            nominator1 = b.P - a.P;
            nominator1 = nominator1.CrossProduct(nominator1, a.v);

            nominator2 = b.v.CrossProduct(b.v, a.v);

            t2 = nominator1 * nominator2;

            denominator = (float)Math.Pow((double)nominator2.Length(), 2);

            t2 = -t2 / denominator;

            //----------------------------------------------------------------------

            Vector Pa = a.P + a.v * t1;
            Vector Pb = b.P + b.v * t2;

            if ((Pa == Pb) && (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1))
            {
                Console.WriteLine("Odcinki przecinaja sie w punkcie: " + Pa);
                return Pa;
            }
            else
            {
                Console.WriteLine("Odcinki nie przecinaja sie");
                return null;
            }
        }

        public void sphereIntersection(Sphere sphere)
        {
            float a = (float)Math.Pow(v.Length(), 2);
            float b = 2 * (v * (P - sphere.c));
            float c = (float)Math.Pow((P - sphere.c).Length(), 2) - (float)Math.Pow(sphere.r, 2);

            float word = (float)Math.Sqrt(Math.Pow(b, 2) - (4 * a * c));

            if (word < 0)
            {
                Console.WriteLine("Sfera i prosta nie przecinaja sie");
            }
            else
            {
                float t1 = (float)Math.Round((-b - word) / (2 * a),2);
                float t2 = (float)Math.Round((-b + word) / (2 * a), 2);

                Vector Pa = P + v * t1;

                Console.WriteLine("Punkt przeciecia " + Pa);


                if (t1!=t2)
                {
                    Vector Pb = P + v * t2;

                    Console.WriteLine("Punkt przeciecia " + Pb);
                }
            }
        }

        public float angleBetween(Line a, Line b)
        {
            float nominator = a.v * b.v;
            float denominator = (a.v.Length() * b.v.Length());
            float result = nominator / denominator;
            result = (float)(Math.Acos(result) * 180.0f / Math.PI);
            return result;
        }

        public Vector getPoint(float t)
        {
            float x = P.GetX() + v.GetX() * t;
            float y = P.GetY() + v.GetY() * t;
            float z = P.GetZ() + v.GetZ() * t;

            return new Vector(x, y, z);
        }

        public override String ToString()
        {
            String result = "l = " + P.ToString() + " + t" + v.ToString();
            return result;
        }

    }
}