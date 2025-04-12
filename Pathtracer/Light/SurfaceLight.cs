namespace Pathtracer.Light
{
    internal class SurfaceLight : LightSource
    {

        private List<PointLight> _pointLights = new();

        public List<PointLight> PointLights => _pointLights;

        public SurfaceLight(LightIntensity lightColor, Point position, int width, int height, int numberOfLights) : base(lightColor, position)
        {
            
            var aspect_ratio = width / height;
            var cols = Math.Round(Math.Sqrt(numberOfLights * aspect_ratio));
            var rows = Math.Ceiling(numberOfLights / cols);
            if ((cols / rows) < aspect_ratio)
            { 
                cols = Math.Ceiling(Math.Sqrt(numberOfLights * aspect_ratio));
                rows = Math.Ceiling(numberOfLights / cols);
            }
            else
            {
                rows = Math.Ceiling(Math.Sqrt(numberOfLights / aspect_ratio));
                cols = Math.Ceiling(numberOfLights / rows);
            }

            var xSpacing = (float)(width / cols);
            var ySpacing = (float)(height / cols);
            var startX = position.X - width / 2f;
            var startY = position.Y - height / 2f;

            for (var i = 0; i < numberOfLights; i++)
            {
                PointLights.Add(new PointLight(lightColor, new Point(startX + i * xSpacing, startY + i * ySpacing, position.Z)));
            }

        }

        public override Vector GetDirectionFrom(Point hitPoint)
        {
            Vector avgDir = new(0, 0, 0);
            foreach (var lightPos in _pointLights)
            { 
                avgDir += (lightPos.Position - hitPoint).UnitVector();
            }
            return avgDir.UnitVector();
        }

        public override float GetDistanceFrom(Point hitPoint)
        {
            float minDist = float.MaxValue;
            foreach (var lightPos in _pointLights)
            {
                float dist = (lightPos.Position - hitPoint).Length();
                if (dist < minDist) 
                { 
                    minDist = dist; 
                }
            }
            return minDist;
        }
    }
}
