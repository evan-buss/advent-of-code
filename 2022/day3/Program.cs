var part1 = File.ReadLines("part1.txt")
    .Select(x => x[..(x.Length / 2)].Intersect(x[(x.Length / 2)..]).First())
    .Sum(c => char.IsUpper(c) ? c - 'A' + 27 : c - 'a' + 1);

var part2 = File.ReadLines("part1.txt")
    .Chunk(3)
    .Select(x => x[0].Intersect(x[1]).Intersect(x[2]).First())
    .Sum(c => char.IsUpper(c) ? c - 'A' + 27 : c - 'a' + 1);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");