namespace day5;

public interface ICrane
{
    void ProcessInstruction(List<Stack<char>> dockLayout, string instruction);
}

public class CrateMover9000 : ICrane
{
    public void ProcessInstruction(List<Stack<char>> dockLayout, string instruction)
    {
        if (string.IsNullOrWhiteSpace(instruction)) return;
        
        var moves = instruction.Split(
                new[] { "move", "from", "to" },
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            )
            .Select(int.Parse)
            .ToArray();

        var iterations = moves[0];
        var from = moves[1];
        var to = moves[2];

        for (var i = 0; i < iterations; i++)
        {
            var fromValue = dockLayout[from - 1].Pop();
            dockLayout[to - 1].Push(fromValue);
        }
    }
}

public class CrateMover9001 : ICrane
{
    public void ProcessInstruction(List<Stack<char>> dockLayout, string instruction)
    {
        if (string.IsNullOrWhiteSpace(instruction)) return;
        
        var moves = instruction.Split(
                new[] { "move", "from", "to" },
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
            )
            .Select(int.Parse)
            .ToArray();

        var iterations = moves[0];
        var from = moves[1];
        var to = moves[2];
        
        var crates = new char[iterations];

        for (var i = 0; i < iterations; i++)
        {
            crates[^(i + 1)] = dockLayout[from - 1].Pop();
        }

        foreach (var crate in crates)
        {
            dockLayout[to - 1].Push(crate);
        }
    }
}