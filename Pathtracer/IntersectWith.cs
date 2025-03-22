using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class IntersectWith
    {
        private float CalculateCoefficientDefiningPotentialIntersection(Line a, Line b)
        {
            float t;

            Vector nominator1;
            Vector nominator2;
            float denominator;

            nominator1 = b.GetPoint() - a.GetPoint();
            nominator1 = Vector.CrossProduct(nominator1, b.GetVector());

            nominator2 = Vector.CrossProduct(a.GetVector(), b.GetVector());

            t = nominator1 * nominator2;

            denominator = (float)Math.Pow((double)nominator2.Length(), 2);

            t = t / denominator;

            return t;
        }

        private bool IsCoefficientValid(float t, Line a)
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

        public Point? Intersetion(Line a, Line b)
        {
            float t1 = CalculateCoefficientDefiningPotentialIntersection(a, b);
            float t2 = CalculateCoefficientDefiningPotentialIntersection(b, a);

            Point Pa = (Point)(a.GetPoint() + a.GetVector() * t1);
            Point Pb = (Point)(b.GetPoint() + b.GetVector() * t2);

            if (Pa == Pb && IsCoefficientValid(t1, a) && IsCoefficientValid(t2, b))
            {
                Console.WriteLine("Przecięcie w punkcie: " + Pa);
                return Pa;
            }
            else
            {
                Console.WriteLine("Brak przecięcia");
                return null;
            }
        }
    }
}
