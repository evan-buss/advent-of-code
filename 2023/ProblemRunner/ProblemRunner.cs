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

public record ProblemPartResult(string ClassName, string Part, Status Status, TimeSpan Duration, int Answer);

public class ProblemRunner<TAssemblyType>(ProblemRunnerMode mode, int? day = null)
{
    private readonly List<IProblem> _problems = FindProblems(day);

    protected async IAsyncEnumerable<ProblemPartResult> RunAsync(
        CancellationToken cancellationToken = default
    )
    {
        foreach (var problem in _problems)
        {
            var result = await RunPart(problem, true, cancellationToken);
            if (result is not null)
                yield return result;

            result = await RunPart(problem, false, cancellationToken);
            if (result is not null)
                yield return result;
        }
    }

    private async Task<ProblemPartResult?> RunPart(
        IProblem problem,
        bool isPart1,
        CancellationToken cancellationToken = default
    )
    {
        var className = problem.GetType().Name;
        var methodName = isPart1 ? nameof(IProblem.Part1) : nameof(IProblem.Part2);
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
        var answer = isPart1 ? problem.Part1(lines) : problem.Part2(lines);
        var elapsed = Stopwatch.GetElapsedTime(start);

        return assertions.Expected switch
        {
            > 0 when answer != assertions.Expected
                => new(className, methodName, Status.Incorrect, elapsed, answer),
            > 0 when answer == assertions.Expected
                => new(className, methodName, Status.Correct, elapsed, answer),
            _ => new(className, methodName, Status.Unknown, elapsed, answer)
        };
    }

    private static List<IProblem> FindProblems(int? day = null)
    {
        var problems = typeof(TAssemblyType).Assembly
            .GetTypes()
            .Where(x => x.IsAssignableTo(typeof(IProblem)))
            .Select(
                x => (Type: x, DayAttribute: x.GetCustomAttributes<DayAttribute>().FirstOrDefault())
            )
            .Where(x => day is null || x.DayAttribute?.Day == day)
            .OrderBy(x => x.DayAttribute?.Day)
            .Select(x => (IProblem)Activator.CreateInstance(x.Type)!)
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
