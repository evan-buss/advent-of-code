var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided"),
};

var lines = File.ReadAllLines(file);

Console.WriteLine($"Part 1: {Part1(lines)}");
Console.WriteLine($"Part 2: {Part2(lines)}");

static int Part1(string[] manifold)
{
    var splits = 0;
    HashSet<int> beams = [manifold[0].IndexOf('S')];

    for (int step = 0; step < manifold.Length; step++)
    {
        for (int i = 0; i < manifold[step].Length; i++)
        {
            if (manifold[step][i] == '^' && beams.Contains(i))
            {
                beams.Add(i - 1);
                beams.Add(i + 1);
                beams.Remove(i);
                splits++;
            }
        }
    }

    return splits;
}

static long Part2(string[] input)
{
    var manifold = input.Select(row => row.Select(Piece.Parse).ToArray()).ToArray();

    for (int y = 1; y < manifold.Length; y++)
    {
        for (int x = 0; x < manifold[y].Length; x++)
        {
            ref var curr = ref manifold[y][x];
            var above = manifold[y - 1][x];

            if (curr is Piece.Reflector)
            {
                ref var left = ref manifold[y][x - 1];
                ref var right = ref manifold[y][x + 1];
                left = new Piece.Value(left.Val + above.Val);
                right = new Piece.Value(right.Val + above.Val);
            }
            else if (above is not Piece.Empty)
            {
                curr = new Piece.Value(curr.Val + above.Val);
            }
        }
    }

    return manifold[^1].Sum(x => x.Val);
}

abstract record Piece(long Val)
{
    public static Piece Parse(char c) =>
        c switch
        {
            'S' => new Start(),
            '.' => new Empty(),
            '^' => new Reflector(),
            _ => throw new ArgumentException($"Invalid Piece: '{c}'", nameof(c)),
        };

    public sealed record Value(long Val) : Piece(Val);

    public sealed record Reflector() : Piece(0);

    public sealed record Start() : Piece(1);

    public sealed record Empty() : Piece(0);
}
