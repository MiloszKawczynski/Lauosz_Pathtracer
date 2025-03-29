using System;
using Pathtracer;

class Program
{
    static void Main(string[] args)
    {
        int imageSize = 255;

        VirtualCamera camera = new VirtualCamera();
        camera.SetFov(imageSize, 45);
        Image image = new Image(imageSize, imageSize, camera);

        image.scene.Add(new Sphere(new Point(-30.0f, 0, 0), 30));
        image.scene.Add(new Sphere(new Point(30.0f, 0, 30.0f), 30));

        image.RenderImage();
    }
}