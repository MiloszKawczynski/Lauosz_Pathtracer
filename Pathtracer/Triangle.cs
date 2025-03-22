using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Triangle : Plane
    {
        private Point p1, p2, p3;

        public Triangle(Point p1, Point p2, Point p3) : base(p1, Vector.CrossProduct(p2 - p1, p3 - p1).UnitVector())
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.n = Vector.CrossProduct(p2 - p1, p3 - p1).UnitVector();
        }

        public Point GetP1()
        {
            return p1;
        }

        public Point GetP2()
        {
            return p2;
        }

        public Point GetP3()
        {
            return p3;
        }

        public override string ToString()
        {
            return "P1 = " + p1.ToString() + ", P2 = " + p2.ToString() + ", P3 = " + p3.ToString() + ", n = " + n.ToString();
        }
    }
}