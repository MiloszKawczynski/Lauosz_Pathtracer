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

        Sphere Ball2 = new Sphere(new Point(50.0f, 0, 90.0f), 30);
        Ball2.color = new LightIntensity(1.0f, 0.0f, 1.0f);
        Ball2.material.n = 5;

        float left = -100;
        float right = 100;

        float down = -100;
        float up = 100;

        float back = 200;
        float front = -200;

        Point boxLDB = new Point(left, down, back);
        Point boxLDF = new Point(left, down, front);

        Point boxLUB = new Point(left, up, back);
        Point boxLUF = new Point(left, up, front);

        Point boxRUB = new Point(right, up, back);
        Point boxRUF = new Point(right, up, front);

        Point boxRDB = new Point(right, down, back);
        Point boxRDF = new Point(right, down, front);

        Point boxMUM = new Point((right + left) / 2, up - 150.0f, (front + back) / 2);

        Triangle backWall1 = new Triangle(boxLDB, boxLUB, boxRUB);
        Triangle backWall2 = new Triangle(boxRDB, boxLDB, boxRUB);

        Triangle leftWall1 = new Triangle(boxLUB, boxLDB, boxLUF);
        Triangle leftWall2 = new Triangle(boxLDB, boxLDF, boxLUF);

        leftWall1.color = new LightIntensity(1.0f, 0.0f, 0.0f);
        leftWall2.color = new LightIntensity(1.0f, 0.0f, 0.0f);

        Triangle rightWall1 = new Triangle(boxRDB, boxRUB, boxRUF);
        Triangle rightWall2 = new Triangle(boxRDF, boxRDB, boxRUF);

        rightWall1.color = new LightIntensity(0.0f, 0.0f, 1.0f);
        rightWall2.color = new LightIntensity(0.0f, 0.0f, 1.0f);

        Triangle upWall1 = new Triangle(boxRUF, boxRUB, boxLUF);
        Triangle upWall2 = new Triangle(boxRUB, boxLUB, boxLUF);

        Triangle downWall1 = new Triangle(boxRDB, boxRDF, boxLDF);
        Triangle downWall2 = new Triangle(boxLDB, boxRDB, boxLDF);

        var pointLightLeft = new PointLight(new LightIntensity(0.0f, 0.0f, 1.0f), new Point(-50.0f, 0.0f, 50.0f));
        var pointLightRight = new PointLight(new LightIntensity(0.0f, 1.0f, 0.0f), new Point(50.0f, 0.0f, 50.0f));
        var pointLightUp = new PointLight(new LightIntensity(1.0f, 0.0f, 0.0f), new Point(0.0f, -50.0f, 50.0f));
        var pointLightDown = new PointLight(new LightIntensity(1.0f, 0.0f, 1.0f), new Point(0.0f, 50.0f, 50.0f));

        var pointLightLeftUp = new PointLight(new LightIntensity(1.0f, 1.0f, 0.0f), new Point(-50.0f, -50.0f, 50.0f));
        var pointLightRightUp = new PointLight(new LightIntensity(0.0f, 1.0f, 1.0f), new Point(50.0f, -50.0f, 50.0f));
        var pointLightLeftDown = new PointLight(new LightIntensity(1.0f, 0.5f, 0.5f), new Point(-50.0f, 50.0f, 50.0f));
        var pointLightRightDown = new PointLight(new LightIntensity(0.5f, 0.5f, 1.0f), new Point(50.05f, 50.0f, 50.0f));

        var pointLight = new PointLight(new LightIntensity(5.0f, 5.0f, 5.0f), boxMUM);

        var surfaceLightShift = new Vector(15, 0, 15);
        var surfaceLightHeight = new Vector(0, 1, 0);
        //var surfaceLight = new SurfaceLight(new LightIntensity(1.0f, 1.0f, 1.0f), boxMUM - surfaceLightShift + surfaceLightHeight, boxMUM + surfaceLightShift + surfaceLightHeight, 2);

        image.scene.Add(Ball1);
        //image.scene.Add(Ball2);

        image.scene.Add(backWall1);
        image.scene.Add(backWall2);

        image.scene.Add(leftWall1);
        image.scene.Add(leftWall2);

        image.scene.Add(rightWall1);
        image.scene.Add(rightWall2);

        image.scene.Add(upWall1);
        image.scene.Add(upWall2);

        image.scene.Add(downWall1);
        image.scene.Add(downWall2);

        image.lightSources.Add(pointLight);
        //image.lightSources.Add(surfaceLight);

        image.RenderImage();
    }
}