namespace Pathtracer.Primitives
{
    internal class Primitive
    {
        public LightIntensity color = new(1.0f, 0.0f, 0.0f);
        public Material material = new();
    }
}