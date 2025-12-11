var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided"),
};

var tiles = File.ReadLines(file).Select(line => Tile.Parse(line)).ToArray();

Console.WriteLine($"Part 1: {Part1(tiles)}");
Console.WriteLine($"Part 2: {Part2(tiles)}");

static long Part1(Tile[] tiles)
{
    var maxArea = long.MinValue;
    for (int i = 0; i < tiles.Length; i++)
    {
        for (int j = i + 1; j < tiles.Length; j++)
        {
            var area = tiles[i].AreaBetween(tiles[j]);
            if (area > maxArea)
            {
                maxArea = area;
            }
        }
    }

    return maxArea;
}

static long Part2(Tile[] tiles)
{
    return 0;
}

readonly record struct Tile(int X, int Y)
{
    public static Tile Parse(ReadOnlySpan<char> input)
    {
        var enumerator = input.Split(',');
        enumerator.MoveNext();
        var x = int.Parse(input[enumerator.Current]);
        enumerator.MoveNext();
        var y = int.Parse(input[enumerator.Current]);

        return new(x, y);
    }

    public long AreaBetween(Tile tile) =>
        (long)(Math.Abs(X - tile.X) + 1) * (Math.Abs(Y - tile.Y) + 1);
}
