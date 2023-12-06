﻿using Spectre.Console;

namespace Problems;

public class ProblemRunnerTerminal<TAssembly> : ProblemRunner<TAssembly>
{
    public ProblemRunnerTerminal(ProblemRunnerMode mode, int? day = null)
        : base(mode, day)
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        AnsiConsole.MarkupLine($"[rapidblink cyan]{_mode}[/] Mode");

        var table = new Table()
            .LeftAligned()
            .ShowFooters()
            .RoundedBorder()
            .AddColumns("Day", "Part", "Status", "Answer", "Duration");

        table.Columns[0].RightAligned();
        table.Columns[1].RightAligned();
        table.Columns[3].RightAligned();
        table.Columns[4].RightAligned();

        var totalDuration = TimeSpan.Zero;

        await AnsiConsole
            .Live(table)
            .StartAsync(async ctx =>
            {
                table.Columns[0].Footer("Totals");
                var correct = 0;
                var total = 0;
                await foreach (var update in RunAsync(cancellationToken))
                {
                    totalDuration += update.Duration;
                    
                    table.AddRow(
                        update.Day.ToString(),
                        update.Part.ToString(),
                        GetStatusLine(update.Status),
                        update.Answer.ToString(),
                        FormatTimeSpan(update.Duration)
                    );

                    if (update.Status == Status.Correct)
                    {
                        correct++;
                    }

                    total++;

                    table.Columns[2].Footer($"{correct}/{total}").RightAligned();
                    table.Columns[4].Footer(FormatTimeSpan(totalDuration));
                }
            });
    }

    private static string GetStatusLine(Status status)
    {
        return status switch
        {
            Status.Correct => "[green]Correct[/]",
            Status.Incorrect => "[red]Incorrect[/]",
            Status.Unknown => "[white]Unknown[/]"
        };
    }

    private static string FormatTimeSpan(TimeSpan duration)
    {
        return $"{duration.TotalMilliseconds:F}ms";
    }
}
