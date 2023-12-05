using Problems.Attributes;

namespace Problems;

[Day(1)]
public class Day1 : IProblem
{
    private static readonly Dictionary<string, int> NumberMap =
        new()
        {
            ["one"] = 1,
            ["two"] = 2,
            ["three"] = 3,
            ["four"] = 4,
            ["five"] = 5,
            ["six"] = 6,
            ["seven"] = 7,
            ["eight"] = 8,
            ["nine"] = 9,
            ["1"] = 1,
            ["2"] = 2,
            ["3"] = 3,
            ["4"] = 4,
            ["5"] = 5,
            ["6"] = 6,
            ["7"] = 7,
            ["8"] = 8,
            ["9"] = 9,
        };

    [PuzzleFile("day1.txt", Expected = 54331)]
    [SampleFile("part1.sample.txt", 142)]
    public int Part1(string[] lines)
    {
        Console.WriteLine("debug test");
        return lines.Sum(GetLineValue);
    }

    [PuzzleFile("day1.txt", Expected = 54518)]
    [SampleFile("part2.sample.txt", 281)]
    public int Part2(string[] lines)
    {
        return lines.Select(ConvertLine).Sum(GetLineValue);
    }

    public static string ConvertLine(string line)
    {
        var firstIndex = line.Length;
        var firstValue = "";
        foreach (var key in NumberMap.Keys)
        {
            var foundIndex = line.IndexOf(key, StringComparison.Ordinal);
            if (foundIndex != -1 && foundIndex < firstIndex)
            {
                firstIndex = foundIndex;
                firstValue = key;
            }
        }

        var lastIndex = 0;
        var lastValue = "";
        foreach (var key in NumberMap.Keys)
        {
            var foundIndex = line.LastIndexOf(key, StringComparison.Ordinal);
            if (foundIndex > lastIndex)
            {
                lastIndex = foundIndex;
                lastValue = key;
            }
        }

        var output = "";
        if (firstValue != "")
        {
            output += NumberMap[firstValue];
        }

        if (lastValue != "")
        {
            output += NumberMap[lastValue];
        }

        return output;
    }

    private static int GetLineValue(string line)
    {
        var nums = line.Where(char.IsNumber).ToArray();
        var value = nums switch
        {
            [var x] => new string(x, 2),
            [var x, .., var y] => new string(new[] { x, y }),
            [] => "0"
        };

        return int.Parse(value);
    }
}
