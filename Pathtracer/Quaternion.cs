using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Quaternion
    {
        private float a;
        private float b;
        private float c;
        private float d;

        public Quaternion(float a, float b, float c, float d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public static Quaternion operator +(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.a + b.a, a.b + b.b, a.c + b.c, a.d + b.d);
        }

        public static Quaternion operator -(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.a - b.a, a.b - b.b, a.c - b.c, a.d - b.d);
        }

        public static Quaternion operator *(Quaternion a, Quaternion b)
        {
            Vector vectorA = new Vector(a.b, a.c, a.d);
            Vector vectorB = new Vector(b.b, b.c, b.d);

            float scalarElement = a.a * b.a - vectorA * vectorB;
            Vector vectorElement = (vectorB * a.a) + (vectorA * b.a) + vectorA.CrossProduct(vectorA, vectorB);

            return new Quaternion(scalarElement, vectorElement.GetX(), vectorElement.GetY(), vectorElement.GetZ());
        }

        public static Quaternion operator /(Quaternion a, Quaternion b)
        {
            Vector vectorA = new Vector(a.b, a.c, a.d);
            Vector vectorB = new Vector(b.b, b.c, b.d);

            float divider = ((float)Math.Pow(b.a, 2) + vectorB * vectorB);
            float scalarElement = ((a.a * b.a + vectorA * vectorB) / divider);
            Vector vectorElement = (((vectorB * -a.a) + (vectorA * b.a) - vectorA.CrossProduct(vectorA, vectorB)) * (1.0f / divider));

            return new Quaternion(scalarElement, vectorElement.GetX(), vectorElement.GetY(), vectorElement.GetZ());
        }

        public static Vector rotate(Vector a, Vector axis, double alpha)
        {
            alpha = alpha * Math.PI / 180;
            Vector unitVector = axis.UnitVector();
            unitVector = unitVector * (float)Math.Sin(alpha / 2);
            Quaternion q = new Quaternion((float)Math.Cos(alpha / 2), unitVector.GetX(), unitVector.GetY(), unitVector.GetZ());
            Quaternion qInverse = new Quaternion((float)Math.Cos(alpha / 2), -unitVector.GetX(), -unitVector.GetY(), -unitVector.GetZ());

            Quaternion result = new Quaternion(0, a.GetX(), a.GetY(), a.GetZ());

            result = q * result * qInverse;

            Vector resultVector = new Vector(result.b, result.c, result.d);

            return resultVector;
        }

        public override String ToString()
        {
            String result = a.ToString() + " + i * " + b.ToString() + " + j * " + c.ToString() + " + k * " + d.ToString();
            return result;
        }

        public void setQuaternion(float a, float b, float c, float d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
    }
}
