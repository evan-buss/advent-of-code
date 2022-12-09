using System.Drawing;

Console.WriteLine($"Part 1: {ComputeGrid(File.ReadLines("input.txt"), 2)}");
Console.WriteLine($"Part 2: {ComputeGrid(File.ReadLines("input.txt"), 10)}");

int ComputeGrid(IEnumerable<string> lines, int ropeLength)
{
    var seen = new HashSet<Point>();
    var rope = new Point[ropeLength];
    Array.Fill(rope, new Point(0, 0));

    foreach (var command in lines)
    {
        var times = int.Parse(command[2..]);
        for (var i = 0; i < times; i++)
        {
            // Compute new head position.
            rope[0] = command[0] switch
            {
                'U' => rope[0] with { Y = rope[0].Y + 1 },
                'D' => rope[0] with { Y = rope[0].Y - 1 },
                'L' => rope[0] with { X = rope[0].X - 1 },
                'R' => rope[0] with { X = rope[0].X + 1 },
                _ => throw new Exception("Invalid Direction")
            };

            // Update all tail segments
            for (var j = 1; j < rope.Length; j++)
            {
                var rowDiff = rope[j - 1].X - rope[j].X;
                var colDiff = rope[j - 1].Y - rope[j].Y;

                if (Math.Abs(rowDiff) > 1 || Math.Abs(colDiff) > 1)
                {
                    rope[j] = rope[j] with
                    {
                        X = rope[j].X + Math.Sign(rowDiff),
                        Y = rope[j].Y + Math.Sign(colDiff)
                    };
                }
            }

            seen.Add(rope[^1]);
        }
    }

    return seen.Count;
}