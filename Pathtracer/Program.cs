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

        Sphere Ball1 = new Sphere(new Point(0.0f, 0, 50.0f), 30);
        Ball1.color = new LightIntensity(0.0f, 1.0f, 1.0f);
        Ball1.material.n = 5;

        Sphere Ball2 = new Sphere(new Point(20.0f, 0, 50.0f), 15);
        Ball2.color = new LightIntensity(1.0f, 0.0f, 1.0f);
        Ball2.material.n = 5;

        var pointLightLeft = new PointLight(new LightIntensity(0.0f, 0.0f, 1.0f), new Vector(-50.0f, 0.0f, 50.0f));
        var pointLightRight = new PointLight(new LightIntensity(0.0f, 1.0f, 0.0f), new Vector(50.0f, 0.0f, 50.0f));
        var pointLightUp = new PointLight(new LightIntensity(1.0f, 0.0f, 0.0f), new Vector(0.0f, -50.0f, 50.0f));
        var pointLightDown = new PointLight(new LightIntensity(1.0f, 0.0f, 1.0f), new Vector(0.0f, 50.0f, 50.0f));

        var pointLightLeftUp = new PointLight(new LightIntensity(1.0f, 1.0f, 0.0f), new Vector(-50.0f, -50.0f, 50.0f));
        var pointLightRightUp = new PointLight(new LightIntensity(0.0f, 1.0f, 1.0f), new Vector(50.0f, -50.0f, 50.0f));
        var pointLightLeftDown = new PointLight(new LightIntensity(1.0f, 0.5f, 0.5f), new Vector(-50.0f, 50.0f, 50.0f));
        var pointLightRightDown = new PointLight(new LightIntensity(0.5f, 0.5f, 1.0f), new Vector(50.05f, 50.0f, 50.0f));

        image.scene.Add(Ball1);
        //image.scene.Add(Ball2);
        image.lightSources.Add(pointLightLeft);
        image.lightSources.Add(pointLightRight);
        //image.lightSources.Add(pointLightUp);
        //image.lightSources.Add(pointLightDown);

        //image.lightSources.Add(pointLightLeftUp);
        //image.lightSources.Add(pointLightRightUp);
        //image.lightSources.Add(pointLightLeftDown);
        //image.lightSources.Add(pointLightRightDown);

        image.RenderImage();
    }
}