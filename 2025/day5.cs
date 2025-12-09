var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var lines = File.ReadAllLines(file);
var separator = Array.FindIndex(lines, x => string.IsNullOrWhiteSpace(x));
var ranges = lines.Take(separator).Select(FreshRange.Parse).ToArray();
var ids = lines.Skip(separator + 1).Select(long.Parse).ToArray();

Console.WriteLine($"Part 1: {Part1(ranges, ids)}");
Console.WriteLine($"Part 2: {Part2(ranges)}");

long Part1(FreshRange[] ranges, IEnumerable<long> ids) =>
    ids.Count(id => ranges.Any(x => x.IncludesId(id)));

long Part2(FreshRange[] ranges) => MergeRanges(ranges).Sum(range => range.Count);

IEnumerable<FreshRange> MergeRanges(FreshRange[] ranges)
{
    var merged = new LinkedList<FreshRange>();

    foreach (var range in ranges.OrderBy(x => x.Start))
    {
        if (merged.Count == 0 || merged.Last!.Value.End < range.Start - 1)
        {
            merged.AddLast(range);
        }
        else
        {
            merged.Last.Value = new FreshRange(
                merged.Last.Value.Start,
                Math.Max(merged.Last.Value.End, range.End)
            );
        }
    }

    return merged;
}

readonly record struct FreshRange(long Start, long End)
{
    internal static FreshRange Parse(string range)
    {
        var ids = range.Split('-');
        return new(long.Parse(ids[0]), long.Parse(ids[1]));
    }

    internal bool IncludesId(long value) => value >= Start && value <= End;

    internal long Count { get; } = End - Start + 1;
}
