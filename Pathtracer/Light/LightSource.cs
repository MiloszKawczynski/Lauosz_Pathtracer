namespace Pathtracer.Light
{
    abstract class LightSource
    {
        public LightIntensity LightIntensity { get; set; }
        public Point Position { get; set; }

        public abstract Vector GetDirectionFrom(Point hitPoint);
        public abstract float GetDistanceFrom(Point hitPoint);

        protected LightSource(LightIntensity lightColor, Point position)
        {
            LightIntensity = lightColor;
            Position = position;
        }
    }
}
