using Problems.Attributes;

namespace Problems;

[Day(9)]
public class Day9 : IProblem
{
    [SampleFile("day9.sample.txt", 114)]
    [PuzzleFile("day9.txt", Expected = 1972648895)]
    public int Part1(string[] input)
    {
        long sum = 0;
        foreach (var line in input)
        {
            var history = new List<List<long>>
            {
                line.Split(" ").Select(long.Parse).ToList()
            };

            while (history[^1].Any(x => x != 0))
            {
                var newHistory = new List<long>();
                for (var i = 1; i < history[^1].Count; i++)
                {
                    var diff = history[^1][i] - history[^1][i - 1];
                    newHistory.Add(diff);
                }

                history.Add(newHistory);
            }

            history[^1].Add(0);

            for (var i = history.Count - 2; i >= 0; i--)
            {
                var below = history[i + 1];
                var row = history[i];

                var newValue = below[^1] + row[^1];

                row.Add(newValue);
            }

            sum += history[0][^1];
        }

        return (int) sum;
    }
    
    [SampleFile("day9.sample.txt", 2)]
    [PuzzleFile("day9.txt", Expected = 919)]
    public int Part2(string[] input)
    {
        long sum = 0;
        foreach (var line in input)
        {
            var history = new List<List<long>>
            {
                line.Split(" ").Select(long.Parse).ToList()
            };

            while (history[^1].Any(x => x != 0))
            {
                var newHistory = new List<long>();
                for (var i = 1; i < history[^1].Count; i++)
                {
                    var diff = history[^1][i] - history[^1][i - 1];
                    newHistory.Add(diff);
                }

                history.Add(newHistory);
            }

            history[^1].Insert(0, 0);

            for (var i = history.Count - 2; i >= 0; i--)
            {
                var below = history[i + 1];
                var row = history[i];

                var newValue = row[0] - below[0];

                row.Insert(0, newValue);
            }

            sum += history[0][0];
        }

        return (int) sum;
    }
}