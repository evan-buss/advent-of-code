var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var rows = File.ReadLines(file).Select(row => row.ToCharArray()).ToArray();

Console.WriteLine($"Part 1: {Part1(rows)}");
Console.WriteLine($"Part 2: {Part2(rows)}");

static int Part1(char[][] rows) => AllCells(rows).Count(coord => IsAccessible(rows, coord));

static int Part2(char[][] rows)
{
    var remove = AllCells(rows).Where(coord => IsAccessible(rows, coord)).ToArray();

    foreach (var (y, x) in remove)
    {
        rows[y][x] = '.';
    }

    return remove.Length == 0 ? 0 : remove.Length + Part2(rows);
}

static IEnumerable<Coord> AllCells(char[][] rows) =>
    from y in Enumerable.Range(0, rows.Length)
    from x in Enumerable.Range(0, rows[y].Length)
    select new Coord(y, x);

static bool IsAccessible(char[][] rows, Coord coord) =>
    IsOccupied(rows, coord) && CountAdjacent(rows, coord) < 4;

static bool IsOccupied(char[][] rows, Coord coord) => rows[coord.Y][coord.X] is '@' or 'x';

static int CountAdjacent(char[][] rows, Coord coord)
{
    (int Y, int X)[] directions =
    [
        (-1, -1),
        (-1, 0),
        (-1, 1),
        (0, -1),
        (0, 1),
        (1, -1),
        (1, 0),
        (1, 1)
    ];

    return directions.Count(d =>
    {
        var newCoord = new Coord(coord.Y + d.Y, coord.X + d.X);
        return InBounds(rows, newCoord) && IsOccupied(rows, newCoord);
    });

    static bool InBounds(char[][] rows, Coord coord) =>
        coord.Y >= 0 && coord.Y < rows.Length && coord.X >= 0 && coord.X < rows[0].Length;
}

readonly record struct Coord(int Y, int X);
