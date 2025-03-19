using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class Matrix
    {
        private int sizeX;
        private int sizeY;
        private float[,] content;

        public Matrix(float[,] content)
        {
            this.content = content;
            sizeX = content.GetLength(0);
            sizeY = content.GetLength(1);
        }

        public Matrix(int size)
        {
            sizeX = size;
            sizeY = size;
            content = new float[sizeX, sizeY];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j == i)
                    {
                        content[i, j] = 1;
                    }
                    else
                    {
                        content[i, j] = 0;
                    }
                }
            }
        }


        public static Matrix? operator +(Matrix a, Matrix b)
        {
            if (a.sizeX == b.sizeX && a.sizeY == b.sizeY)
            {
                float[,] added = new float[a.sizeX, b.sizeY];
                for (int i = 0; i < a.sizeX; i++)
                {
                    for (int j = 0; j < b.sizeY; j++)
                    {
                        added[i, j] = a.content[i, j] + b.content[i, j];
                    }
                }

                Matrix c = new Matrix(added);
                return c;
            }
            Console.WriteLine("Warning: Matrices have incompatible sizes for addition.");
            return null;
        }

        public static Matrix? operator -(Matrix a, Matrix b)
        {
            if (a.sizeX == b.sizeX && a.sizeY == b.sizeY)
            {
                float[,] added = new float[a.sizeX, b.sizeY];
                for (int i = 0; i < a.sizeX; i++)
                {
                    for (int j = 0; j < b.sizeY; j++)
                    {
                        added[i, j] = a.content[i, j] - b.content[i, j];
                    }
                }

                Matrix c = new Matrix(added);
                return c;
            }
            Console.WriteLine("Warning: Matrices have incompatible sizes for subtraction.");
            return null;
        }

        public Matrix Transpose()
        {
            Matrix b = new Matrix(this.content);

            for (int i = 0; i < b.sizeX; i++)
            {
                for (int j = i + 1; j < b.sizeY; j++)
                {
                    float tmp = b.content[i, j];
                    b.content[i, j] = b.content[j, i];
                    b.content[j, i] = tmp;
                }
            }
            return b;
        }

        public static Matrix operator *(Matrix a, float scalar)
        {
            Matrix b = new Matrix(a.content);

            for (int i = 0; i < b.sizeX; i++)
            {
                for (int j = 0; j < b.sizeY; j++)
                {
                    b.content[i, j] *= scalar;
                }
            }

            return b;
        }

        public float? determinant()
        {
            if (sizeX == sizeY)
            {
                float determinant = 0.0f;

                switch (sizeX)
                {
                    case (2):
                        {
                            determinant =
                            content[0, 0] * content[1, 1] - content[1, 0] * content[0, 1];
                            break;
                        }
                    case (3):
                        {
                            determinant =
                            content[0, 0] * content[1, 1] * content[2, 2] - content[2, 0] * content[1, 1] * content[0, 2] +
                            content[1, 0] * content[2, 1] * content[0, 2] - content[0, 0] * content[2, 1] * content[1, 2] +
                            content[2, 0] * content[0, 1] * content[1, 2] - content[1, 0] * content[0, 1] * content[2, 2];
                            break;
                        }
                    case (4):
                        {
                            determinant =
                            content[0, 3] * content[1, 2] * content[2, 1] * content[3, 0] - content[0, 2] * content[1, 3] * content[2, 1] * content[3, 0] -
                            content[0, 3] * content[1, 1] * content[2, 2] * content[3, 0] + content[0, 1] * content[1, 3] * content[2, 2] * content[3, 0] +
                            content[0, 2] * content[1, 1] * content[2, 3] * content[3, 0] - content[0, 1] * content[1, 2] * content[2, 3] * content[3, 0] -
                            content[0, 3] * content[1, 2] * content[2, 0] * content[3, 1] + content[0, 2] * content[1, 3] * content[2, 0] * content[3, 1] +
                            content[0, 3] * content[1, 0] * content[2, 2] * content[3, 1] - content[0, 0] * content[1, 3] * content[2, 2] * content[3, 1] -
                            content[0, 2] * content[1, 0] * content[2, 3] * content[3, 1] + content[0, 0] * content[1, 2] * content[2, 3] * content[3, 1] +
                            content[0, 3] * content[1, 1] * content[2, 0] * content[3, 2] - content[0, 1] * content[1, 3] * content[2, 0] * content[3, 2] -
                            content[0, 3] * content[1, 0] * content[2, 1] * content[3, 2] + content[0, 0] * content[1, 3] * content[2, 1] * content[3, 2] +
                            content[0, 1] * content[1, 0] * content[2, 3] * content[3, 2] - content[0, 0] * content[1, 1] * content[2, 3] * content[3, 2] -
                            content[0, 2] * content[1, 1] * content[2, 0] * content[3, 3] + content[0, 1] * content[1, 2] * content[2, 0] * content[3, 3] +
                            content[0, 2] * content[1, 0] * content[2, 1] * content[3, 3] - content[0, 0] * content[1, 2] * content[2, 1] * content[3, 3] -
                            content[0, 1] * content[1, 0] * content[2, 2] * content[3, 3] + content[0, 0] * content[1, 1] * content[2, 2] * content[3, 3];
                            break;
                        }
                }

                return determinant;
            }
            else
            {
                return null;
            }
        }

        public Matrix? Inverse()
        {
            if (sizeX == sizeY)
            {
                float[,] contentOfInverse = new float[sizeX, sizeY];

                float det = (float)determinant();

                if (det == 0)
                {
                    return null;
                }

                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        contentOfInverse[i, j] = (float)Math.Pow(-1, i + 1 + j + 1) * content[sizeX - i - 1, sizeX - j - 1];
                    }
                }

                Matrix inverse = new Matrix(contentOfInverse);

                inverse.Transpose();

                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; j < sizeX; j++)
                    {
                        inverse.content[i, j] /= det;
                    }
                }

                return inverse;
            }
            else
            {
                Console.WriteLine("Warning: Matrix is not squared for inverse.");
                return null;
            }
        }


        public static Matrix? operator *(Matrix a, Matrix b)
        {
            if (a.sizeY == b.sizeX)
            {
                float[,] added = new float[a.sizeX, a.sizeY];
                for (int i = 0; i < a.sizeX; i++)
                {
                    for (int j = 0; j < b.sizeY; j++)
                    {
                        float s = 0.0f;
                        for (int o = 0; o < a.sizeY; o++)
                        {
                            s += a.content[i, o] * b.content[o, j];
                        }
                        added[i, j] = s;
                    }
                }
                Matrix c = new Matrix(added);
                return c;
            }
            Console.WriteLine("Warning: Matrices have incompatible sizes for multiplication.");
            return null;
        }


        public override String ToString()
        {
            String result = "";
            for (int i = 0; i < sizeX; i++)
            {
                result += "[";
                for (int j = 0; j < sizeY; j++)
                {
                    result += content[i, j].ToString();
                    if (j < sizeY - 1)
                    {
                        result += ";";
                    }
                }
                result += "]\n";
            }
            return result;
        }
    }
}
