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