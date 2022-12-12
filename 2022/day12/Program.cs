var maze = File.ReadLines("input.sample.txt")
    .Select(x => x.ToCharArray())
    .ToArray();

Search(0, 0);

bool IsValid(IReadOnlyList<char[]> m, bool[,] visited, Point from, Point to)
{
    if (to.X < 0 || to.Y < 0 ||
        to.Y >= visited.GetLength(0) ||
        to.X >= visited.GetLength(1))
    {
        return false;
    }

    // if (visited[to.Y, to.X])
    // {
    //     return false;
    // }

    var toChar = m[to.Y][to.X];
    var fromChar = m[from.Y][from.X];

    if (toChar == 'S') return false;
    if (fromChar == 'S') return true;

    if (toChar < fromChar) return true;

    if (toChar - fromChar <= 1) return true;


    return false;
}

void Search(int startX, int startY)
{
    var nodes = new Queue<Point>();
    var visited = new bool[maze.Length, maze[0].Length];

    nodes.Enqueue(new Point(startX, startY));
    visited[startY, startX] = true;

    while (nodes.Count != 0)
    {
        var (x, y) = nodes.Dequeue();

        Console.WriteLine($"{x}{y}");

        if (maze[y][x] == 'E')
        {
            Console.WriteLine($"FOUND IT {x}{y}");
            return;
        }

        // Left
        if (IsValid(maze, visited, new Point(x, y), new Point(x - 1, y)))
        {
            nodes.Enqueue(new Point(x - 1, y));
            visited[y, x - 1] = true;
        }

        // Right
        if (IsValid(maze, visited, new Point(x, y), new Point(x + 1, y)))
        {
            nodes.Enqueue(new Point(x + 1, y));
            visited[y, x + 1] = true;
        }

        // Up
        if (IsValid(maze, visited, new Point(x, y), new Point(x, y - 1)))
        {
            nodes.Enqueue(new Point(x, y - 1));
            visited[y - 1, x] = true;
        }

        if (IsValid(maze, visited, new Point(x, y), new Point(x, y + 1)))
        {
            nodes.Enqueue(new Point(x, y + 1));
            visited[y + 1, x] = true;
        }
    }
}

public record Point(int X, int Y);