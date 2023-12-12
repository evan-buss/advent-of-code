using Problems.Attributes;

namespace Problems;

[Day(10)]
public class Day10 : IProblem
{
    private sealed record Point(int X, int Y);

    private static readonly Dictionary<char, char[]> PipeTypes =
        new()
        {
            ['|'] = new[] { 'N', 'S' },
            ['-'] = new[] { 'W', 'E' },
            ['L'] = new[] { 'N', 'E' },
            ['J'] = new[] { 'N', 'W' },
            ['7'] = new[] { 'S', 'W' },
            ['F'] = new[] { 'S', 'E' },
            ['S'] = new[] { 'N', 'S', 'E', 'W' }
        };

    private static readonly Dictionary<char, (Point Point, char Direction)> Directions =
        new()
        {
            ['N'] = (new(0, -1), 'S'),
            ['S'] = (new(0, 1), 'N'),
            ['E'] = (new(1, 0), 'W'),
            ['W'] = (new(-1, 0), 'E')
        };

    private readonly Dictionary<Point, int> _seen = new();

    [SampleFile("day10.sample.simple.txt", 4)]
    [PuzzleFile("day10.txt")]
    public int Part1(string[] input)
    {
        var (map, start) = ConstructMap(input);
        var queue = new Queue<(Point Point, int Distance)>([(start, 0)]);

        while (queue.Count > 0)
        {
            var (current, distance) = queue.Dequeue();
            if (!_seen.TryAdd(current, distance))
            {
                continue;
            }

            var directions = PipeTypes[map[current.Y][current.X]];
            foreach (var direction in directions)
            {
                var ((deltaX, deltaY), opposite) = Directions[direction];
                var newPoint = new Point(current.X + deltaX, current.Y + deltaY);

                if (
                    (newPoint.X < 0 || newPoint.X >= map[0].Count)
                    || (newPoint.Y < 0 || newPoint.Y >= map.Count)
                )
                {
                    continue;
                }

                var target = map[newPoint.Y][newPoint.X];

                if (!PipeTypes.TryGetValue(target, out var targetDirections))
                {
                    continue;
                }

                if (targetDirections.Contains(opposite))
                {
                    queue.Enqueue((newPoint, distance + 1));
                }
            }
        }

        return _seen.Values.Max();
    }

    [SampleFile("day10.part2.sample.txt", 10)]
    public int Part2(string[] input)
    {
        var (map, start) = ConstructMap(input);
        for (var y = 0; y < map.Count; y++)
        {
            var norths = 0;
            for (var x = 0; x < map[0].Count; x++)
            {
                var place = map[y][x];
                if (_seen.ContainsKey(new Point(x, y)))
                {
                    var pipeDirections = PipeTypes[place];
                    if (pipeDirections.Contains('N'))
                    {
                        norths++;
                    }

                    continue;
                }

                if (norths % 2 == 0)
                {
                    map[y][x] = 'O';
                }
                else
                {
                    map[y][x] = 'I';
                }
            }
        }

        return map.Select(x => x.Count(c => c == 'I')).Sum();
    }

    private static (List<List<char>> map, Point start) ConstructMap(string[] input)
    {
        var map = new List<List<char>>();
        var start = new Point(0, 0);
        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y];
            var row = new List<char>();

            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (c == 'S')
                {
                    start = new Point(x, y);
                }

                row.Add(c);
            }

            map.Add(row);
        }

        return (map, start);
    }
}
