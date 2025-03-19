using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Vector
    {
        private float x;
        private float y;
        private float z;

        public Vector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector operator *(Vector a, float s)
        {
            return new Vector(a.x * s, a.y * s, a.z * s);
        }
        public static Vector operator /(Vector a, float s)
        {
            return new Vector(a.x / s, a.y / s, a.z / s);
        }

        public bool isEqual(Vector a, Vector b)
        {
            if(a.getX()==b.getX() && a.getY() == b.getY() && a.getZ() == b.getZ())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float Length(Vector a)
        {
            return (float)Math.Sqrt(Math.Pow(a.x, 2) + Math.Pow(a.y, 2) + Math.Pow(a.z, 2));
        }

        public float DotProduct(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public float AngleRad(Vector a, Vector b)
        {
            return (float)Math.Acos(DotProduct(a, b) / (Length(a) * Length(b)));
        }

        public float AngleDeg(Vector a, Vector b)
        {
            return (float)(Math.Acos(DotProduct(a, b) / (Length(a) * Length(b))) * 180.0f / Math.PI);
        }

        public Vector CrossProduct(Vector a, Vector b)
        {
            float x, y, z;
            x = a.y * b.z - a.z * b.y;
            y = a.z * b.x - a.x * b.z;
            z = a.x * b.y - a.y * b.x;
            return new Vector(x, y, z);
        }

        public Vector UnitVector(Vector a)
        {
            float length = Length(a);
            return new Vector(a.x / length, a.y / length, a.z / length);
        }

        public Vector Sign()
        {
            return new Vector(Math.Sign(x), Math.Sign(y), Math.Sign(z));
        }

        public override String ToString()
        {
            String result = "[" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + "]";
            return result;
        }

        public void setVector(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float getX()
        {
            return x;
        }

        public float getY()
        {
            return y;
        }

        public float getZ()
        {
            return z;
        }

        public Vector invert()
        {
            return new Vector(-x, -y, -z);
        }

        //public void round()
        //{
        //    x = (float)Math.Round(x,1);
        //    y = (float)Math.Round(y,1);
        //    z = (float)Math.Round(z,1);
        //}
    }
}
