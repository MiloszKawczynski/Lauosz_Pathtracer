namespace Pathtracer.Light
{
    internal class PointLight : LightSource
    {
       
        public PointLight(LightIntensity lightColor, Vector position) : base(lightColor, position) 
        {
        
        }

        public override Vector GetDirectionFrom(Point hitPoint)
        => (Position - hitPoint).UnitVector();

        public override float GetDistanceFrom(Point hitPoint)
            => (Position - hitPoint).Length();
    }
}
