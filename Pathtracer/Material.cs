using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Material
    {
        public float Kd = 1.0f;
        public float Ks = 0.0f;
        public float Ka = 0.0f;
        public float n = 1f;
        public bool isReflective = false;

        public Material(float Kd, float Ks, float Ka, float n)
        {
            this.Kd = Kd;
            this.Ks = Ks;
            this.Ka = Ka;
            this.n = n;
        }

        public Material() { }
    }
}
