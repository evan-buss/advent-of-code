namespace day8;

public static class Part1
{
    public static void Run(int[,] forest)
    {
        var visible = new bool[forest.GetLength(0), forest.GetLength(1)];

        for (var y = 0; y < forest.GetLength(0); y++)
        {
            var rowMax = int.MinValue;
            var reverseMax = int.MinValue;
            for (var x = 0; x < forest.GetLength(1); x++)
            {
                var reverseX = forest.GetLength(1) - 1 - x;
                
                visible[y, x] |= rowMax < forest[y, x];
                rowMax = Math.Max(rowMax, forest[y, x]);
                
                visible[y,  reverseX] |= reverseMax < forest[y,  reverseX];
                reverseMax = Math.Max(reverseMax, forest[y, reverseX]);
            }
        }

        for (var x = 0; x < forest.GetLength(1); x++)
        {
            var colMax = int.MinValue;
            var reverseMax = int.MinValue;
            for (var y = 0; y < forest.GetLength(0); y++)
            {
                visible[y, x] |= colMax < forest[y, x];
                colMax = Math.Max(colMax, forest[y, x]);
             
                var reverseY = forest.GetLength(0) - 1 - y;
                
                visible[reverseY, x] |= reverseMax < forest[reverseY, x];
                reverseMax = Math.Max(reverseMax, forest[reverseY, x]);
            }
        }

        Console.WriteLine($"Part 1: {visible.OfType<bool>().Count(x => x)}");
    }
}