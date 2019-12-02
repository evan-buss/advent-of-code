// ARGS
// 0 -> input file name
// 1 -> anything here will trigger part two to run 

if (Args.Count < 1)
{
    Console.WriteLine("Usage: day2.csx [input file] <part2>");
    Environment.Exit(-1);
}

var ops = File.ReadAllText(Args[0]).Split(",").Select(Int32.Parse).ToArray();

// PART 1
if (Args.Count() == 1)
{
    Console.WriteLine("Part 1 Solution: {0}", RunComputer(12, 2));
}
// PART 2
else
{
    for (var noun = 0; noun <= 99; noun++)
    {
        for (var verb = 0; verb <= 99; verb++)
        {
            // Loop until we find the magic number
            if (RunComputer(noun, verb) == 19690720)
            {
                Console.WriteLine("Part 2 Solution: {0} {1} = {2}", noun, verb, 100 * noun + verb);
                return;
            }
        }
    }
}

// Run the computer with the given noun and verb values
// Return the first value of the final answer
int RunComputer(int noun, int verb)
{
    int[] copy = new int[ops.Length];
    ops.CopyTo(copy, 0);
    copy[1] = noun;
    copy[2] = verb;
    for (var i = 0; i < copy.Count(); i += 4)
    {
        if (copy[i] == 1)
        {
            copy[copy[i + 3]] = copy[copy[i + 1]] + copy[copy[i + 2]];
        }
        else if (copy[i] == 2)
        {
            copy[copy[i + 3]] = copy[copy[i + 1]] * copy[copy[i + 2]];
        }
        else if (copy[i] == 99)
        {
            return copy[0];
        }
    }
    return copy[0];
}