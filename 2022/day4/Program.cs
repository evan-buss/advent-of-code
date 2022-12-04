using System.Diagnostics.CodeAnalysis;

var part1 = File.ReadLines("part1.txt")
    .Select(line => new Pair(line))
    .Count(x => x.FullyContains);

var part2 = File.ReadLines("part1.txt")
    .Select(line => new Pair(line))
    .Count(x => x.Overlaps);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");

public record Range
{
    public required int Start { get; init; }
    public required int End { get; init; }
    
    [SetsRequiredMembers]
    public Range(int start, int end)
    {
        Start = start;
        End = end;
    }
    
    [SetsRequiredMembers]
    public Range(string line)
    {
        var parts = line.Split("-").Select(int.Parse).ToArray();
        Start = parts[0];
        End = parts[1];
    }
}

public record Pair
{
    public required Range Section1 { get; init; }
    public required Range Section2 { get; init; }

    [SetsRequiredMembers]
    public Pair(Range section1, Range section2)
    {
        Section1 = section1;
        Section2 = section2;
    }
    
    [SetsRequiredMembers]
    public Pair(string line)
    {
        var parts = line.Split(",");
        Section1 = new Range(parts[0]);
        Section2 = new Range(parts[1]);
    }

    public bool FullyContains
        => Section1.Start >= Section2.Start && Section1.End <= Section2.End || // First is Contained in Second
           Section2.Start >= Section1.Start && Section2.End <= Section1.End; // Second is Contained in First

    public bool Overlaps => Math.Max(Section1.Start, Section2.Start) <= Math.Min(Section1.End, Section2.End);
}