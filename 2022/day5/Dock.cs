using System.Text;

namespace day5;

public class Dock
{
    private readonly ICrane _crane;
    private readonly List<Stack<char>> _layout = new();

    public Dock(ICrane crane) => _crane = crane;

    public void Load(IReadOnlyList<string> layout)
    {
        for (var x = 0; x < layout[^1].Length; x++)
        {
            if (!char.IsDigit(layout[^1][x])) continue;

            var stack = new Stack<char>();
            for (var y = layout.Count - 2; y >= 0; y--)
            {
                var crate = layout[y][x];
                if (!char.IsWhiteSpace(crate))
                {
                    stack.Push(crate);
                }
            }

            _layout.Add(stack);
        }
    }

    public void ProcessInstruction(string instruction)
        => _crane.ProcessInstruction(_layout, instruction);

    public string GetTopology()
        => string.Join("", _layout.Select(x => x.Peek()));

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < _layout.Count; i++)
        {
            sb.Append($"Index: {i++}: ");
            sb.AppendJoin(" ", _layout[i].ToArray().Reverse());
            sb.AppendLine();
        }

        return sb.ToString();
    }
}