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

        //public bool IntesectWithLine(Line line, VirtualCamera camera)
        //{
        //    for (int i = 0; i < planes.Length; i++)
        //    {
        //        Vector point = (planes[i].IntersetionWithLine(line));

        //        if (
        //            point.X >= -1 && point.X <= 1 &&
        //            point.Y >= -1 && point.Y <= 1 &&
        //            point.Z >= -1 && point.Z <= 1
        //            )
        //        {
        //            Vector direction = (point - camera.position).Sign();

        //            if (direction == line.v.Sign())
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}
