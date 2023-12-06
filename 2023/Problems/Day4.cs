using Problems.Attributes;

namespace Problems;

[Day(4)]
public class Day4 : IProblem
{
    public record Game(int Card, int MatchCount, int Value)
    {
        public static Game Parse(string line)
        {
            var parts = line.Split(": ");
            var card = int.Parse(parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);
            var games = parts[1]
                .Split(" | ")
                .Select(
                    set =>
                        set.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToHashSet()
                )
                .ToArray();

            games[1].IntersectWith(games[0]);
            return new(card, games[1].Count, (int)Math.Pow(2, games[1].Count - 1));
        }
    }

    [SampleFile("day4.sample.txt", 13)]
    [PuzzleFile("day4.txt", Expected = 21558)]
    public int Part1(string[] input)
    {
        return input.Select(Game.Parse).Sum(x => x.Value);
    }

    [SampleFile("day4.sample.txt", 30)]
    [PuzzleFile("day4.txt", Expected = 10425665)]
    public int Part2(string[] input)
    {
        var games = input.Select(Game.Parse).ToArray();

        var cards = new int[games.Length];
        Array.Fill(cards, 1);

        foreach (var game in games)
        {
            for (var i = game.Card; i < game.Card + game.MatchCount; i++)
            {
                cards[i] += cards[game.Card - 1];
            }
        }

        return cards.Sum();

        // Naive (260ms)
        // var games = input.Select(Game.Parse).ToList();
        //
        // var cards = new Stack<Game>(games);
        //
        // var sum = 0;
        // while (cards.Count > 0)
        // {
        //     var game = cards.Pop();
        //     var winning = game.MatchingNumbers;
        //     for (var i = game.Card; i < game.Card + winning; i++)
        //     {
        //         cards.Push(games[i]);
        //     }
        //
        //     sum++;
        // }
        //
        // return sum;
    }
}
