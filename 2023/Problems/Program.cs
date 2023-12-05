using Problems;

var mode = ProblemRunnerMode.Puzzle;
int? day = null;
if (args.Contains("sample"))
{
    mode = ProblemRunnerMode.Sample;
}

var runner = new ProblemRunnerTerminal<Day1>(mode, day);

await runner.StartAsync();
