using Problems.Attributes;

namespace Problems;

[Day(2)]
public class Day2 : IProblem
{
    public record Game(int Id, List<Round> Rounds)
    {
        public static Game Parse(string line)
        {
            var game = int.Parse(line.Split(": ")[0].Split(" ")[1]);
            var rounds = line.Split(": ")[1].Split("; ").Select(Round.Parse).ToList();

            return new(game, rounds);
        }
    }

    public record Round(int Red, int Green, int Blue)
    {
        public static Round Parse(string input)
        {
            return input
                .Split(", ")
                .Aggregate(
                    new Round(0, 0, 0),
                    (round, cube) =>
                    {
                        return cube.Split(" ") switch
                        {
                            [var count, "red"] => round with { Red = int.Parse(count) },
                            [var count, "blue"] => round with { Blue = int.Parse(count) },
                            [var count, "green"] => round with { Green = int.Parse(count) },
                            _
                                => throw new InvalidOperationException(
                                    $"Invalid input provided. Unable to parse: {input}"
                                )
                        };
                    }
                );
        }
    }

    [PuzzleFile("day2.txt", Expected = 2237)]
    [SampleFile("day2.sample.txt", 8)]
    public int Part1(string[] input)
    {
        const int red = 12;
        const int green = 13;
        const int blue = 14;

        return input
            .Select(Game.Parse)
            .Where(
                game =>
                    game.Rounds.TrueForAll(
                        round => round is { Red: <= red, Green: <= green, Blue: <= blue }
                    )
            )
            .Sum(game => game.Id);
    }

    [PuzzleFile("day2.txt", Expected = 66681)]
    [SampleFile("day2.sample.txt", 2286)]
    public int Part2(string[] input)
    {
        return input
            .Select(Game.Parse)
            .Select(game =>
            {
                var minRed = game.Rounds.MaxBy(x => x.Red)!.Red;
                var minGreen = game.Rounds.MaxBy(x => x.Green)!.Green;
                var minBlue = game.Rounds.MaxBy(x => x.Blue)!.Blue;

                return minRed * minGreen * minBlue;
            })
            .Sum();
    }
}
