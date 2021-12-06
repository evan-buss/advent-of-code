using Day5;

var lines = File.ReadAllLines("input.txt").Select(Line.Parse).ToArray();

Part1();
Part2();

void Part1()
{
    var map = new Map();
    foreach (var line in lines) map.Plot(line, diag: false);
    Console.WriteLine($"Part 1: {map.OverlapCount}");
    map.RenderBitmap($"part1_{DateTime.Now:hh_mm_ss}.bmp", 1000, 1000);
}

void Part2()
{
    var map = new Map();
    foreach (var line in lines) map.Plot(line, diag: true);
    Console.WriteLine($"Part 2: {map.OverlapCount}");
    map.RenderBitmap($"part2_{DateTime.Now:hh_mm_ss}.bmp", 1000, 1000);
}