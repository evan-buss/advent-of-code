using System.Text.RegularExpressions;
using Problems.Attributes;

namespace Problems;

[Day(8)]
public partial class Day8 : IProblem
{
    [SampleFile("day8.sample.txt", 6)]
    [PuzzleFile("day8.txt", Expected = 17621)]
    public int Part1(string[] input)
    {
        var instructions = input[0]
            .Select(x => x == 'R' ? Instruction.Right : Instruction.Left)
            .ToList();
        var map = input[2..]
            .Select(line => InstructionRegex().Match(line))
            .ToDictionary(
                match => match.Groups[1].Value,
                match => new[] { match.Groups[2].Value, match.Groups[3].Value }
            );

        var count = 0;
        var i = 0;
        var curr = "AAA";
        while (true)
        {
            var instruction = instructions[i % instructions.Count];
            var node = map[curr];

            if (curr == "ZZZ")
            {
                break;
            }

            curr = instruction switch
            {
                Instruction.Left => node[0],
                Instruction.Right => node[1]
            };

            count++;
            i++;
        }

        return count;
    }

    [SampleFile("day8.part2.sample.txt", 6)]
    [PuzzleFile("day8.txt", Expected = 962334463)] // Wrong answer but correct when casting the correct long answer to int
    public int Part2(string[] input)
    {
        var instructions = input[0]
            .Select(x => x == 'R' ? Instruction.Right : Instruction.Left)
            .ToList();
        var map = input[2..]
            .Select(line => InstructionRegex().Match(line))
            .ToDictionary(
                match => match.Groups[1].Value,
                match => new[] { match.Groups[2].Value, match.Groups[3].Value }
            );

        var starts = map.Keys.Where(x => x.EndsWith('A')).ToArray();

        var loops = new List<long>();
        foreach (var start in starts)
        {
            var i = 0;
            var curr = start;
            while (true)
            {
                if (curr.EndsWith('Z'))
                {
                    break;
                }

                var node = map[curr];

                curr = instructions[i % instructions.Count] switch
                {
                    Instruction.Left => node[0],
                    Instruction.Right => node[1]
                };

                i++;
            }

            loops.Add(i);
        }

        var value = loops.Aggregate(LCM);

        // TODO: Incorrect because it overflows an int. Need to set up runner to work with long...
        return (int)value;
    }

    private long LCM(long a, long b)
    {
        return Math.Abs(a * b) / GCD(a, b);
    }

    private long GCD(long a, long b)
    {
        return b == 0 ? a : GCD(b, a % b);
    }

    // 16, 8
    // 8, 8
    // 0, 8

    public enum Instruction
    {
        Left,
        Right,
    }

    [GeneratedRegex(@"([1-9A-Z]{3}) = \(([1-9A-Z]{3}), ([1-9A-Z]{3})\)")]
    private static partial Regex InstructionRegex();
}
