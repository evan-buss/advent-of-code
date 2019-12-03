using System;
struct Dimension
{
    int length, height, width;
    int[] sorted;
    public Dimension(string line)
    {
        var parse = line.Split("x");
        this.length = Int32.Parse(parse[0]);
        this.width = Int32.Parse(parse[1]);
        this.height = Int32.Parse(parse[2]);
        // Sort the dimensions smallest to largest
        sorted = new int[] { this.length, this.width, this.height };
        Array.Sort(sorted);
    }

    public int volume()
    {
        return (2 * this.length * this.width)
            + (2 * this.width * this.height)
            + (2 * this.height * this.length)
            + sorted[0] + sorted[1];
    }

    public int bowLength()
    {
        return (sorted[0] * 2) + (sorted[1] * 2) + (length * width * height);
    }
}

// Part 1
var boxes = File.ReadLines("input.txt")
    .Select(line => new Dimension(line))
    .ToList();

var area = boxes.Select(box => box.volume()).Sum();
var ribbon = boxes.Select(box => box.bowLength()).Sum();

Console.WriteLine(area);
Console.WriteLine(ribbon);