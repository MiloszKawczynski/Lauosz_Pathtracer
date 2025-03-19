using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Quantornion
    {
        private float a;
        private float b;
        private float c;
        private float d;

        public Quantornion(float a, float b, float c, float d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }

        public static Quantornion operator +(Quantornion a, Quantornion b)
        {
            return new Quantornion(a.a + b.a, a.b + b.b, a.c + b.c, a.d + b.d);
        }

        public static Quantornion operator -(Quantornion a, Quantornion b)
        {
            return new Quantornion(a.a - b.a, a.b - b.b, a.c - b.c, a.d - b.d);
        }

        public static Quantornion operator *(Quantornion a, Quantornion b)
        {
            Vector vectorA = new Vector(a.b, a.c, a.d);
            Vector vectorB = new Vector(b.b, b.c, b.d);

            float scalarElement = a.a * b.a - vectorA * vectorB;
            Vector vectorElement = (vectorB * a.a) + (vectorA * b.a) + vectorA.CrossProduct(vectorA, vectorB);

            return new Quantornion(scalarElement, vectorElement.getX(), vectorElement.getY(), vectorElement.getZ());
        }

        public static Quantornion operator /(Quantornion a, Quantornion b)
        {
            Vector vectorA = new Vector(a.b, a.c, a.d);
            Vector vectorB = new Vector(b.b, b.c, b.d);

            float divider = ((float)Math.Pow(b.a, 2) + vectorB * vectorB);
            float scalarElement = ((a.a * b.a + vectorA * vectorB) / divider);
            Vector vectorElement = (((vectorB * -a.a) + (vectorA * b.a) - vectorA.CrossProduct(vectorA, vectorB)) * (1.0f / divider));

            return new Quantornion(scalarElement, vectorElement.getX(), vectorElement.getY(), vectorElement.getZ());
        }

        public static Vector rotate(Vector a, Vector axis, double alpha)
        {
            alpha = alpha * Math.PI / 180;
            Vector unitVector = axis.UnitVector();
            unitVector = unitVector * (float)Math.Sin(alpha / 2);
            Quantornion q = new Quantornion((float)Math.Cos(alpha / 2), unitVector.getX(), unitVector.getY(), unitVector.getZ());
            Quantornion qInverse = new Quantornion((float)Math.Cos(alpha / 2), -unitVector.getX(), -unitVector.getY(), -unitVector.getZ());

            Quantornion result = new Quantornion(0, a.getX(), a.getY(), a.getZ());

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
