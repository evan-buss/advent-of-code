var input = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();

Part1();
Part2();

void Part1()
{
  var minDistance = Enumerable.Range(input.Min(), input.Max())
    .Min(distance => input.Sum(crab => Math.Abs(crab - distance)));

  Console.WriteLine($"Part 1: {minDistance}");
}
void Part2()
{
  var minDistance = Enumerable.Range(input.Min(), input.Max())
     .Min(distance => input.Select(crab => Math.Abs(crab - distance)).Sum(x => x * (x + 1) / 2));

  Console.WriteLine($"Part 2: {minDistance}");
}