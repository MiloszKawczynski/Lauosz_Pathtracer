using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Line
    {
        private Point p;
        private Vector v;

        public Line(Point p, Vector v)
        {
            this.p = p;
            this.v = v;
        }


        public void sphereIntersection(Sphere sphere)
        {
            float a = (float)Math.Pow(v.Length(), 2);
            float b = 2 * (v * (p - sphere.c));
            float c = (float)Math.Pow((p - sphere.c).Length(), 2) - (float)Math.Pow(sphere.r, 2);

            float word = (float)Math.Sqrt(Math.Pow(b, 2) - (4 * a * c));

            if (word < 0)
            {
                Console.WriteLine("Sfera i prosta nie przecinaja sie");
            }
            else
            {
                float t1 = (float)Math.Round((-b - word) / (2 * a), 2);
                float t2 = (float)Math.Round((-b + word) / (2 * a), 2);

                Vector Pa = p + v * t1;

                Console.WriteLine("Punkt przeciecia " + Pa);


                if (t1 != t2)
                {
                    Vector Pb = p + v * t2;

                    Console.WriteLine("Punkt przeciecia " + Pb);
                }
            }
        }

        public Vector GetPointAt(float t)
        {
            float x = p.GetX() + v.GetX() * t;
            float y = p.GetY() + v.GetY() * t;
            float z = p.GetZ() + v.GetZ() * t;

            return new Vector(x, y, z);
        }

        public override String ToString()
        {
            String result = "l = " + p.ToString() + " + t" + v.ToString();
            return result;
        }

        public Point GetPoint()
        {
            return p;
        }

        public Vector GetVector()
        {
            return v;
        }
    }
}