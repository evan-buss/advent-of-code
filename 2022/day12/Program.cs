var maze = File.ReadLines("input.txt")
    .Select(x => x.ToCharArray())
    .ToArray();

Console.WriteLine($"Part 1: {Search(FindCharacter(maze, 'S').First())}");

var minPath = FindCharacter(maze, 'a').Select(Search).Min();
Console.WriteLine($"Part 2: {minPath}");

int Search(Point start)
{
    var nodes = new Queue<Point>();
    var visited = new bool[maze.Length, maze[0].Length];

    nodes.Enqueue(start with { Depth = 0 });
    visited[start.Y, start.X] = true;

    var dRow = new[] { -1, 0, 1, 0 };
    var dCol = new[] { 0, 1, 0, -1 };

    while (nodes.Count != 0)
    {
        var node = nodes.Dequeue();
        var (x, y, depth) = node;

        if (maze[y][x] == 'E')
        {
            return depth;
        }

        for (var i = 0; i < 4; i++)
        {
            var newNode = new Point(node.X + dCol[i], node.Y + dRow[i], depth + 1);
            if (!IsValid(maze, visited, node, newNode)) continue;
            nodes.Enqueue(newNode);
            visited[newNode.Y, newNode.X] = true;
        }
    }

    return int.MaxValue;
}

bool IsValid(IReadOnlyList<char[]> m, bool[,] visited, Point from, Point to)
{
    if (to.X < 0 || to.Y < 0 ||
        to.Y >= visited.GetLength(0) || to.X >= visited.GetLength(1) || 
        visited[to.Y, to.X])
    {
        return false;
    }

    // E has elevation of 'Z'
    var toChar = m[to.Y][to.X] == 'E' ? 'z' : m[to.Y][to.X];
    var fromChar = m[from.Y][from.X];

    if (toChar == 'S') return false;
    if (fromChar == 'S') return true;
    if (toChar < fromChar) return true;

    return toChar - fromChar <= 1;
}


IEnumerable<Point> FindCharacter(char[][] m, char c)
{
    for (var y = 0; y < m.Length; y++)
    {
        for (var x = 0; x < m[0].Length; x++)
        {
            if (m[y][x] == c)
            {
                yield return new Point(x, y);
            }
        }
    }
}

public record Point(int X, int Y, int Depth = 0);