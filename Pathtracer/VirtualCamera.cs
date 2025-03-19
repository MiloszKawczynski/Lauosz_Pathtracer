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
        public Vector position;
        public Vector front;
        public Vector up;

        public Cube cube;

        public string[,] array = new string[60, 60];

        public VirtualCamera(Vector position, Vector front, Vector up)
        {
            this.position = position;
            this.front = front;
            this.up = up;
        }

        public override String ToString()
        {
            return position.ToString();
        }

        public void moveCamera()
        {
            int option = 1;
            while (option != 101)
            {
                option = Console.Read();
                switch (option)
                {
                    case 119:   //w
                        position.setVector(position.getX(), position.getY(), position.getZ() + 0.5f);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 115:   //s
                        position.setVector(position.getX(), position.getY(), position.getZ() - 0.5f);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 97:    //a
                        position.setVector(position.getX() - 0.5f, position.getY(), position.getZ());
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 100:   //d
                        position.setVector(position.getX() + 0.5f, position.getY(), position.getZ());
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 117:   //u
                        position.setVector(position.getX(), position.getY() + 0.5f, position.getZ());
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 105:   //i
                        position.setVector(position.getX(), position.getY() - 0.5f, position.getZ());
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    default:
                        break;
                }
            }
        }

        public void rotateCamera()
        {
            int option = 1;
            while (option != 101)
            {
                option = Console.Read();
                switch (option)
                {
                    case 119:   //w
                        position = Quantornion.rotate(position, new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        up = Quantornion.rotate(up, new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        front = Quantornion.rotate(front, new Vector(1.0f, 0.0f, 0.0f), 35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 115:   //s
                        position = Quantornion.rotate(position, new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        up = Quantornion.rotate(up, new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        front = Quantornion.rotate(front, new Vector(1.0f, 0.0f, 0.0f), -35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 97:    //a
                        position = Quantornion.rotate(position, new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        up = Quantornion.rotate(up, new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        front = Quantornion.rotate(front, new Vector(0.0f, 0.0f, 1.0f), -35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 100:   //d
                        position = Quantornion.rotate(position, new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        up = Quantornion.rotate(up, new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        front = Quantornion.rotate(front, new Vector(0.0f, 0.0f, 1.0f), 35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 117:   //u
                        position = Quantornion.rotate(position, new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        up = Quantornion.rotate(up, new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        front = Quantornion.rotate(front, new Vector(0.0f, 1.0f, 0.0f), 35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    case 105:   //i
                        position = Quantornion.rotate(position, new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        up = Quantornion.rotate(up, new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        front = Quantornion.rotate(front, new Vector(0.0f, 1.0f, 0.0f), -35.0);
                        Console.WriteLine(ToString());
                        draw();
                        break;
                    default:
                        break;
                }
            }
        }

        public void draw()
        {
            for (int i = 0; i < 60; i++)
            {
                for (int j = 0; j < 60; j++)
                {
                    Vector vector = front;

                    float fov = 0.0001f;
                    //vector = vector - new Vector(1.0f, 0.0f, 0.0f) * (i - 30);
                    vector = Quantornion.rotate(vector, up.CrossProduct(up, front), (i - 30) * fov);
                    vector = Quantornion.rotate(vector, up, (j - 30) * fov);

                    //vecotr = 

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
