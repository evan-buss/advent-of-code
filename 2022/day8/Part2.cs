namespace day8;

public static class Part2
{
    public static void Run(int[,] forest)
    {
        var maxView = int.MinValue;
        for (var y = 0; y < forest.GetLength(0); y++)
        {
            for (var x = 0; x < forest.GetLength(1); x++)
            {
                maxView = Math.Max(maxView, CalculateVertical(forest, x, y) * CalculateHorizontal(forest, x, y));
            }
        }

        Console.WriteLine($"Part 2: {maxView}");
    }

    private static int CalculateVertical(int[,] forest, int x, int y)
    {
        var up = 0;
        for (var i = y - 1; i >= 0; i--)
        {
            up++;
            if (forest[i, x] >= forest[y, x])
                break;
        }

        // Go Down
        var down = 0;
        for (var i = y + 1; i < forest.GetLength(0); i++)
        {
            down++;
            if (forest[i, x] >= forest[y, x])
                break;
        }

        return up * down;
    }

    private static int CalculateHorizontal(int[,] forest, int x, int y)
    {
        var left = 0;
        for (var i = x - 1; i >= 0; i--)
        {
            left++;
            if (forest[y, i] >= forest[y, x])
                break;
        }

        var right = 0;
        for (var i = x + 1; i < forest.GetLength(1); i++)
        {
            right++;
            if (forest[y, i] >= forest[y, x])
                break;
        }

        return left * right;
    }
}