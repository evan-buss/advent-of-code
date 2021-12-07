var seed = File.ReadAllText("input.txt").Split(",").Select(int.Parse).ToArray();

Console.WriteLine($"Part 1: {Simulate(80)}");
Console.WriteLine($"Part 2: {Simulate(256)}");

long Simulate(int days)
{
  var population = new long[9];
  foreach (var fishy in seed) population[fishy]++;
  for (var day = 0; day < days; day++)
  {
    var babies = population[0];
    population = ShiftLeft(population);
    population[6] += babies;
  }
  return population.Sum();
}

static long[] ShiftLeft(long[] arr)
{
  var temp = arr[0];
  Array.Copy(arr, 1, arr, 0, arr.Length - 1);
  arr[^1] = temp;
  return arr;
}