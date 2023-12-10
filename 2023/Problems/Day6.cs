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

        return times
            .Zip(distances)
            .Aggregate(1, (acc, tuple) => acc * (int)BeatRecords(tuple.First, tuple.Second));
    }

    [SampleFile("day6.sample.txt", 71_503)]
    [PuzzleFile("day6.txt", Expected = 36_872_656)]
    public int Part2(string[] input)
    {
        var time = ParseSingleRace(input[0]);
        var distance = ParseSingleRace(input[1]);

        return (int)BeatRecords(time, distance);
    }

    private static long BeatRecords(long time, long distance)
    {
        long lowestSeconds = 0;
        long highestSeconds = 0;

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

        return highestSeconds - lowestSeconds + 1;
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
