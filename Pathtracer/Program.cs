using System;
using Pathtracer;

class Program
{
    static void Main(string[] args)
    {
        VirtualCamera camera = new VirtualCamera();
        Image image = new Image(255, 255, camera);

        image.scene.Add(new Sphere(new Point(-15.0f, 0, 5), 30));
        image.scene.Add(new Sphere(new Point(15.0f, 0, 5), 30));

        image.RenderImage();
    }
}