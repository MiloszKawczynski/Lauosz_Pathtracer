namespace Pathtracer
{
    internal class Point : Vector
    {
        public Point(float x, float y, float z) : base(x, y, z) { }

        public static Point operator +(Point p, Point v)
        {
            return new Point(p.X + v.X, p.Y + v.Y, p.Z + v.Z);
        }

        public static Point operator *(Point a, float s)
        {
            return new Point(a.X * s, a.Y * s, a.Z * s);
        }
    }
}
