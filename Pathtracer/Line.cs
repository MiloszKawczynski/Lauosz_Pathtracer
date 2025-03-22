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