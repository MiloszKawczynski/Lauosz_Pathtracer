namespace Pathtracer.Light
{
    internal class PointLight : LightSource
    {

        public PointLight(LightIntensity lightColor, Point position) : base(lightColor, position)
        {

        }

        public override Vector GetDirectionFrom(Point hitPoint)
        => (Position - hitPoint).Normalize();

        public override float GetDistanceFrom(Point hitPoint)
            => (Position - hitPoint).Length();
    }
}
