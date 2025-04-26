namespace Pathtracer
{
    internal class Material
    {
        public float Kd = 0.5f;
        public float Ks = 0.3f;
        public float Ka = 0.3f;
        public float n = 1f;

        public bool isReflective = false;
        public bool isRefractive = false;
        public float indexOfRefraction = 1.5f;

        public float metallic;
        public float roughness;
        public float ao;
        public Vector albedo = new Vector(1.0f, 1.0f, 1.0f);

        public Material(float Kd, float Ks, float Ka, float n)
        {
            this.Kd = Kd;
            this.Ks = Ks;
            this.Ka = Ka;
            this.n = n;
        }

        public Material(float o, float r, float m, Vector albedo)
        {
            ao = o;
            this.albedo = albedo;
            roughness = r;
            metallic = m;
        }

        public Material() { }
    }
}
