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

            return a.GetPointAt(t);
        }

        public override String ToString()
        {
            String result = "P = " + p.ToString() + " + n" + n.ToString();
            return result;
        }

        public Point GetPoint()
        {
            return p;
        }

        public Vector GetNormal()
        {
            return n;
        }
    }
}