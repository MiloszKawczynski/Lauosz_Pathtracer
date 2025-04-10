namespace Pathtracer
{
    internal class LightIntensity : Vector
    {
        public float R => X;
        public float G => Y;
        public float B => Z;

        public LightIntensity(float r, float g, float b) : base(Math.Clamp(r, 0f, 1f), Math.Clamp(g, 0f, 1f), Math.Clamp(b, 0f, 1f)) { }
        public LightIntensity(Vector v) : base(v.X, v.Y, v.Z) { }

        public float GetBrightness()
        {
            return (R + B + G) / 3.0f;
        }

        public static LightIntensity? operator +(LightIntensity a, LightIntensity b)
        {
            return new LightIntensity(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static LightIntensity operator *(LightIntensity a, float s)
        {
            return new LightIntensity(a.X * s, a.Y * s, a.Z * s);
        }

        public static LightIntensity operator *(float s, LightIntensity a)
        {
            return new LightIntensity(a.X * s, a.Y * s, a.Z * s);
        }
    }
}
