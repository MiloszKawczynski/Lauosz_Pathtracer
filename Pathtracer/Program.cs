using System;
using Pathtracer;
using Pathtracer.Light;

class Program
{
    static void Main(string[] args)
    {
        int imageSize = 255;

        VirtualCamera camera = new VirtualCamera();
        camera.SetFov(imageSize, 45);
        camera.projection = ProjectionType.Perspective;
        Image image = new Image(imageSize, imageSize, camera);

        Sphere Ball1 = new Sphere(new Point(-80.0f, 0, 0.0f), 30);
        Ball1.color = new LightIntensity(0.0f, 1.0f, 0.0f);

        Sphere Ball2 = new Sphere(new Point(-20.0f, 0, 0.0f), 15);
        Ball2.color = new LightIntensity(0.0f, 1.0f, 0.0f);

        var pointLight = new PointLight(new LightIntensity(1.0f, 0.0f, 0.0f), new Vector(50.0f, 0f, 0.0f));
        //var surfaceLight = new SurfaceLight(new LightIntensity(1.0f, 0.0f, 0.0f), new Vector(150.0f, -50f, -20.0f), 10, 10, 2);

        image.scene.Add(Ball1);
        image.scene.Add(Ball2);
        image.lightSources.Add(pointLight);

        image.RenderImage();
    }
}