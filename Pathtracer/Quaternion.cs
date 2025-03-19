using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Quaternion
    {
        private float s;
        private Vector v;

        public Quaternion(float s, float x, float y, float z)
        {
            this.s = s;
            this.v = new Vector(x, y, z);
        }

        public Quaternion(float s, Vector v)
        {
            this.s = s;
            this.v = v;
        }

        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.v.GetX() + b.v.GetX(), a.v.GetY() + b.v.GetY(), a.v.GetZ() + b.v.GetZ(), a.s + b.s);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.v.GetX() - b.v.GetX(), a.v.GetY() - b.v.GetY(), a.v.GetZ() - b.v.GetZ(), a.s - b.s);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            float scalarElement = a.s * b.s - a.v * b.v;
            Vector vectorElement = (b.v * a.s) + (a.v * b.s) + a.v.CrossProduct(a.v, b.v);

            return new Quaternion(scalarElement, vectorElement);
        }

        public static Quaternion operator *(Quaternion a, float b)
        {
            float scalarElement = a.s * b;
            Vector vectorElement = a.v * b;

            return new Quaternion(scalarElement, vectorElement);
        }

        public static Quaternion operator /(Quaternion a, Quaternion b)
        {
            float divider = ((float)Math.Pow(b.s, 2) + a.v * b.v);
            float scalarElement = ((a.s * b.s + a.v * b.v) / divider);
            Vector vectorElement = (((b.v * -a.s) + (a.v * b.s) - a.v.CrossProduct(a.v, b.v)) * (1.0f / divider));

            return new Quaternion(scalarElement, vectorElement);
        }

        public float Norm()
        {
            return (float)Math.Sqrt(Math.Pow(s, 2) + (v.GetX() * v.GetX() + v.GetY() * v.GetY() + v.GetZ() * v.GetZ()));
        }

        public static Vector Rotate(Vector a, Vector axis, double alpha)
        {
            alpha = alpha * Math.PI / 180;
            Vector unitVector = axis.UnitVector();
            unitVector = unitVector * (float)Math.Sin(alpha / 2);
            Quaternion q = new Quaternion((float)Math.Cos(alpha / 2), unitVector);
            Quaternion qInverse = new Quaternion((float)Math.Cos(alpha / 2), unitVector * -1);

            Quaternion result = new Quaternion(0, a.GetX(), a.GetY(), a.GetZ());

            result = q * result * qInverse;

            return result.v;
        }

        public override String ToString()
        {
            String result = s.ToString() + " + i * " + v.GetX().ToString() + " + j * " + v.GetY().ToString() + " + k * " + v.GetZ().ToString();
            return result;
        }

        public void Set(float s, float x, float y, float z)
        {
            this.s = s;
            this.v.Set(x, y, z);
        }
        public void Set(float s, Vector v)
        {
            this.s = s;
            this.v = v;
        }
    }
}
