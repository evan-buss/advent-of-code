var input = File.ReadAllLines("input.txt");

Part1(input);
Part2(input);

void Part2(IReadOnlyList<string> instructions)
{
    Console.WriteLine("Part 2:");
    var register = 1;
    var cycle = 1;
    var currentInstruction = 0;

    var isDelayed = false;
    while (currentInstruction < instructions.Count)
    {
        var beam = cycle % 40 - 1;
        Console.Write(beam >= register - 1 && beam <= register + 1 ? "#" : ".");

        if (cycle % 40 == 0)
        {
            Console.WriteLine();
        }

        switch (instructions[currentInstruction].Split(' '))
        {
            case ["noop"]:
                currentInstruction++;
                break;
            case ["addx", var value]:
                if (isDelayed)
                {
                    register += int.Parse(value);
                    currentInstruction++;
                }

                isDelayed = !isDelayed;

                break;
        }

        cycle++;
    }
}

void Part1(IReadOnlyList<string> instructions)
{
    var register = 1;
    var cycle = 1;
    var currentInstruction = 0;

    var isDelayed = false;
    var nextCycle = 20;
    var total = 0;
    while (currentInstruction < instructions.Count)
    {
        switch (instructions[currentInstruction].Split(' '))
        {
            case ["noop"]:
                currentInstruction++;
                break;
            case ["addx", var value]:
                if (isDelayed)
                {
                    register += int.Parse(value);
                    currentInstruction++;
                }

                isDelayed = !isDelayed;

                break;
        }

        cycle++;

        if (cycle == nextCycle)
        {
            nextCycle += 40;
            total += cycle * register;
        }
    }

    Console.WriteLine($"Part 1: {total}");
}