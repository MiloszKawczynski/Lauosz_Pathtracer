namespace Pathtracer
{
    internal class LightIntensity : Vector
    {
        public float R => X;
        public float G => Y;
        public float B => Z;

        public LightIntensity(float r, float g, float b) : base(Math.Clamp(r, 0f, 1f), Math.Clamp(g, 0f, 1f), Math.Clamp(b, 0f, 1f)) { }
    }
}
