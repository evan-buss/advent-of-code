var numbers = File.ReadAllLines("input.txt").Select(int.Parse).ToArray();
Console.WriteLine($"Part 1: {Compute(numbers)}");

var chunkSums = numbers.Chunk(3).Select(chunk => chunk.Sum());
Console.WriteLine($"Part 2: {Compute(Windowed(numbers))}");

static IEnumerable<int> Windowed(int[] numbers, int size = 3)
{
  for (var currGroupIndex = 0; currGroupIndex <= numbers.Length - size; currGroupIndex++)
  {
    yield return numbers[currGroupIndex..(currGroupIndex + size)].Sum();
  }
}

static int Compute(IEnumerable<int> numbers)
{
  var count = 0;
  var prev = numbers.First();
  foreach (var num in numbers)
  {
    if (num > prev) count++;
    prev = num;
  }

  return count;
}