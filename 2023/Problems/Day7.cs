using System.Diagnostics;
using System.Text.Json;
using Problems.Attributes;

namespace Problems;

[Day(7)]
public class Day7 : IProblem
{
    [PuzzleFile("day7.txt", Expected = 241_344_943)]
    [SampleFile("day7.sample.txt", 6_440)]
    public int Part1(string[] input)
    {
        return input
            .Select(x => new Hand(x))
            .Order()
            .Select((x, index) => (Hand: x, Rank: index + 1))
            .Aggregate(0, (acc, result) => acc + (result.Hand.Bid * result.Rank));
    }

    [PuzzleFile("day7.txt", Expected = 243_101_568)]
    [SampleFile("day7.sample.txt", 5905)]
    public int Part2(string[] input)
    {
        return input
            .Select(x => new Hand(x, wild: true))
            .Order()
            .Select((x, index) => (Hand: x, Rank: index + 1))
            .Aggregate(0, (acc, result) => acc + (result.Hand.Bid * result.Rank));
    }

    private enum HandType
    {
        FiveOfAKind = 1,
        FourOfAKind = 2,
        FullHouse = 3,
        ThreeOfAKind = 4,
        TwoPair = 5,
        OnePair = 6,
        HighCard = 7
    }

    private sealed class CardComparer(bool wild = false) : IComparer<char>
    {
        private static readonly char[] RegularCards =
        [
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'T',
            'J',
            'Q',
            'K',
            'A'
        ];

        private static readonly char[] WildcardCards =
        [
            'J',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            'T',
            'Q',
            'K',
            'A'
        ];

        public int Compare(char x, char y)
        {
            var cardRules = wild ? WildcardCards : RegularCards;
            var mine = Array.FindIndex(cardRules, c => c == x);
            var theirs = Array.FindIndex(cardRules, c => c == y);

            return mine.CompareTo(theirs);
        }
    }

    [DebuggerDisplay("Cards: {Cards} HandType: {HandType} Bid: {Bid}")]
    private class Hand : IComparable<Hand>
    {
        private string Cards { get; }
        public int Bid { get; }
        private HandType HandType { get; }
        private readonly bool _wildCardRules;

        private readonly CardComparer _cardComparer;

        public Hand(string line, bool wild = false)
        {
            _wildCardRules = wild;
            _cardComparer = new(_wildCardRules);

            var parts = line.Split(" ");
            Cards = parts[0];
            Bid = int.Parse(parts[1]);
            HandType = DetermineHand();
        }

        private HandType DetermineHand()
        {
            var groupedCards = Cards
                .GroupBy(x => x)
                .Select(x => (Card: x.Key, Count: x.Count()))
                .OrderByDescending(x => x.Count)
                .ThenByDescending(x => x.Card, _cardComparer)
                .ToList();

            if (_wildCardRules)
            {
                var jokerIndex = groupedCards.FindIndex(c => c.Card == 'J');
                var nonJokerIndex = groupedCards.FindIndex(c => c.Card != 'J');
                if (jokerIndex != -1)
                {
                    var jokers = groupedCards[jokerIndex];
                    if (nonJokerIndex != -1)
                    {
                        var groupedCard = groupedCards[nonJokerIndex];
                        groupedCard.Count += jokers.Count;
                        groupedCards[nonJokerIndex] = groupedCard;
                        groupedCards.RemoveAt(jokerIndex);
                    }
                    else
                    {
                        // If we don't have anything but jokers, make them all most valuable card.
                        var groupedCard = groupedCards[0];
                        groupedCard.Card = 'A';
                        groupedCards[0] = groupedCard;
                    }
                }
            }

            var counts = groupedCards.Select(x => x.Count).ToArray();

            return counts switch
            {
                [5] => HandType.FiveOfAKind,
                [4, ..] => HandType.FourOfAKind,
                [3, 2] => HandType.FullHouse,
                [3, _, _] => HandType.ThreeOfAKind,
                [2, 2, 1] => HandType.TwoPair,
                [2, _, _, _] => HandType.OnePair,
                [_, _, _, _, _] => HandType.HighCard,
                _
                    => throw new InvalidOperationException(
                        $"Cards {Cards} {JsonSerializer.Serialize(counts)}"
                    )
            };
        }

        public int CompareTo(Hand? other)
        {
            if (other is null)
                return -1;
            if (HandType != other.HandType)
                return other.HandType.CompareTo(HandType);
            if (Cards == other.Cards)
                return 0;

            for (var i = 0; i < 5; i++)
            {
                var compare = _cardComparer.Compare(Cards[i], other.Cards[i]);
                if (compare != 0)
                    return compare;
            }

            return 0;
        }
    }
}
