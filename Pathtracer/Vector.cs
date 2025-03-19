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

        public static bool operator ==(Vector a, Vector b)
        {
            return a.GetX() == b.GetX() && a.GetY() == b.GetY() && a.GetZ() == b.GetZ();
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public float Length()
        {
            return (float)Math.Sqrt(Math.Pow(this.x, 2) + Math.Pow(this.y, 2) + Math.Pow(this.z, 2));
        }

        public static float operator *(Vector a, Vector b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public float AngleRad(Vector a, Vector b)
        {
            return (float)Math.Acos((a * b) / (a.Length() * b.Length()));
        }

        public float AngleDeg(Vector a, Vector b)
        {
            return (float)(Math.Acos((a * b) / (a.Length() * b.Length())) * 180.0f / Math.PI);
        }

        public Vector CrossProduct(Vector a, Vector b)
        {
            float x, y, z;
            x = a.y * b.z - a.z * b.y;
            y = a.z * b.x - a.x * b.z;
            z = a.x * b.y - a.y * b.x;
            return new Vector(x, y, z);
        }

        public Vector UnitVector()
        {
            float length = this.Length();
            return new Vector(this.x / length, this.y / length, this.z / length);
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

        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }

        public float GetZ()
        {
            return z;
        }

        public Vector Invert()
        {
            return new Vector(-x, -y, -z);
        }
    }
}
