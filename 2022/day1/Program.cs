var elfCalories = File.ReadLines("part1.txt")
    .ChunkBy(string.IsNullOrWhiteSpace)
    .Select(x => x.Sum(int.Parse))
    .ToList();

var part1 = elfCalories.Max();
var part2 = elfCalories.OrderDescending().Take(3).Sum();

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

static class Extensions
{
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, Func<T, bool> shouldChunk)
    {
        var chunk = new LinkedList<T>();
        foreach (var line in source)
        {
            if (shouldChunk(line))
            {
                yield return new List<T>(chunk);
                chunk.Clear();
                continue;
            }
            
            chunk.AddLast(line);
        }
    }
}