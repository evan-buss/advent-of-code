var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var input = File.ReadAllText(file);

Console.WriteLine($"Part 1: {Parse(input).Where(Part1).Sum()}");
Console.WriteLine($"Part 2: {Parse(input).Where(Part2).Sum()}");

static bool Part1(long id)
{
    Span<char> buffer = stackalloc char[20];

    if (!id.TryFormat(buffer, out int written))
    {
        return false;
    }

    if (written % 2 != 0)
    {
        return false;
    }

    var mid = written / 2;

    return MemoryExtensions.Equals(buffer[..mid], buffer[mid..written], StringComparison.Ordinal);
}

static bool Part2(long id)
{
    Span<char> buffer = stackalloc char[20];

    if (!id.TryFormat(buffer, out int written))
    {
        return false;
    }

    for (int i = 0; i < written; i++)
    {
        var sequence = buffer[i..written];
        if (sequence.Length == written)
        {
            continue;
        }

        if (
            MemoryExtensions.Equals(
                sequence.Repeat(written / sequence.Length),
                buffer[..written],
                StringComparison.Ordinal
            )
        )
        {
            return true;
        }
    }

    return false;
}

static IEnumerable<long> Parse(string input)
{
    return input
        .Split(',')
        .SelectMany(range =>
        {
            var ids = range.Split('-');
            return Extensions.Range(
                long.Parse(ids[0]),
                long.Parse(ids[1]) - long.Parse(ids[0]) + 1
            );
        });
}

public static class Extensions
{
    public static IEnumerable<long> Range(long start, long count)
    {
        long limit = start + count;
        while (start < limit)
        {
            yield return start;
            start++;
        }
    }

    public static Span<char> Repeat(this Span<char> textSpan, int n)
    {
        var span = new Span<char>(new char[textSpan.Length * n]);
        for (var i = 0; i < n; i++)
        {
            textSpan.CopyTo(span.Slice(i * textSpan.Length, textSpan.Length));
        }

        return span;
    }
}
