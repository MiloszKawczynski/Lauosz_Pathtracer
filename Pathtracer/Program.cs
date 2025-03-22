using Pathtracer;

class Program
{
    static void Main(string[] args)
    {
        // Vector test
        // Przemienność dodawania
        var vectorA = new Vector(1, 0, 0);
        var vectorB = new Vector(3, 2, 1);
        Console.WriteLine($"Dodawanie wektorów: {vectorA} i {vectorB} : {vectorA + vectorB}");
        Console.WriteLine($"Dodawanie wektorów: {vectorB} i {vectorA} : {vectorB + vectorA}");

        // Kąt między wektorami
        vectorA = new Vector(0, 3, 0);
        vectorB = new Vector(5, 5, 0);
        var angleInRads = AngleBetween.Angle(vectorA, vectorB);
        Console.WriteLine($"Kąt między wektorami {vectorA} i {vectorB} : {angleInRads} ({angleInRads * 180 / Math.PI})");

        // Wektor prostopadły (cross product)
        vectorA = new Vector(4, 5, 1);
        vectorB = new Vector(4, 1, 3);
        var vectorPerpendicular = Vector.CrossProduct(vectorA, vectorB);
        Console.WriteLine($"Wektor prostopadły do {vectorA} i {vectorB} : {vectorPerpendicular}");

        // Znormalizowany wektor prostopadły
        Console.WriteLine($"Wektor prostopadły znormalizowany {vectorPerpendicular.UnitVector()}");


        // Sphere test
        // Skierowany w środek kuli - kierunek = (0, 0, 0) - (0, 0, -20) = (0, 0, 20)
        var S = new Sphere(new Point(0, 0, 0), 10);
        var R1 = new Ray(new Point(0, 0, -20), new Vector(0, 0, 20));
        var R2 = new Ray(new Point(0, 0, -20), new Vector(0, 1, 0));
        Console.WriteLine($"Przecięcie promienia {R1} ze sferą {S}");
        IntersectWith.IntersectionLineSphere(R1, S);
        Console.WriteLine($"Przecięcie promienia {R2} ze sferą {S}");
        IntersectWith.IntersectionLineSphere(R2, S);

        // Promień styczny do sfery - początek poza sferą, delta == 0
        var R3 = new Ray(new Point(-50, -10, 0), new Vector(50, 0, 0));
        Console.WriteLine($"Przecięcie promienia {R3} ze sferą {S}");
        IntersectWith.IntersectionLineSphere(R3, S);


        // Plane test
        var P = new Plane(new Point(0, 0, 0), new Vector(0, (float)Math.Sqrt(2) / 2, (float)Math.Sqrt(2) / 2));
        Console.WriteLine($"Przecięcie promienia {R2} z płaszczyzną {P}");
        IntersectWith.IntersectionLinePlane(R2, P);

        // Triangle test
        var triangle = new Triangle(new Point(0, 0, 0), new Point(1, 0, 0), new Point(0, 1, 0));
        var line = new Line(new Point(-1, 0.5f, 0), new Point(1, 0.5f, 0));
        IntersectWith.IntersectionLineTriangle(line, triangle);
        line = new Line(new Point(2, -1, 0), new Point(2, 2, 0));
        IntersectWith.IntersectionLineTriangle(line, triangle);
        line = new Line(new Point(0, 0, -1), new Point(0, 0, 1));
        IntersectWith.IntersectionLineTriangle(line, triangle);

        //var matrixAContent = new float[3, 3] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
        //var matrixBContent = new float[3, 3] { { 9, 8, 7 }, { 6, 5, 4 }, { 3, 2, 1 } };
        //var matrixA = new Matrix(matrixAContent);
        //var matrixB = new Matrix(matrixBContent);

        //Console.WriteLine(matrixA - matrixB);
    }
}