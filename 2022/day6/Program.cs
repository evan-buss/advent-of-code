var input = File.ReadAllText("input.txt");

Console.WriteLine($"Part 1: {SignalProcessor(input)}");
Console.WriteLine($"Part 2: {SignalProcessor(input, 14)}");

int SignalProcessor(string stream, int size = 4)
{
    for (var i = 0; i < stream.Length - size; i++)
    {
        if (new HashSet<char>(stream[i..(i + size)]).Count == size) return i + size;
    }

    return -1;
}