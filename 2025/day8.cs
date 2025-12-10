var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided"),
};

var boxes = File.ReadLines(file).Select(line => Box.Parse(line)).ToArray();

var iterations = file.Contains("sample") ? 10 : 1000;
Console.WriteLine($"Part 1: {Part1(boxes, iterations)}");
Console.WriteLine($"Part 2: {Part2(boxes)}");

static long Part1(Box[] boxes, int iterations)
{
    var pq = BuildDistancePriorityQueue(boxes);
    var circuits = new UnionFind<Box>();

    for (int i = 0; i < iterations; i++)
    {
        var (item1, item2) = pq.Dequeue();
        circuits.Union(item1, item2);
    }

    return circuits
        .GetAllSets()
        .OrderByDescending(x => x.Value.Count)
        .Take(3)
        .Aggregate(1L, (acc, curr) => acc * curr.Value.Count);
}

static long Part2(Box[] boxes)
{
    var pq = BuildDistancePriorityQueue(boxes);
    var circuits = new UnionFind<Box>();

    while (pq.Count > 0)
    {
        var (item1, item2) = pq.Dequeue();
        circuits.Union(item1, item2);

        if (circuits.ElementCount == boxes.Length && circuits.SetCount == 1)
        {
            return (long)item1.X * item2.X;
        }
    }

    return 0;
}

static PriorityQueue<(Box, Box), double> BuildDistancePriorityQueue(Box[] boxes)
{
    var pq = new PriorityQueue<(Box, Box), double>();

    for (int i = 0; i < boxes.Length; i++)
    {
        for (int j = i + 1; j < boxes.Length; j++)
        {
            var distance = boxes[i].DistanceTo(boxes[j]);
            pq.Enqueue((boxes[i], boxes[j]), distance);
        }
    }

    return pq;
}

readonly record struct Box(int X, int Y, int Z)
{
    public static Box Parse(ReadOnlySpan<char> input)
    {
        var enumerator = input.Split(',');
        enumerator.MoveNext();
        var x = int.Parse(input[enumerator.Current]);
        enumerator.MoveNext();
        var y = int.Parse(input[enumerator.Current]);
        enumerator.MoveNext();
        var z = int.Parse(input[enumerator.Current]);

        return new(x, y, z);
    }

    public double DistanceTo(Box box) =>
        Math.Sqrt(Math.Pow(box.X - X, 2) + Math.Pow(box.Y - Y, 2) + Math.Pow(box.Z - Z, 2));
}

public class UnionFind<T>
    where T : notnull
{
    private readonly Dictionary<T, T> parent = [];
    private readonly Dictionary<T, int> rank = [];
    private int setCount = 0;

    public void Add(T item)
    {
        if (parent.TryAdd(item, item)) // parent pointer (self = root)
        {
            rank[item] = 0; // tree height (approximate)
            setCount++;
        }
    }

    private T Find(T item)
    {
        if (!parent.TryGetValue(item, out T? found))
            throw new ArgumentException("Item not found in UnionFind.");

        // Path compression
        if (!found.Equals(item))
            parent[item] = Find(found);

        return parent[item];
    }

    public void Union(T a, T b)
    {
        Add(a);
        Add(b);

        T rootA = Find(a);
        T rootB = Find(b);

        if (rootA.Equals(rootB))
            return;

        // Union by rank
        if (rank[rootA] < rank[rootB])
        {
            parent[rootA] = rootB;
        }
        else if (rank[rootA] > rank[rootB])
        {
            parent[rootB] = rootA;
        }
        else
        {
            parent[rootB] = rootA;
            rank[rootA]++;
        }

        setCount--;
    }

    public Dictionary<T, List<T>> GetAllSets()
    {
        var result = new Dictionary<T, List<T>>();

        foreach (var item in parent.Keys)
        {
            T root = Find(item);

            if (!result.ContainsKey(root))
                result[root] = [];

            result[root].Add(item);
        }

        return result;
    }

    // Number of disjoint sets remaining
    public int SetCount => setCount;
    public int ElementCount => parent.Count;
}
