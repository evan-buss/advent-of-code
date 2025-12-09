var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var rows = File.ReadLines(file).Select(row => row.Select(x => x).ToArray()).ToArray();

Console.WriteLine($"Part 1: {Part1(rows)}");

static int Part1(char[][] rows)
{
    var total = 0;
    for (var y = 0; y < rows.Length; y++)
    {
        for (int x = 0; x < rows[y].Length; x++)
        {
            if (rows[y][x] == '.')
            {
                continue;
            }

            var adjacent = CountAdjacent(rows, y, x);

            if (adjacent < 4)
            {
                total++;
            }
        }
    }
    return total;
}

static int CountAdjacent(char[][] rows, int y, int x)
{
    var sum = 0;
    for (int yD = -1; yD <= 1; yD++)
    {
        for (int xD = -1; xD <= 1; xD++)
        {
            var newY = y + yD;
            var newX = x + xD;

            if (yD == 0 && xD == 0)
            {
                continue;
            }

            if (newY < 0 || newY > rows.Length - 1)
            {
                continue;
            }

            if (newX < 0 || newX > rows[0].Length - 1)
            {
                continue;
            }

            if (rows[newY][newX] is '@' or 'x')
            {
                sum++;
            }
        }
    }

    return sum;
}

static void Print(char[][] rows)
{
    foreach (var row in rows)
    {
        Console.WriteLine(row);
    }
}
