using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    enum ProjectionType
    {
        Ortogonal,
        Perspective
    }
    internal class VirtualCamera
    {
        public Point position;
        public Vector front;
        public Vector up;

        private ProjectionType projection = ProjectionType.Perspective;

        public VirtualCamera(Point position, Vector front, Vector up)
        {
            this.position = position;
            this.front = front;
            this.up = up;
        }

        public override String ToString()
        {
            return position.ToString();
        }

        public void Move()
        {
            int option = 1;
            while (option != 101)
            {
                option = Console.Read();
                switch (option)
                {
                    case 119:   //w
                        position.Set(position.X, position.Y, position.Z + 0.5f);
                        Console.WriteLine(ToString());
                        break;
                    case 115:   //s
                        position.Set(position.X, position.Y, position.Z - 0.5f);
                        Console.WriteLine(ToString());
                        break;
                    case 97:    //a
                        position.Set(position.X - 0.5f, position.Y, position.Z);
                        Console.WriteLine(ToString());
                        break;
                    case 100:   //d
                        position.Set(position.X + 0.5f, position.Y, position.Z);
                        Console.WriteLine(ToString());
                        break;
                    case 117:   //u
                        position.Set(position.X, position.Y + 0.5f, position.Z);
                        Console.WriteLine(ToString());
                        break;
                    case 105:   //i
                        position.Set(position.X, position.Y - 0.5f, position.Z);
                        Console.WriteLine(ToString());
                        break;
                    default:
                        break;
                }
            }
        }

        public void Rotate()
        {
            int option = 1;
            while (option != 101)
            {
                option = Console.Read();
                switch (option)
                {
                    case 119:   //w
                        position = (Point)position.Rotate(new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        up = up.Rotate(new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        front = front.Rotate(new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        Console.WriteLine(ToString());
                        break;
                    case 115:   //s
                        position = (Point)position.Rotate(new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        up = up.Rotate(new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        front = front.Rotate(new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        Console.WriteLine(ToString());
                        break;
                    case 97:    //a
                        position = (Point)position.Rotate(new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        up = up.Rotate(new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        front = front.Rotate(new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        Console.WriteLine(ToString());
                        break;
                    case 100:   //d
                        position = (Point)position.Rotate(new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        up = up.Rotate(new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        front = front.Rotate(new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        Console.WriteLine(ToString());
                        break;
                    case 117:   //u
                        position = (Point)position.Rotate(new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        up = up.Rotate(new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        front = front.Rotate(new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        Console.WriteLine(ToString());
                        break;
                    case 105:   //i
                        position = (Point)position.Rotate(new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        up = up.Rotate(new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        front = front.Rotate(new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        Console.WriteLine(ToString());
                        break;
                    default:
                        break;
                }
            }
        }
    }
