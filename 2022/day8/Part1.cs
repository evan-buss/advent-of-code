namespace day8;

[Flags]
public enum Visibility
{
    North = 1,
    East = 2,
    South = 4,
    West = 8
}

public class Part1
{
    public void Do(int[,] forest)
    {
        var visible = new Visibility[forest.GetLength(0), forest.GetLength(1)];

        for (var y = 0; y < forest.GetLength(0); y++)
        {
            var rowMax = int.MinValue;
            for (var x = 0; x < forest.GetLength(1); x++)
            {
                if (rowMax < forest[y, x])
                {
                    visible[y, x] = Visibility.West;
                }

                rowMax = Math.Max(rowMax, forest[y, x]);
            }

            rowMax = int.MinValue;
            for (var x = forest.GetLength(1) - 1; x >= 0; x--)
            {
                if (rowMax < forest[y, x])
                {
                    visible[y, x] |= Visibility.East;
                }

                rowMax = Math.Max(rowMax, forest[y, x]);
            }
        }

        for (var x = 0; x < forest.GetLength(1); x++)
        {
            var colMax = int.MinValue;
            for (var y = 0; y < forest.GetLength(0); y++)
            {
                if (colMax < forest[y, x])
                {
                    visible[y, x] |= Visibility.North;
                }

                colMax = Math.Max(colMax, forest[y, x]);
            }

            colMax = int.MinValue;
            for (var y = forest.GetLength(0) - 1; y >= 0; y--)
            {
                if (colMax < forest[y, x])
                {
                    visible[y, x] |= Visibility.South;
                }

                colMax = Math.Max(colMax, forest[y, x]);
            }
        }

        var part1 = 0;
        for (var y = 0; y < visible.GetLength(0); y++)
        {
            for (var x = 0; x < visible.GetLength(1); x++)
            {
                if ((int)visible[y, x] > 0)
                {
                    part1++;
                }
            }
        }
        
        Console.WriteLine($"Part 1: {part1}");
    }
}