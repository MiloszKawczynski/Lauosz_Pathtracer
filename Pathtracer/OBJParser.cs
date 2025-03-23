using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathtracer
{
    internal class OBJParser
    {
        
        public static List<Vector> LoadVertices(string pathToOBJFile)
        {
            List<Vector> vertices = new();
            foreach (var line in File.ReadLines(pathToOBJFile))
            {
                if (line.StartsWith("v "))
                {
                    string[] parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length >= 4)
                    {
                        if (float.TryParse(parts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float x) &&
                            float.TryParse(parts[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float y) &&
                            float.TryParse(parts[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float z))
                        {
                            vertices.Add(new Vector(x, y, z));
                        }
                        else
                        {
                            Console.WriteLine($"Błąd parsowania wiersza: {line}");
                        }
                    }
                }
            }
            return vertices;
        }
    }
}
