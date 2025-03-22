namespace Pathtracer
{
    internal class Vector
    {
        private float x;
        private float y;
        private float z;

        public float X => x;
        public float Y => y;
        public float Z => z;

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

        public static bool operator ==(Vector a, Vector b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public float Length()
        {
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
        }

        public static float operator *(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vector CrossProduct(Vector a, Vector b)
        {
            float x, y, z;
            x = a.y * b.z - a.z * b.y;
            y = a.z * b.x - a.x * b.z;
            z = a.x * b.y - a.y * b.x;
            return new Vector(x, y, z);
        }

        public Vector UnitVector()
        {
            float length = Length();
            return new Vector(x / length, y / length, z / length);
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

        public void Set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector Invert()
        {
            return new Vector(-x, -y, -z);
        }

        public Vector Rotate(Vector axis, double alpha)
        {
            alpha = alpha * Math.PI / 180;
            Vector unitVector = axis.UnitVector();
            unitVector = unitVector * (float)Math.Sin(alpha / 2);
            Quaternion q = new Quaternion((float)Math.Cos(alpha / 2), unitVector);
            Quaternion qInverse = new Quaternion((float)Math.Cos(alpha / 2), unitVector * -1);

            Quaternion result = new Quaternion(0, this);

            result = q * result * qInverse;

            return result.V;
        }
    }
}
