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
        protected Vector n;

        public Point P => p;
        public Vector N => n;

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

    }
}