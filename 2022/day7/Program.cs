using day7;
using Directory = day7.Directory;
using File = day7.File;

var root = FileSystem.Parse(System.IO.File.ReadAllText("input.sample.txt").Split("\n").Skip(1));

var directoryTotals = new List<(string Name, int Size)>();

int TraverseFileSystem(INode? node)
{
    var sum = 0;
    switch (node)
    {
        case null:
            return sum;
        case File f:
            sum += f.Size;
            break;
    }

    sum += node.Children.Sum(TraverseFileSystem);

    if (node is Directory)
    {
        directoryTotals.Add((node.Name, sum));
    }

    return sum;
}

var totalUsed = TraverseFileSystem(root);
var unused = 70_000_000 - totalUsed;
var need = 30_000_000 - unused;

Console.WriteLine($"Part 1: {directoryTotals.Where(x => x.Size <= 100_000).Sum(x => x.Size)}");
Console.WriteLine($"Part 2: {directoryTotals.Where(x => x.Size >= need).MinBy(x => x.Size).Size}");