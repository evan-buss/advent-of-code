using System.Diagnostics;
using System.Reflection;
using Problems.Attributes;

namespace Problems;

public enum ProblemRunnerMode
{
    Sample,
    Puzzle
}

public enum Status
{
    Correct,
    Incorrect,
    Unknown
}

public record ProblemPartResult(int Day, int Part, Status Status, TimeSpan Duration, int Answer);

public class ProblemRunner<TAssemblyType>(ProblemRunnerMode mode, int? day = null)
{
    private readonly List<(IProblem Problem, int Day)> _problems = FindProblems(day);
    protected readonly ProblemRunnerMode _mode = mode;

    protected async IAsyncEnumerable<ProblemPartResult> RunAsync(
        CancellationToken cancellationToken = default
    )
    {
        foreach (var (problem, day) in _problems)
        {
            var result = await RunPart(problem, day, 1, cancellationToken);
            if (result is not null)
                yield return result;

            result = await RunPart(problem, day, 2, cancellationToken);
            if (result is not null)
                yield return result;
        }
    }

    private async Task<ProblemPartResult?> RunPart(
        IProblem problem,
        int day,
        int part,
        CancellationToken cancellationToken = default
    )
    {
        var className = problem.GetType().Name;
        var methodName = part == 1 ? nameof(IProblem.Part1) : nameof(IProblem.Part2);
        var assertions = GetFileInputForMethod(problem.GetType().GetMethod(methodName), mode);

        // For example if we are on part 1, we don't have any data file or expected result for part 2, so don't run it.
        if (assertions is null)
        {
            return null;
        }

        var lines = await File.ReadAllLinesAsync(
            Path.Join("Inputs", assertions.Filename),
            cancellationToken
        );

        var start = Stopwatch.GetTimestamp();
        var answer = part == 1 ? problem.Part1(lines) : problem.Part2(lines);
        var elapsed = Stopwatch.GetElapsedTime(start);

        return assertions.Expected switch
        {
            > 0 when answer != assertions.Expected
                => new(day, part, Status.Incorrect, elapsed, answer),
            > 0 when answer == assertions.Expected
                => new(day, part, Status.Correct, elapsed, answer),
            _ => new(day, part, Status.Unknown, elapsed, answer)
        };
    }

    private static List<(IProblem, int)> FindProblems(int? day = null)
    {
        var problems = typeof(TAssemblyType).Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IProblem)))
            .Select(
                x => (Type: x, DayAttribute: x.GetCustomAttributes<DayAttribute>().FirstOrDefault())
            )
            .Where(x => x.DayAttribute is not null)
            .Where(x => day is null || x.DayAttribute!.Day == day)
            .OrderBy(x => x.DayAttribute?.Day)
            .Select(x => ((IProblem)Activator.CreateInstance(x.Type)!, x.DayAttribute!.Day))
            .ToList();
        return problems;
    }

    private static BaseFileAttribute? GetFileInputForMethod(
        MemberInfo? methodInfo,
        ProblemRunnerMode mode
    )
    {
        ArgumentNullException.ThrowIfNull(methodInfo);

        var attributes = methodInfo.GetCustomAttributes<BaseFileAttribute>().ToList();
        if (attributes.Count > 2)
        {
            throw new InvalidOperationException("Methods cannot have more than 2 file attributes");
        }

        var attributeType = mode switch
        {
            ProblemRunnerMode.Sample => typeof(SampleFileAttribute),
            ProblemRunnerMode.Puzzle => typeof(PuzzleFileAttribute)
        };

        return attributes.Find(x => x.GetType() == attributeType);
    }
}
