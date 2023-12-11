using Problems.Attributes;

namespace Problems;

[Day(5)]
public class Day5 : IProblem
{
    private sealed class SeedRange(IReadOnlyList<long> range)
    {
        private readonly long _rangeStart = range[0];
        private readonly long _rangeEnd = range[1];

        public bool InRange(long source)
        {
            return source >= _rangeStart && source <= _rangeStart + _rangeEnd;
        }
    }

    private sealed class Range
    {
        private readonly long _destinationRangeStart;
        private readonly long _sourceRangeStart;
        private readonly long _length;

        public Range(string line)
        {
            var parts = line.Split(" ").Select(long.Parse).ToArray();
            _destinationRangeStart = parts[0];
            _sourceRangeStart = parts[1];
            _length = parts[2];
        }

        public long? GetDestination(long source)
        {
            if (source < _sourceRangeStart || source >= _sourceRangeStart + _length)
            {
                return null;
            }

            var sourceOffset = source - _sourceRangeStart;
            return _destinationRangeStart + sourceOffset;
        }

        public long? GetSource(long destination)
        {
            if (
                destination < _destinationRangeStart
                || destination >= _destinationRangeStart + _length
            )
            {
                return null;
            }

            var destinationOffset = destination - _destinationRangeStart;
            return _sourceRangeStart + destinationOffset;
        }
    }

    private sealed class Bucket
    {
        public string Name { get; }
        private readonly List<Range> _ranges;

        public Bucket(IReadOnlyCollection<string> lines)
        {
            Name = lines.First()[..^1];
            _ranges = lines.Skip(1).Select((x) => new Range(x)).ToList();
        }

        public long GetDestination(long source)
        {
            return _ranges.Select(x => x.GetDestination(source)).FirstOrDefault(x => x != null)
                ?? source;
        }

        public long GetSource(long destination)
        {
            return _ranges.Select(x => x.GetSource(destination)).FirstOrDefault(x => x != null)
                ?? destination;
        }
    };

    [SampleFile("Day5.sample.txt", 35)]
    [PuzzleFile("Day5.txt", Expected = 240320250)]
    public int Part1(string[] input)
    {
        var seeds = new List<long>(input[0].Split(": ")[1].Split(" ").Select(long.Parse));
        var buckets = input[2..]
            .ChunkBy(line => line.Length == 0)
            .Select(x => new Bucket(x))
            .ToList();

        return (int)
            seeds
                .Select(
                    seed =>
                        buckets.Aggregate(seed, (current, bucket) => bucket.GetDestination(current))
                )
                .Min();
    }

    [SampleFile("Day5.sample.txt", 46)]
    [PuzzleFile("Day5.txt", Expected = 28580589, Skip = true)]
    public int Part2(string[] input)
    {
        var seeds = input[0]
            .Split(": ")[1]
            .Split(" ")
            .Select(long.Parse)
            .Chunk(2)
            .Select(x => new SeedRange(x))
            .ToList();
        var buckets = input[2..]
            .ChunkBy(line => line.Length == 0)
            .Select(x => new Bucket(x))
            .Reverse()
            .ToList();

        long location = 1;
        while (true)
        {
            var source = buckets.Aggregate(
                location,
                (current, bucket) => bucket.GetSource(current)
            );

            if (seeds.Exists(seed => seed.InRange(source)))
            {
                return (int)location;
            }

            location++;
        }
    }
}

public static class ChunkExtension
{
    public static IEnumerable<List<T>> ChunkBy<T>(
        this IEnumerable<T> source,
        Func<T, bool> shouldChunk
    )
    {
        var chunk = new LinkedList<T>();
        foreach (var line in source)
        {
            if (shouldChunk(line))
            {
                yield return [..chunk];
                chunk.Clear();
                continue;
            }

            chunk.AddLast(line);
        }

        yield return [..chunk];
    }
}
