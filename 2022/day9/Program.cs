using System.Drawing;

var lines = File.ReadLines("input.txt");

var seen = new HashSet<Point>();
var head = new Point(0, 0);
var tail = new Point(0, 0);

foreach (var command in lines)
{
    var times = int.Parse(command[2..]);
    for (var i = 0; i < times; i++)
    {
        var lastPosition = head;
        head = command[0] switch
        {
            'U' => head with { Y = head.Y + 1 },
            'D' => head with { Y = head.Y - 1 },
            'L' => head with { X = head.X - 1 },
            'R' => head with { X = head.X + 1 },
            _ => throw new Exception("Invalid Direction")
        };
        var diff = Point.Subtract(head, new Size(tail));

        if (Math.Abs(diff.X) > 1 || Math.Abs(diff.Y) > 1)
        {
            tail = lastPosition;
        }

        seen.Add(tail);

        // Console.WriteLine($"Command: {command} Tail{tail} -- Head: {head} -- Diff {diff}");
        // PrintGrid(head, tail);
    }
}

Console.WriteLine($"Part 1: {seen.Count}");

void PrintGrid(Point h, Point t, int size = 10)
{
    for (var y = 0; y < size; y++)
    {
        for (var x = 0; x < size; x++)
        {
            var curr = new Point(x, y);
            if (curr == h)
                Console.Write(" H ");
            else if (curr == t)
                Console.Write(" T ");
            else
                Console.Write(" . ");
        }

        Console.WriteLine();
    }
}