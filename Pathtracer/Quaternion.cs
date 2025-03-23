using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Quaternion
    {
        private float s;
        private Vector v;

        public float S => s;
        public Vector V => v;

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

        public static Quaternion FromAxisAngle(float angleDegrees, Vector axis)
        {
            float angleRad = (float)(angleDegrees * Math.PI / 180.0);
            float halfAngle = angleRad / 2.0f;

            float sinHalfAngle = (float)Math.Sin(halfAngle);
            float cosHalfAngle = (float)Math.Cos(halfAngle);

            float x = axis.X * sinHalfAngle;
            float y = axis.Y * sinHalfAngle;
            float z = axis.Z * sinHalfAngle;
            float w = cosHalfAngle;

            return new Quaternion(w, x, y, z);
        }

        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.v.X + b.v.X, a.v.Y + b.v.Y, a.v.Z + b.v.Z, a.s + b.s);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.v.X - b.v.X, a.v.Y - b.v.Y, a.v.Z - b.v.Z, a.s - b.s);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            float scalarElement = a.s * b.s -a.v * b.v;

            Vector vectorElement = (b.v * a.s) + (a.v * b.s) + Vector.CrossProduct(a.v, b.v);


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
            Vector vectorElement = (((b.v * -a.s) + (a.v * b.s) - Vector.CrossProduct(a.v, b.v)) * (1.0f / divider));

            return new Quaternion(scalarElement, vectorElement);
        }

        public Quaternion Inverse()
        {
            float absoluteValue = (float)(1 / Math.Pow(Norm(), 2));

            float scalarElement = Conjugate().s * absoluteValue;
            Vector vectorElement = Conjugate().v * absoluteValue;

            return new Quaternion(scalarElement, vectorElement);
        }

        public float Norm()
        {
            return (float)Math.Sqrt(Math.Pow(s, 2) + (v.X * v.X + v.Y * v.Y + v.Z * v.Z));
        }

        public Vector Eulers()
        {
            float t0 = 2.0f * (s * v.X + v.Y * v.Z);
            float t1 = 1.0f - 2.0f * (v.X * v.X + v.Y * v.Y);
            float roll = (float)Math.Atan2(t0, t1);

            float t2 = 2.0f * (s * v.Y - v.Z * v.X);
            t2 = t2 > 1.0f ? 1.0f : t2;
            t2 = t2 < -1.0f ? -1.0f : t2;
            float pitch = (float)Math.Asin(t2);

            float t3 = 2.0f * (s * v.Z + v.X * v.Y);
            float t4 = 1.0f - 2.0f * (v.Y * v.Y + v.Z * v.Z);
            float yaw = (float)Math.Atan2(t3, t4);

            // Konwersja na stopnie
            roll = roll * (180.0f / (float)Math.PI);
            pitch = pitch * (180.0f / (float)Math.PI);
            yaw = yaw * (180.0f / (float)Math.PI);

            return new Vector(roll, pitch, yaw);
        }

        public Quaternion Normalize()
        {
            return this * (1 / Norm());
        }

        public Quaternion Conjugate()
        {
            return new Quaternion(s, v * -1);
        }

        public static float DotProduct(Quaternion a, Quaternion b)
        {
            return (float)(a.v.X * b.v.X + a.v.Y * b.v.Y + a.v.Z * b.v.Z + a.s * b.s);
        }

        public override String ToString()
        {
            String result = s.ToString() + " + i * " + v.X.ToString() + " + j * " + v.Y.ToString() + " + k * " + v.Z.ToString();
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
