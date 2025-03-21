using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class VirtualCamera
    {
        public Point position;
        public Vector front;
        public Vector up;

        public Cube cube;

        public string[,] array = new string[60, 60];

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
                        position.Set(position.GetX(), position.GetY(), position.GetZ() + 0.5f);
                        Console.WriteLine(ToString());
                        Draw();
                        break;
                    case 115:   //s
                        position.Set(position.GetX(), position.GetY(), position.GetZ() - 0.5f);
                        Console.WriteLine(ToString());
                        Draw();
                        break;
                    case 97:    //a
                        position.Set(position.GetX() - 0.5f, position.GetY(), position.GetZ());
                        Console.WriteLine(ToString());
                        Draw();
                        break;
                    case 100:   //d
                        position.Set(position.GetX() + 0.5f, position.GetY(), position.GetZ());
                        Console.WriteLine(ToString());
                        Draw();
                        break;
                    case 117:   //u
                        position.Set(position.GetX(), position.GetY() + 0.5f, position.GetZ());
                        Console.WriteLine(ToString());
                        Draw();
                        break;
                    case 105:   //i
                        position.Set(position.GetX(), position.GetY() - 0.5f, position.GetZ());
                        Console.WriteLine(ToString());
                        Draw();
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
                        position = Quaternion.Rotate(position, new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        up = Quaternion.Rotate(up, new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        front = Quaternion.Rotate(front, new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 115:   //s
                        position = Quaternion.Rotate(position, new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        up = Quaternion.Rotate(up, new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        front = Quaternion.Rotate(front, new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 97:    //a
                        position = Quaternion.Rotate(position, new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        up = Quaternion.Rotate(up, new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        front = Quaternion.Rotate(front, new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 100:   //d
                        position = Quaternion.Rotate(position, new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        up = Quaternion.Rotate(up, new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        front = Quaternion.Rotate(front, new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 117:   //u
                        position = Quaternion.Rotate(position, new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        up = Quaternion.Rotate(up, new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        front = Quaternion.Rotate(front, new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 105:   //i
                        position = Quaternion.Rotate(position, new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        up = Quaternion.Rotate(up, new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        front = Quaternion.Rotate(front, new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    default:
                        break;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    Vector vector = front;

                    float fov = 0.0001f;

                    vector = vector.Rotate(Vector.CrossProduct(up, front), (i - 30) * fov);
                    vector = vector.Rotate(up, (j - 30) * fov);

                    if (cube.IntesectWithLine(new Line(position, vector), this))
                    {
                        array[i, j] = "0";
                    }
                    else
                    {
                        array[i, j] = ".";
                    }
                }
            }

            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

    }

}
