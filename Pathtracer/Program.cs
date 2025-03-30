using System;
using Pathtracer;

class Program
{
    static void Main(string[] args)
    {
        int imageSize = 255;

        VirtualCamera camera = new VirtualCamera();
        camera.SetFov(imageSize, 45);
        camera.projection = ProjectionType.Perspective;
        Image image = new Image(imageSize, imageSize, camera);

        Sphere Ball1 = new Sphere(new Point(-10.0f, 0, 50.0f), 30);
        Ball1.color = new LightIntensity(0.0f, 1.0f, 0.0f);

        Sphere Ball2 = new Sphere(new Point(10.0f, 0, 90.0f), 30);
        Ball2.color = new LightIntensity(1.0f, 0.0f, 0.0f);

        image.scene.Add(Ball1);
        image.scene.Add(Ball2);

        image.RenderImage();
    }
}