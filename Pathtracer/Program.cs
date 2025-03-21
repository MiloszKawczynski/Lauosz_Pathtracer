using Pathtracer;

Vector[] vertices = { 
    new Vector(-1.0f, -1.0f, -1.0f),    //left down near
    new Vector(1.0f, -1.0f, -1.0f),     //right down near
    new Vector(-1.0f, 1.0f, -1.0f),     //left top near
    new Vector(1.0f, 1.0f, -1.0f),      //right top near
    new Vector(-1.0f, -1.0f, 1.0f),     //left down far
    new Vector(1.0f, -1.0f, 1.0f),      //right down far
    new Vector(-1.0f, 1.0f, 1.0f),      //left top far
    new Vector(1.0f, 1.0f, 1.0f),       //right top far
};

Plane[] planes = {
    new Plane(vertices[1], new Vector(0.0f, 0.0f, -1.0f)),  //bottom
    new Plane(vertices[2], new Vector(0.0f, 0.0f, 1.0f)),   //top
    new Plane(vertices[0], new Vector(-1.0f, 0.0f, 0.0f)),  //near
    new Plane(vertices[4], new Vector(1.0f, 0.0f, 0.0f)),   //far
    new Plane(vertices[6], new Vector(0.0f, -1.0f, 0.0f)),  //left
    new Plane(vertices[5], new Vector(0.0f, 1.0f, 0.0f)),   //right
};

Cube cube = new Cube(planes, vertices);
VirtualCamera camera = new VirtualCamera(new Vector(0.0f, 0.0f, -50000.0f), new Vector(0.0f, 0.0f, 1.0f), new Vector(0.0f, 1.0f, 0.0f));
camera.cube = cube;
Console.WriteLine(camera.ToString());
camera.Draw();

Console.WriteLine("Zmiana pozycji kamery: w, s, a, d, u, i. e - wyjście");
camera.Move();
Console.WriteLine("Zmiana rotacji kamery: w, s, a, d, u, i. e - wyjście");
camera.Rotate();

Console.ReadLine();
