using System.Diagnostics;
using System.Security.Principal;
using Problems.Attributes;

namespace Problems;

[Day(3)]
public class Day3 : IProblem
{
    public record Coord(int X, int Y);

    [DebuggerDisplay($"{{{nameof(Value)}}}")]
    public class PartCode(int value)
    {
        public int Value { get; } = value;
    };

    [SampleFile("day3.sample.txt", 925)]
    [PuzzleFile("day3.txt", Expected = 559667)]
    public int Part1(string[] input)
    {
        var partCodes = new Dictionary<Coord, PartCode>();
        var symbols = new Dictionary<Coord, char>();

        for (var y = 0; y < input.Length; y++)
        {
            var accum = new List<char>();
            var coords = new List<Coord>();
            for (var x = 0; x < input[y].Length; x++)
            {
                var c = input[y][x];
                if (char.IsNumber(c))
                {
                    accum.Add(c);
                    coords.Add(new(x, y));
                }
                else if (c != '.')
                {
                    symbols.Add(new(x, y), c);
                }


                // We hit a non-number and we have numbers accumulated OR
                // We hit the end of the line and we have numbers accumulated
                if ((!char.IsNumber(c) || x == input[y].Length - 1) && accum.Count > 0)
                {
                    var code = new PartCode(int.Parse(string.Join("", accum)));
                    foreach (var coord in coords)
                    {
                        partCodes.Add(coord, code);
                    }

                    accum.Clear();
                    coords.Clear();
                }
            }
        }

        var sum = 0;
        foreach (var (coord, _) in symbols)
        {
            var startX = Math.Max(0, coord.X - 1);
            var endX = Math.Min(input[0].Length, coord.X + 1);
            var startY = Math.Max(0, coord.Y - 1);
            var endY = Math.Min(input.Length, coord.Y + 1);

            var symbolCodes = new HashSet<PartCode>();
            for (var y = startY; y <= endY; y++)
            {
                for (var x = startX; x <= endX; x++)
                {
                    var searching = new Coord(x, y);
                    if (partCodes.TryGetValue(searching, out var code))
                    {
                        symbolCodes.Add(code);
                    }
                }
            }


            sum += symbolCodes.Sum(x => x.Value);
            symbolCodes.Clear();
        }

        return sum;
    }

    public int Part2(string[] input)
    {
        throw new NotImplementedException();
    }
}