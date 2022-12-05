using day5;

var sections = File.ReadAllText("part1.txt").Split("\n\n");
var layout = sections[0].Split("\n");
var instructions = sections[1].Split("\n");

var part1Dock = new Dock(new CrateMover9000());
var part2Dock = new Dock(new CrateMover9001());

part1Dock.Load(layout);
part2Dock.Load(layout);

foreach (var instruction in instructions)
{
    part1Dock.ProcessInstruction(instruction);
    part2Dock.ProcessInstruction(instruction);
}

Console.WriteLine($"Part 1: {part1Dock.GetTopology()}");
Console.WriteLine($"Part 2: {part2Dock.GetTopology()}");