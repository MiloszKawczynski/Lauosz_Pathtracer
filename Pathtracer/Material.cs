﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Material
    {
        public float Kd = 0.8f;
        public float Ks = 0.0f;
        public float Ka = 0.1f;
        public float n = 0.5f;

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
