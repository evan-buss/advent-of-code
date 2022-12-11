using System.Diagnostics;

namespace day11;

public class Monkey
{
    private readonly int _falseMonkeyIndex;
    private readonly List<long> _items;
    private readonly string[] _operation;
    private readonly int _trueMonkeyIndex;

    public Monkey(string line)
    {
        var rows = line.Split("\n").Select(x => x.Trim()).ToArray();
        _items = rows[1]["Starting items: ".Length..].Split(", ").Select(long.Parse).ToList();

        _operation = rows[2]["Operation: new = ".Length..].Split(" ");

        Divisor = int.Parse(rows[3]["Test: divisible by ".Length..]);

        _trueMonkeyIndex = int.Parse(rows[4]["If true: throw to monkey ".Length..]);
        _falseMonkeyIndex = int.Parse(rows[5]["If false: throw to monkey ".Length..]);
    }

    public long Inspections { get; private set; }
    public int Divisor { get; }

    public IEnumerable<(int MonkeyIndex, long Item)> TakeTurn(int? modulo = null)
    {
        foreach (var item in _items)
        {
            var worryLevel = _operation switch
            {
                [_, "*", "old"] => item * item,
                [_, "*", var value] => item * long.Parse(value),
                [_, "+", "old"] => item + item,
                [_, "+", var value] => item + long.Parse(value),
                _ => throw new UnreachableException()
            };

            worryLevel = modulo is null
                ? worryLevel / 3
                : worryLevel % modulo.Value;

            yield return (
                worryLevel % Divisor == 0
                    ? _trueMonkeyIndex
                    : _falseMonkeyIndex,
                worryLevel
            );
        }

        Inspections += _items.Count;
        _items.Clear();
    }

    public void Catch(long item)
    {
        _items.Add(item);
    }
}