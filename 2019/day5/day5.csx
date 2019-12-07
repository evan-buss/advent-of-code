var ops = File.ReadAllText("input.txt").Split(",").Select(Int32.Parse).ToArray();

// Not fun :(
int RunComputer(int noun, int verb)
{
    int input = 0;
    var output = new List<int>();
    int[] copy = new int[ops.Length];
    ops.CopyTo(copy, 0);
    copy[1] = noun;
    copy[2] = verb;
    int i = 0;
    while (i < copy.Length)
    {
        if (copy[i] == 1)
        {
            copy[copy[i + 3]] = copy[copy[i + 1]] + copy[copy[i + 2]];
        }
        else if (copy[i] == 2)
        {
            copy[copy[i + 3]] = copy[copy[i + 1]] * copy[copy[i + 2]];
        }
        else if (copy[i] == 3)
        {
            copy[i + 1] = input;
        }
        else if (copy[i] == 4)
        {
            output.Add(copy[i + 1]);
        }
        else if (copy[i] == 99)
        {
            return copy[0];
        }
        i++;
    }
    return copy[0];
}