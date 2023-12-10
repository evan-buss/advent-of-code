using Problems.Attributes;

namespace Problems;

[Day(6)]
public class Day6 : IProblem
{
    [SampleFile("day6.sample.txt", 288)]
    [PuzzleFile("day6.txt", Expected = 393_120)]
    public int Part1(string[] input)
    {
        var times = ParseMultipleRaces(input[0]);
        var distances = ParseMultipleRaces(input[1]);

        var recordsPerRace = new List<int>();
        foreach (var (time, distance) in times.Zip(distances))
        {
            var records = 0;
            for (var holdSeconds = 0; holdSeconds <= time; holdSeconds++)
            {
                var moveTime = time - holdSeconds;
                var distanceCovered = moveTime * holdSeconds;

                if (distanceCovered > distance)
                {
                    records++;
                }
            }

            recordsPerRace.Add(records);
        }

        return recordsPerRace.Aggregate(1, (acc, curr) => acc * curr);
    }

    [SampleFile("day6.sample.txt", 71_503)]
    [PuzzleFile("day6.txt", Expected = 36_872_656)]
    public int Part2(string[] input)
    {
        var time = ParseSingleRace(input[0]);
        var distance = ParseSingleRace(input[1]);

        var lowestSeconds = long.MaxValue;
        var highestSeconds = long.MinValue;

        for (var holdSeconds = 0; holdSeconds <= time; holdSeconds++)
        {
            var moveTime = time - holdSeconds;
            var distanceCovered = moveTime * holdSeconds;

            if (distanceCovered > distance)
            {
                lowestSeconds = holdSeconds;
                break;
            }
        }

        for (var holdSeconds = time; holdSeconds > 0; holdSeconds--)
        {
            var moveTime = time - holdSeconds;
            var distanceCovered = moveTime * holdSeconds;

            if (distanceCovered > distance)
            {
                highestSeconds = holdSeconds;
                break;
            }
        }

        return (int)highestSeconds - (int)lowestSeconds + 1;
    }

    private static int[] ParseMultipleRaces(string line)
    {
        return line.Split(":")[1]
            .Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();
    }

    private static long ParseSingleRace(string line)
    {
        return long.Parse(line.Split(":")[1].Replace(" ", ""));
    }
}
