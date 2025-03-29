using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Sphere : Primitive
    {
        public Point c;
        public float r;

        public Sphere(Point c, float r)
        {
            this.c = c;
            this.r = r;
        }

        public Vector? NormalAtPointOnSphere(Point p)
        {
            Vector centerToPoint = p - c;

            if (centerToPoint.Length() > r)
            {
                Console.WriteLine("Warning: Point is outside sphere.");
                return null;
            }
            else if (centerToPoint.Length() < r)
            {
                Console.WriteLine("Warning: Point is inside sphere.");
                return null;
            }
            else
            {
                return centerToPoint.UnitVector();
            }
        }
    }
}
