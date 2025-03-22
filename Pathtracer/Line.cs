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
        private Point p2;
        private Vector v;

        public Point P => p;
        public Point P2 => p2;
        public Vector V => v;

        public Line(Point p, Vector v)
        {
            this.p = p;
            this.v = v;
        }

        public Line(Point p, Point p2)
        {
            this.p = p;
            this.p2 = p2;
            v = (new Vector(p2.X - p.X, p2.Y - p.Y, p2.Z - p.Z)).UnitVector();
        }

        public Point GetPointAt(float t)
        {
            float x = p.X + v.X * t;
            float y = p.Y + v.Y * t;
            float z = p.Z + v.Z * t;

            return new Point(x, y, z);
        }

        public override String ToString()
        {
            String result = "l = " + p.ToString() + " + t" + v.ToString();
            return result;
        }

    }
}