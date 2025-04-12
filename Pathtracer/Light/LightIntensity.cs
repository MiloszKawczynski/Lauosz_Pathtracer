namespace Pathtracer
{
    internal class LightIntensity : Vector
    {
        public float R => X;
        public float G => Y;
        public float B => Z;

        public LightIntensity(float r, float g, float b) : base(r, g, b) { }
        public LightIntensity(Vector v) : base(v.X, v.Y, v.Z) { }

        public float GetBrightness()
        {
            return (R + B + G) / 3.0f;
        }

        public LightIntensity Clamp()
        {
            return new LightIntensity(Math.Clamp(R, 0.0f, 1.0f), Math.Clamp(G, 0.0f, 1.0f), Math.Clamp(B, 0.0f, 1.0f));
        }

        public static LightIntensity? operator +(LightIntensity a, LightIntensity b)
        {
            return new LightIntensity(a.R + b.R, a.G + b.G, a.B + b.B);
        }

        public static LightIntensity operator *(LightIntensity a, float s)
        {
            return new LightIntensity(a.R * s, a.G * s, a.B * s);
        }

        public static LightIntensity operator *(float s, LightIntensity a)
        {
            return new LightIntensity(a.R * s, a.G * s, a.B * s);
        }

        public static LightIntensity operator *(LightIntensity a, LightIntensity b)
        {
            return new LightIntensity(a.R * b.R, a.G * b.G, a.B * b.B);
        }
    }
}
