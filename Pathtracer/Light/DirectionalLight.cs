using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer.Light
{
    internal class DirectionalLight : LightSource
    {
        public Vector Direction;
        public DirectionalLight(LightIntensity lightColor, Vector direction, Point position) : base(lightColor, position)
        {
            Direction = direction;
        }

        public override Vector GetDirectionFrom(Point hitPoint) => Direction.UnitVector();

        public override float GetDistanceFrom(Point hitPoint) => float.MaxValue;
    }
}
