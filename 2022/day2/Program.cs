
using System.Diagnostics;

var part1 = File.ReadLines("part1.txt")
    .Select(GetValue)
    .Sum();

Console.WriteLine($"Part 1: {part1}");

int GetValue(string line)
{
    var parts = line.Split(" ");
    var opponent = GetMoveValue(parts[0]);
    var mine = GetMoveValue(parts[1]);
    var score = GetScore(opponent, mine);

    return (int) mine + score;
}

int GetScore(Move opponent, Move mine)
{
    if (opponent == mine) return 3;

    return mine switch
    {
        Move.Rock when opponent == Move.Paper => 0,
        Move.Rock when opponent == Move.Scissor => 6,
        Move.Paper when opponent == Move.Scissor => 0,
        Move.Paper when opponent == Move.Rock => 6,
        Move.Scissor when opponent == Move.Rock => 0,
        Move.Scissor when opponent == Move.Paper => 6,
    };
}

Move GetMoveValue(string move)
{
    return move switch
    {
        "A" or "X" => Move.Rock,
        "B" or "Y" => Move.Paper,
        "C" or "Z" => Move.Scissor
    };
}

enum Move
{
    Rock = 1,
    Paper = 2,
    Scissor = 3
}