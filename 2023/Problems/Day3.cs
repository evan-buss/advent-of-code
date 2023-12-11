using System.Diagnostics;
using Problems.Attributes;

namespace Problems;

[Day(3)]
public class Day3 : IProblem
{
    private record struct Coord(int X, int Y);

    // Since this is a class, using it in a HashSet will compare based on reference.
    // This is desired as we store the coordinate of each digit of the number in the dictionary
    // with a shared PartCode instance. If a single number borders a symbol multiple times, it will
    // only have 1 entry in the HashSet.
    [DebuggerDisplay($"{{{nameof(Value)}}}")]
    private class PartCode(int value)
    {
        public int Value { get; } = value;
    };

    [SampleFile("day3.sample.txt", 925)]
    [PuzzleFile("day3.txt", Expected = 559667)]
    public int Part1(string[] input)
    {
        var (partCodes, symbols) = ParseInput(input);

        var codes = new HashSet<PartCode>();
        foreach (var (coord, _) in symbols)
        {
            foreach (var adjacent in EnumeratePerimeter(coord, input))
            {
                if (partCodes.TryGetValue(adjacent, out var code))
                {
                    codes.Add(code);
                }
            }
        }

        return codes.Sum(x => x.Value);
    }

    [SampleFile("day3.sample.txt", 6756)]
    [PuzzleFile("day3.txt", Expected = 86841457)]
    public int Part2(string[] input)
    {
        var (partCodes, symbols) = ParseInput(input);

        var sum = 0;
        var symbolCodes = new HashSet<PartCode>();
        foreach (var (coord, _) in symbols.Where(x => x.Value == '*'))
        {
            foreach (var adjacent in EnumeratePerimeter(coord, input))
            {
                if (partCodes.TryGetValue(adjacent, out var code))
                {
                    symbolCodes.Add(code);
                }
            }

            if (symbolCodes.Count == 2)
            {
                sum += symbolCodes.Aggregate(1, (i, code) => i * code.Value);
            }

            symbolCodes.Clear();
        }

        return sum;
    }

    private static IEnumerable<Coord> EnumeratePerimeter(Coord coord, IReadOnlyList<string> input)
    {
        var startX = Math.Max(0, coord.X - 1);
        var endX = Math.Min(input[0].Length, coord.X + 1);
        var startY = Math.Max(0, coord.Y - 1);
        var endY = Math.Min(input.Count, coord.Y + 1);

        for (var y = startY; y <= endY; y++)
        {
            for (var x = startX; x <= endX; x++)
            {
                yield return new(x, y);
            }
        }
    }

    private static (Dictionary<Coord, PartCode>, Dictionary<Coord, char>) ParseInput(
        IReadOnlyList<string> input
    )
    {
        var partCodes = new Dictionary<Coord, PartCode>();
        var symbols = new Dictionary<Coord, char>();

        for (var y = 0; y < input.Count; y++)
        {
            var numbers = new List<char>();
            var coords = new List<Coord>();
            for (var x = 0; x < input[y].Length; x++)
            {
                var c = input[y][x];
                if (char.IsNumber(c))
                {
                    numbers.Add(c);
                    coords.Add(new(x, y));
                }
                else if (c != '.')
                {
                    symbols.Add(new(x, y), c);
                }

                // We hit a non-number and we have numbers accumulated OR
                // We hit the end of the line and we have numbers accumulated
                if ((!char.IsNumber(c) || x == input[y].Length - 1) && numbers.Count > 0)
                {
                    var code = new PartCode(int.Parse(string.Join("", numbers)));
                    foreach (var coord in coords)
                    {
                        partCodes.Add(coord, code);
                    }

                    numbers.Clear();
                    coords.Clear();
                }
            }
        }

        return (partCodes, symbols);
    }
}
