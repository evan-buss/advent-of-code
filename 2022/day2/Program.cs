using day2;

var part1 = File.ReadLines("part1.txt")
    .Sum(line =>
    {
        var parts = line.Split(" ");
        var opponent = Game.GetMove(parts[0]);
        var mine = Game.GetMove(parts[1]);
        var score = Game.GetScore(opponent, mine);

        return (int)mine + score;
    });

var part2 = File.ReadLines("part1.txt")
    .Sum(line =>
    {
        var parts = line.Split(" ");
        var opponent = Game.GetMove(parts[0]);
        var mine = Game.GetNeededMove(opponent, parts[1]);
        var score = Game.GetScore(opponent, mine);

        return (int)mine + score;
    });

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");