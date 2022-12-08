namespace day7;

public static class FileSystem
{
    public static INode Parse(IEnumerable<string> lines)
    {
        INode root = new Directory("/", null);
        var path = root;

        foreach (var line in lines)
        {
            if (line.StartsWith("$ cd"))
            {
                if (line.Contains(".."))
                {
                    path = path?.Parent;
                    continue;
                }

                var folder = line["$ cd ".Length..];
                var dir = path?.Children.First(x => x is Directory d && d.Name == folder);
                path = dir;
            }
            else if (line.StartsWith("dir"))
            {
                path?.Children.Add(new Directory(line["dir ".Length..], path));
            }
            else if (char.IsDigit(line[0]))
            {
                var parts = line.Split(' ');
                path?.Children.Add(new File(parts[1], int.Parse(parts[0]), path));
            }
        }

        return root;
    }
}

public interface INode
{
    public string Name { get; }
    public INode? Parent { get; }
    public List<INode> Children { get; }
}

public class File : INode
{
    public string Name { get; }
    public int Size { get; }
    public INode? Parent { get; set; }
    public List<INode> Children { get; set; } = new();

    public File(string name, int size, INode parent)
    {
        Name = name;
        Size = size;
        Parent = parent;
    }
}

public class Directory : INode
{
    public string Name { get; }
    public INode? Parent { get; set; }
    public List<INode> Children { get; set; } = new();

    public Directory(string name, INode? parent)
    {
        Name = name;
        Parent = parent;
    }
}