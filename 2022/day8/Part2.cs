namespace day8;

public class Part2
{
    public Part2(int[,] forest)
    {
        var maxView = int.MinValue;
        for (var y = 0; y < forest.GetLength(0); y++)
        {
            for (var x = 0; x < forest.GetLength(1); x++)
            {
                var vertical = CalculateVertical(forest, x, y);
                var horizontal = CalculateHorizontal(forest, x, y);
                var total = vertical * horizontal;
                maxView = Math.Max(maxView, total);
                // Console.WriteLine($"Position Y:{y} X:{x} -- Total: {total} Horiz: {horizontal} Vert: {vertical}");
            }
        }

        Console.WriteLine($"Part 2: {maxView}");
    }

    private static int CalculateVertical(int[,] forest, int x, int y)
    {
        // Go Up
        var up = 0;
        for (var i = y; i >= 0; i--)
        {
            var position = forest[i, x];
            if (i == y) continue;

            up++;
            if (position >= forest[y, x])
                break;
        }

        // Go Down
        var down = 0;
        for (var i = y; i < forest.GetLength(0); i++)
        {
            var position = forest[i, x];
            if (i == y) continue;

            down++;
            if (position >= forest[y, x])
                break;
        }

        return up * down;
    }

    private static int CalculateHorizontal(int[,] forest, int x, int y)
    {
        // Go Left
        var left = 0;
        for (var i = x; i >= 0; i--)
        {
            var position = forest[y, i];
            if (i == x) continue;

            left++;
            if (position >= forest[y, x])
                break;
        }

        var right = 0;
        // Go Right
        for (var i = x; i < forest.GetLength(1); i++)
        {
            var position = forest[y, i];
            if (i == x) continue;

            right++;
            if (position >= forest[y, x])
                break;
        }

        return left * right;
    }
}