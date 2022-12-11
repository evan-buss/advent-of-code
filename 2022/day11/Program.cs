using day11;

var input = File.ReadAllText("input.txt").Split("\n\n");

var barrel1 = new Barrel(input);
for (var i = 0; i < 20; i++) barrel1.PlayRound();
Console.WriteLine($"Part 1: {Solve(barrel1)}");


var barrel2 = new Barrel(input);
var modulo = barrel2.Select(x => x.Divisor).Aggregate((i, m) => m * i);
for (var i = 0; i < 10_000; i++) barrel2.PlayRound(modulo);
Console.WriteLine($"Part 2: {Solve(barrel2)}");


double Solve(Barrel b)
{
    return b.Select(x => x.Inspections)
        .OrderDescending()
        .Take(2)
        .Aggregate((i, count) => i * count);
}