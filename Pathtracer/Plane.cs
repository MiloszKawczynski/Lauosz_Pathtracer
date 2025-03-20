using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Plane
    {
        private Vector P;
        private Vector n;

        public Plane(Vector P, Vector n)
        {
            this.P = P;
            this.n = n;
        }

        public Line Intersetion(Plane a)
        {
            Vector lineVector = Vector.CrossProduct(n, a.n);

            return new Line(P, lineVector);
        }
        public Vector IntersetionWithLine(Line a)
        {
            float nominator;
            float denominator;

            nominator = n.Invert() * (a.p - P);
            denominator = n * a.v;

            float t = nominator / denominator;

            return a.getPoint(t);
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

        public float angleBetween(Plane a)
        {
            float nominator = a.n * n;
            float denominator = (a.n.Length() * n.Length());
            float result = nominator / denominator;
            result = (float)(Math.Acos(result) * 180.0f / Math.PI);
            return result;
        }

        public override String ToString()
        {
            String result = "P = " + P.ToString() + " + n" + n.ToString();
            return result;
        }

    }
}