using System.ComponentModel;

var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var lines = File.ReadAllLines(file).ToList();

Console.WriteLine($"Part 1: {Part1(lines)}");
Console.WriteLine($"Part 2: {Part2(lines)}");

static long Part1(List<string> lines)
{
    var rows = lines
        .Select(
            x =>
                x.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        )
        .ToArray();

    var equations = new List<Equation>();
    foreach (var symbol in rows[^1])
    {
        equations.Add(new Equation([], Equation.ParseSymbol(char.Parse(symbol))));
    }
    foreach (var numbers in rows.SkipLast(1))
    {
        foreach (var (col, number) in numbers.Index())
        {
            equations[col] = equations[col].WithNumber(number);
        }
    }

    return equations.Sum(x => x.Calculate());
}

static long Part2(List<string> lines)
{
    var equations = new List<Equation>();
    List<int> numbers = [];
    for (var x = lines[0].Length - 1; x >= 0; x--)
    {
        var number = 0;
        for (var y = 0; y < lines.Count - 1; y++)
        {
            if (char.IsWhiteSpace(lines[y][x]))
            {
                continue;
            }

            number = (number * 10) + (lines[y][x] - '0');
        }
        numbers.Add(number);

        if (lines[^1][x] != ' ')
        {
            equations.Add(new Equation([.. numbers], Equation.ParseSymbol(lines[^1][x])));
            numbers.Clear();
            x--;
        }
    }

    return equations.Sum(x => x.Calculate());
}

enum Symbol
{
    Add,
    Multiply
}

readonly record struct Equation(long[] Numbers, Symbol Symbol)
{
    public Equation WithNumber(string value)
    {
        return this with { Numbers = [.. Numbers, long.Parse(value)] };
    }

    public static Symbol ParseSymbol(char c)
    {
        return c switch
        {
            '+' => Symbol.Add,
            '*' => Symbol.Multiply,
            _ => throw new InvalidEnumArgumentException()
        };
    }

    public long Calculate()
    {
        return Symbol switch
        {
            Symbol.Add => Numbers.Aggregate(0L, (curr, acc) => curr + acc),
            Symbol.Multiply => Numbers.Aggregate(1L, (curr, acc) => curr * acc),
            _ => throw new InvalidEnumArgumentException()
        };
    }
}
