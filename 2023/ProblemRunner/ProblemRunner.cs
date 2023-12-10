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
    Unknown,
    Skipped,
}

public record ProblemPartResult(int Day, int Part, Status Status, TimeSpan? Duration, int? Answer);

public class ProblemRunner<TAssemblyType>(ProblemRunnerMode mode, int? day = null)
{
    private readonly List<(IProblem Problem, int Day)> _problems = FindProblems(day);
    protected readonly ProblemRunnerMode Mode = mode;

    protected async IAsyncEnumerable<ProblemPartResult> RunAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        foreach (var (problem, problemDay) in _problems)
        {
            await foreach (var result in RunPart(problem, problemDay, 1, cancellationToken))
            {
                yield return result;
            }

            await foreach (var result in RunPart(problem, problemDay, 2, cancellationToken))
            {
                yield return result;
            }
        }
    }

    private async IAsyncEnumerable<ProblemPartResult> RunPart(
        IProblem problem,
        int day,
        int part,
        [EnumeratorCancellation] CancellationToken cancellationToken = default
    )
    {
        var methodName = part == 1 ? nameof(IProblem.Part1) : nameof(IProblem.Part2);
        var assertions = GetFileInputForMethod(problem.GetType().GetMethod(methodName), Mode);

        // For example if we are on part 1, we don't have any data file or expected result for part 2, so don't run it.
        if (assertions is null)
        {
            yield break;
        }

        if (assertions.Skip)
        {
            yield return new(day, part, Status.Skipped, null, null);
            yield break;
        }

        var lines = await File.ReadAllLinesAsync(
            Path.Join("Inputs", assertions.Filename),
            cancellationToken
        );

        var start = Stopwatch.GetTimestamp();
        var answer = part == 1 ? problem.Part1(lines) : problem.Part2(lines);
        var elapsed = Stopwatch.GetElapsedTime(start);

        yield return assertions.Expected switch
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
            .Select(x => ((IProblem) Activator.CreateInstance(x.Type)!, x.DayAttribute!.Day))
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
            ProblemRunnerMode.Puzzle => typeof(PuzzleFileAttribute),
            _ => throw new InvalidOperationException("Invalid ProblemRunnerMode")
        };

        return attributes.Find(x => x.GetType() == attributeType);
    }
}