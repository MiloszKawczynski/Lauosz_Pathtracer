using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Sphere
    {
        public Vector c;
        public float r;

        public Sphere(Vector c, float r)
        {
            this.c = c;
            this.r = r;
        }
    }
}
