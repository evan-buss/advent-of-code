using day8;

var input = File.ReadAllLines("input.txt");

var forest = new int[input.Length, input[0].Length];

for (var y = 0; y < input.Length; y++)
{
    var trees = input[y].Select(x => int.Parse(x.ToString())).ToArray();
    for (var x = 0; x < trees.Length; x++)
    {
        forest[y, x] = trees[x];
    }
}

Part1.Run(forest);
Part2.Run(forest);