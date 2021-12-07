var input = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();

Part1();
Part2();

void Part1()
{
  var min = input.Min();
  var max = input.Max();

  int minDistance = int.MaxValue;
  for (int i = min; i < max; i++)
  {
    var total = 0;
    foreach (var crab in input)
    {
      total += Math.Abs(crab - i);
    }
    minDistance = Math.Min(minDistance, total);
  }

  Console.WriteLine($"Part 1: {minDistance}");
}

void Part2()
{
  var min = input.Min();
  var max = input.Max();

  int minDistance = int.MaxValue;
  for (int i = min; i < max; i++)
  {
    var total = 0;
    foreach (var crab in input)
    {
      var distance = Math.Abs(crab - i);
      total += ((distance * distance) + distance) / 2;
    }
    minDistance = Math.Min(minDistance, total);
  }

  Console.WriteLine($"Part 2: {minDistance}");
}