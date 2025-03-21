using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Plane
    {
        private Point p;
        private Vector n;

        public Plane(Point p, Vector n)
        {
            this.p = p;
            this.n = n;
        }

        public Line Intersetion(Plane a)
        {
            Vector lineVector = Vector.CrossProduct(n, a.n);

            return new Line(p, lineVector);
        }
        public Vector IntersetionWithLine(Line a)
        {
            float nominator;
            float denominator;

            nominator = n.Invert() * (a.p - p);
            denominator = n * a.v;

            float t = nominator / denominator;

            return a.GetPoint(t);
        }

        public float angleBetweenPlaneAndLine(Line a)
        {
            float nominator = a.v * n;
            float denominator = (a.v.Length() * n.Length());
            float result = nominator / denominator;
            result = (float)(Math.Acos(result) * 180.0f / Math.PI);
            if (result > 90)
            {
                result = result - 90;
            }
            else if (result < 90)
            {
                result = 90 - result;
            }
            return result;
        }

        public float AngleBetween(Plane a)
        {
            return Vector.AngleRad(a.n, n);
        }

        public override String ToString()
        {
            String result = "P = " + p.ToString() + " + n" + n.ToString();
            return result;
        }

    }
}