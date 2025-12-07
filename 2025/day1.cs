var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var lines = File.ReadAllLines(file);

Console.WriteLine($"Part 1: {Dial(Part1(lines)).Count(x => x == 0)}");
Console.WriteLine($"Part 2: {Dial(Part2(lines)).Count(x => x == 0)}");

static IEnumerable<int> Dial(IEnumerable<int> rotations)
{
    int position = 50;
    foreach (var rotation in rotations)
    {
        position = ((position + rotation) % 100 + 100) % 100;
        yield return position;
    }
}

static IEnumerable<int> Part1(IEnumerable<string> lines)
{
    return lines.Select(line =>
    {
        var direction = line[0];
        var distance = int.Parse(line[1..]);
        return (direction == 'R' ? 1 : -1) * distance;
    });
}

static IEnumerable<int> Part2(IEnumerable<string> lines)
{
    return lines.SelectMany(line =>
    {
        var direction = line[0];
        var distance = int.Parse(line[1..]);
        return Enumerable.Range(0, distance).Select(x => direction == 'R' ? 1 : -1);
    });
}
