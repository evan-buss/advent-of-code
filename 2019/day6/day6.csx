using System.Runtime.Serialization.Formatters;
using System.IO.Enumeration;
class Planet
{
    public string Name { get; set; }
    public List<Planet> Orbits { get; set; } = new List<Planet>();

    public Planet(string data)
    {
        this.Name = data;
    }
}

var lines = File.ReadAllLines("input.txt")
    .ToDictionary(
        line => line.Split(")")[0],
        line => line.Split(")")[1]
    );

Console.WriteLine(lines.Count);