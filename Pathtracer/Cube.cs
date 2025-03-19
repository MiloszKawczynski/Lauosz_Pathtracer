using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Cube
    {
        private Plane[] planes;
        private Vector[] vertices;

        public Cube(Plane[] plane, Vector[] vertices)
        {
            this.planes = plane;
            this.vertices = vertices;
        }

        public bool IntesectWithLine(Line line, VirtualCamera camera)
        {
            for (int i = 0; i < planes.Length; i++)
            {
                Vector point = (planes[i].IntersetionWithLine(line));

                if (
                    point.GetX() >= -1 && point.GetX() <= 1 &&
                    point.GetY() >= -1 && point.GetY() <= 1 &&
                    point.GetZ() >= -1 && point.GetZ() <= 1
                    )
                {
                    Vector direction = (point - camera.position).Sign();

                    if (direction == line.v.Sign())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
