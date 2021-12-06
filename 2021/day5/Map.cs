using System.Text;
using System.Drawing;

namespace Day5;

public record Coordinate(int X, int Y)
{
    public static Coordinate Parse(string coords)
    {
        var parts = coords.Split(",").Select(int.Parse).ToArray();
        return new Coordinate(parts[0], parts[1]);
    }
}

public record Line(Coordinate Start, Coordinate End)
{
    public static Line Parse(string line)
    {
        var parts = line.Split(" -> ");
        return new Line(Coordinate.Parse(parts[0]), Coordinate.Parse(parts[1]));
    }
}

public class Map
{
    private readonly int[,] _map = new int[1000, 1000];

    public int OverlapCount
    {
        get
        {
            var sum = 0;
            for (var y = 0; y < _map.GetLength(0); y++)
            {
                for (var x = 0; x < _map.GetLength(1); x++)
                {
                    if (_map[y, x] > 1) sum++;
                }
            }

            return sum;
        }
    }

    public void Plot(Line line, bool diag = false)
    {
        var ((startX, startY), (endX, endY)) = line;

        var xStep = startX > endX ? -1 : 1;
        var yStep = startY > endY ? -1 : 1;

        var x = startX;
        var y = startY;

        if (startX == endX) // Vertical Line
        {
            xStep = 0;
        }
        else if (startY == endY) // Horizontal Line
        {
            yStep = 0;
        }
        else if (!diag)
        {
            return;
        }

        // If the line is actually a single point, condition is always false and point isn't added
        // So we add it first.
        _map[y, x]++;
        while (x != endX || y != endY)
        {
            x += xStep;
            y += yStep;
            _map[y, x]++;
        }
    }

    public string RenderSection(int maxX, int maxY)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                sb.Append(_map[y, x].ToString().PadLeft(2));
                sb.Append(' ');
            }

            sb.Append('\n');
        }

        return sb.ToString();
    }

#pragma warning disable CA1416
    public void RenderBitmap(string fileName, int maxX, int maxY)
    {
        var bmp = new Bitmap(maxX, maxY);
        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                bmp.SetPixel(x, y, _map[y, x] > 0 ? Color.Red : Color.Black);
            }
        }

        bmp.Save(fileName);
    }
#pragma warning restore CA1416
}