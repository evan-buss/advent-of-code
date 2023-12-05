using Spectre.Console;

namespace Problems;

public class ProblemRunnerTerminal<TAssembly> : ProblemRunner<TAssembly>
{
    public ProblemRunnerTerminal(ProblemRunnerMode mode, int? day = null) : base(mode, day)
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var table = new Table()
            .LeftAligned()
            .ShowFooters()
            .Expand()
            .AddColumns("Day", "Part", "Status", "Answer", "Duration");

        var totalDuration = TimeSpan.Zero;

        await AnsiConsole
            .Live(table)
            .StartAsync(async ctx =>
            {
                table.Columns[0].Footer("Totals");
                await foreach (var update in RunAsync(cancellationToken))
                {
                    totalDuration += update.Duration;
                    table.AddRow(
                        update.ClassName,
                        update.Part,
                        GetStatusLine(update.Status),
                        update.Answer.ToString(),
                        FormatTimeSpan(update.Duration)
                    );

                    table.Columns[4].Footer(FormatTimeSpan(totalDuration));
                }
            });
    }

    private static string GetStatusLine(Status status)
    {
        return Emoji.Replace(status switch
        {
            Status.Correct => ":check_mark_button:  Correct",
            Status.Incorrect => ":cross_mark:  Incorrect",
            Status.Unknown => ":white_question_mark:  Unknown :white_question_mark:"
        });
    }

    private static string FormatTimeSpan(TimeSpan duration)
    {
        return $"{duration.TotalMilliseconds:F}ms";
    }
}