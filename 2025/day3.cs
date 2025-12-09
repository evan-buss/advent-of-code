var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var banks = File.ReadAllLines(file);

Console.WriteLine($"Part 1: {banks.Sum(bank => Solve(bank, 2))}");
Console.WriteLine($"Part 2: {banks.Sum(bank => Solve(bank, 12))}");

static long Solve(ReadOnlySpan<char> bank, int targetLength)
{
    var result = 0L;
    var start = 0;

    for (var end = bank.Length - targetLength; end < bank.Length; end++)
    {
        var maxIndex = IndexOfMaxValue(bank, start, end);
        result = (result * 10) + (bank[maxIndex] - '0');
        start = maxIndex + 1;
    }

    return result;

    static int IndexOfMaxValue(ReadOnlySpan<char> bank, int start, int end)
    {
        var maxValue = char.MinValue;
        var maxIndex = -1;
        for (var i = start; i <= end; i++)
        {
            if (bank[i] > maxValue) // first largest wins
            {
                maxValue = bank[i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }
}
