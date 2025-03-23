using System;
using Pathtracer;

class Program
{
    static void Main(string[] args)
    {
        // Vector test
        // Przemienność dodawania
        var vectorA = new Vector(1, 0, 0);
        var vectorB = new Vector(3, 2, 1);
        Console.WriteLine($"\nDodawanie wektorów: {vectorA} i {vectorB} : {vectorA + vectorB}");
        Console.WriteLine($"\nDodawanie wektorów: {vectorB} i {vectorA} : {vectorB + vectorA}");

        // Kąt między wektorami
        vectorA = new Vector(0, 3, 0);
        vectorB = new Vector(5, 5, 0);
        var angleInRads = AngleBetween.Angle(vectorA, vectorB);
        Console.WriteLine($"\nKąt między wektorami {vectorA} i {vectorB} : {angleInRads} ({angleInRads * 180 / Math.PI})");

        // Wektor prostopadły (cross product)
        vectorA = new Vector(4, 5, 1);
        vectorB = new Vector(4, 1, 3);
        var vectorPerpendicular = Vector.CrossProduct(vectorA, vectorB);
        Console.WriteLine($"\nWektor prostopadły do {vectorA} i {vectorB} : {vectorPerpendicular}");

        // Znormalizowany wektor prostopadły
        Console.WriteLine($"\nWektor prostopadły znormalizowany {vectorPerpendicular.UnitVector()}");


        // Sphere test
        // Skierowany w środek kuli - kierunek = (0, 0, 0) - (0, 0, -20) = (0, 0, 20)
        var S = new Sphere(new Point(0, 0, 0), 10);
        var R1 = new Ray(new Point(0, 0, -20), new Vector(0, 0, 20));
        var R2 = new Ray(new Point(0, 0, -20), new Vector(0, 1, 0));
        Console.WriteLine($"\nPrzecięcie promienia {R1} ze sferą {S}");
        IntersectWith.IntersectionLineSphere(R1, S);
        Console.WriteLine($"\nPrzecięcie promienia {R2} ze sferą {S}");
        IntersectWith.IntersectionLineSphere(R2, S);

        // Promień styczny do sfery - początek poza sferą, delta == 0
        var R3 = new Ray(new Point(-50, -10, 0), new Vector(50, 0, 0));
        Console.WriteLine($"\nPrzecięcie promienia {R3} ze sferą {S}");
        IntersectWith.IntersectionLineSphere(R3, S);


        // Plane test
        var P = new Plane(new Point(0, 0, 0), new Vector(0, (float)Math.Sqrt(2) / 2, (float)Math.Sqrt(2) / 2));
        Console.WriteLine($"\nPrzecięcie promienia {R2} z płaszczyzną {P}");
        IntersectWith.IntersectionLinePlane(R2, P);

        // Triangle test
        var triangle = new Triangle(new Point(0, 0, 0), new Point(1, 0, 0), new Point(0, 1, 0));
        var line = new Line(new Point(-1, 0.5f, 0), new Point(1, 0.5f, 0));
        IntersectWith.IntersectionLineTriangle(line, triangle);
        line = new Line(new Point(2, -1, 0), new Point(2, 2, 0));
        IntersectWith.IntersectionLineTriangle(line, triangle);
        line = new Line(new Point(0, 0, -1), new Point(0, 0, 1));
        IntersectWith.IntersectionLineTriangle(line, triangle);

        // Additional
        //Matrix tests
        var matrixAContent = new float[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
        var matrixBContent = new float[3, 3] { { 9, 8, 7 }, { 6, 5, 4 }, { 3, 2, 1 } };
        var matrixA = new Matrix(matrixAContent);
        var matrixB = new Matrix(matrixBContent);
        Console.WriteLine($"\nDodawanie macierzy {matrixA} do {matrixB}");
        Console.WriteLine(matrixA + matrixB);
        Console.WriteLine($"Odejmowanie macierzy {matrixB} od {matrixA}");
        Console.WriteLine(matrixA - matrixB);
        Console.WriteLine($"Mnożenie macierzy {matrixA} przez skalar 2.5");
        Console.WriteLine(matrixA * 2.5f);
        Console.WriteLine($"Macierz {matrixA} transponowana");
        Console.WriteLine(matrixA.Transpose());
        matrixA = new Matrix(new float[3, 3] { { 1, 5, 2 }, { 5, 1, 2 }, { 2, 2, 2 } });
        Console.WriteLine($"Macierz {matrixA} odwrotna");
        Console.WriteLine(matrixA.Inverse());
        Console.WriteLine($"Macierz jednostkowa 4x4");
        Console.WriteLine(matrixA.IdentityMatrix(4));

        // Vector test
        Console.WriteLine($"Obrót wektora {vectorA} o 90 stopni wokół osi Y");
        Console.WriteLine(vectorA.Rotate(new Vector(0, 1, 0), 90));

        // Matrix multiply test
        matrixA = new Matrix(matrixAContent);
        Console.WriteLine($"\nMnożenie macierzy {matrixA} przez {matrixB}");
        Console.WriteLine(matrixA * matrixB);
        Console.WriteLine($"Mnożenie macierzy {matrixB} przez {matrixA}");
        Console.WriteLine(matrixB * matrixA);

        // Vector rotation test
        vectorA = new Vector(3,1,3);
        vectorB = new Vector(1,0,1);
        Console.WriteLine($"Obrót wektora {vectorA} wokół {vectorB} o 90 stopni");
        Console.WriteLine(vectorA.Rotate(vectorB, 90));

        // Quaternion test
        var quaternion = Quaternion.FromAxisAngle(30, new Vector(1, 0, 0));
        Console.WriteLine($"\nWielkość kwaternionu {quaternion} to {quaternion.Norm()}");
        Console.WriteLine($"\nOdwrotność kwaternionu {quaternion} to {quaternion.Inverse()}");
        Console.WriteLine($"\nKwaternion transformujący obrót lokalny na globalny jest odwrotnością kwaternionu obrotu obiektu. Obiekt ma więc orientację" +
            $"{quaternion.Inverse().Eulers()}");

        quaternion = new Quaternion(0.233f, new Vector(0.060f, -0.257f, -0.935f));
        var quaternionB = new Quaternion(-0.752f, new Vector(0.286f, 0.374f, 0.459f));
        Console.WriteLine($"\nIloczyn skalarny kwaternionów {quaternion} i {quaternionB}");
        Console.WriteLine(Quaternion.DotProduct(quaternion, quaternionB));
        Console.WriteLine($"\nIloczyn kwaternionowy kwaternionów {quaternion} i {quaternionB}");
        Console.WriteLine(quaternion * quaternionB);
        Console.WriteLine($"\nRóżnica między kwaternionami {quaternion} i {quaternionB}");
        Console.WriteLine(quaternion - quaternionB);
    }
}