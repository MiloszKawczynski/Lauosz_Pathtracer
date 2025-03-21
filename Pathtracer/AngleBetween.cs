using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class AngleBetween
    {
        public static float Angle(Vector a, Vector b)
        {
            return (float)Math.Acos((a * b) / (a.Length() * b.Length()));
        }

        public static float Angle(Line a, Plane b)
        {
            float result = Angle(a.GetVector(), b.GetNormal());
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

        public float Angle(Plane a, Plane b)
        {
            return Angle(a.GetNormal(), b.GetNormal());
        }

        public float Angle(Line a, Line b)
        {
            return Angle(a.GetVector(), b.GetVector());
        }
    }
}
