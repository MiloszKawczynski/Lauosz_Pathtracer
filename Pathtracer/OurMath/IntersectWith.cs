using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathtracer.Primitives;

namespace Pathtracer
{
    internal class IntersectWith
    {
        private static float CalculateCoefficientDefiningPotentialIntersection(Line a, Line b)
        {
            float t;

            Vector nominator1;
            Vector nominator2;
            float denominator;

            nominator1 = b.P - a.P;
            nominator1 = Vector.CrossProduct(nominator1, b.V);

            nominator2 = Vector.CrossProduct(a.V, b.V);

            t = nominator1 * nominator2;

            denominator = (float)Math.Pow(nominator2.Length(), 2);

            t = t / denominator;

            return t;
        }

        private static bool IsCoefficientValid(float t, Line a)
        {
            if (a is Section)
            {
                return t >= 0 && t <= 1;
            }
            else if (a is Ray)
            {
                return t >= 0;
            }
            return true;
        }

        public static Point? IntersectionLines(Line a, Line b)
        {
            float t1 = CalculateCoefficientDefiningPotentialIntersection(a, b);
            float t2 = CalculateCoefficientDefiningPotentialIntersection(b, a);

            Point Pa = (Point)(a.P + a.V * t1);
            Point Pb = (Point)(b.P + b.V * t2);

            if (Pa == Pb && IsCoefficientValid(t1, a) && IsCoefficientValid(t2, b))
            {
                return Pa;
            }
            else
            {
                return null;
            }
        }

        public static List<Point>? IntersectionLineSphere(Line line, Sphere sphere)
        {
            List<Point> result = new List<Point>();

            float a = (float)Math.Pow(line.V.Length(), 2);
            float b = 2 * (line.V * (line.P - sphere.c));
            float c = (float)Math.Pow((line.P - sphere.c).Length(), 2) - (float)Math.Pow(sphere.r, 2);

            float word = (float)Math.Sqrt(Math.Pow(b, 2) - (4 * a * c));

            if (word >= 0)
            {
                float t1 = (-b - word) / (2 * a);
                float t2 = (-b + word) / (2 * a);

                if (IsCoefficientValid(t1, line))
                {
                    var vectorOfPoint = line.P + line.V * t1;
                    Point Pa = new(vectorOfPoint.X, vectorOfPoint.Y, vectorOfPoint.Z);
                    result.Add(Pa);
                }


                if (t1 != t2)
                {
                    if (IsCoefficientValid(t2, line))
                    {
                        var vectorOfPoint = line.P + line.V * t2;
                        Point Pb = new(vectorOfPoint.X, vectorOfPoint.Y, vectorOfPoint.Z);
                        result.Add(Pb);
                    }
                }
            }
            else
            {
                return null;
            }

            return result;
        }

        public static Line IntersectionPlanes(Plane a, Plane b)
        {
            Vector lineVector = Vector.CrossProduct(b.N, a.N);

            return new Line(b.P, lineVector);
        }

        public static Point IntersectionLineTriangle(Line line, Triangle triangle)
        {
            Point? intersection = IntersectionLinePlane(line, triangle);
            if (intersection is null)
            {
                return null;
            }

            Point p = intersection;

            Vector v0 = triangle.P2 - triangle.P1;
            Vector v1 = triangle.P3 - triangle.P1;
            Vector v2 = p - triangle.P1;

            float dot00 = v0 * v0;
            float dot01 = v0 * v1;
            float dot02 = v0 * v2;
            float dot11 = v1 * v1;
            float dot12 = v1 * v2;

            float invDenom = 1f / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            if (u >= 0 && v >= 0 && u + v <= 1)
            {
                return p;
            }

            return null;
        }


        public static Point? IntersectionLinePlane(Line a, Plane b)
        {
            float nominator;
            float denominator;

            nominator = b.N * (b.P - a.P);
            denominator = b.N * a.V;

            if (Math.Abs(denominator) < 0.0001f)
            {
                return null;
            }

            float t = nominator / denominator;

            if (IsCoefficientValid(t, a))
            {
                var point = a.GetPointAt(t);
                return point;
            }
            else
            {
                return null;
            }
        }
    }

}
