using System.Collections;

namespace day11;

public class Barrel : IEnumerable<Monkey>
{
    private readonly List<Monkey> _monkeys;

    public Barrel(IEnumerable<string> monkeys)
    {
        _monkeys = monkeys.Select(x => new Monkey(x)).ToList();
    }

    public IEnumerator<Monkey> GetEnumerator()
    {
        return _monkeys.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void PlayRound(int? modulo = null)
    {
        foreach (var monkey in _monkeys)
        foreach (var (monkeyIndex, item) in monkey.TakeTurn(modulo))
            _monkeys[monkeyIndex].Catch(item);
    }
}