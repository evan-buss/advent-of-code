// ARGS
// 0 -> input file name
// 1 -> anything here will trigger part two to run 

if (Args.Count < 1)
{
    Console.WriteLine("Usage: day2.csx [input file] <part2>");
    Environment.Exit(-1);
}

var line = File.ReadAllText(Args[0]);
var ops = line.Split(",").Select(Int32.Parse).ToArray();

// Add Operation
Action<int, int[]> add = (i, ops) =>
{
    ops[ops[i + 3]] = ops[ops[i + 1]] + ops[ops[i + 2]];
};

// Multiply operation
Action<int, int[]> mul = (i, ops) =>
{
    ops[ops[i + 3]] = ops[ops[i + 1]] * ops[ops[i + 2]];
};

// PART 1
if (Args.Count() == 1)
{
    // 1202 ALARM STATE MODIFICATION
    ops[1] = 12;
    ops[2] = 2;

    for (var i = 0; i < ops.Count(); i += 4)
    {
        // Read the opcode
        switch (ops[i])
        {
            case 1:
                add(i, ops);
                break;
            case 2:
                mul(i, ops);
                break;
            case 99:
                Console.WriteLine("EXIT OPCODE: {0}", ops[0]);
                Environment.Exit(-1);
                break;
            default:
                Console.WriteLine("ERROR: {0}", ops[0]);
                Environment.Exit(-1);
                break;
        }
    }
    Console.WriteLine("Part 1 Solution: {0}", ops[0]);
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
                Console.WriteLine("SUCCESS {0} {1} -> {2}", noun, verb, 100 * noun + verb);
                Environment.Exit(0);
            }
        }
    }
}

// Run the computer with the given noun and verb values return the first value of the final answer
int RunComputer(int noun, int verb)
{
    int[] copy = new int[ops.Length];
    ops.CopyTo(copy, 0);
    copy[1] = noun;
    copy[2] = verb;
    for (var i = 0; i < copy.Count(); i += 4)
    {
        // Read the opcode
        switch (copy[i])
        {
            case 1:
                add(i, copy);
                break;
            case 2:
                mul(i, copy);
                break;
            case 99:
                return copy[0];
            default:
                return copy[0];
        }
    }
    return copy[0];
}