Part1();
Part2();

static void Part1()
{
  var lines = File.ReadAllLines("input.txt").ToList();

  var gammaStr = string.Empty;
  var epsilonStr = string.Empty;
  for (var i = 0; i < lines[0].Length; i++)
  {
    gammaStr += MoreOnesAtIndex(lines, i) ? "1" : "0";
    epsilonStr += MoreOnesAtIndex(lines, i) ? "0" : "1";
  }

  var gamma = Convert.ToUInt32(gammaStr, 2);
  var epsilon = Convert.ToUInt32(epsilonStr, 2);

  Console.WriteLine("Part 1: ");
  Console.WriteLine($"\tGamma: {gamma}");
  Console.WriteLine($"\tEpsilon: {epsilon * gamma}");
}

static void Part2()
{
  var lines = File.ReadAllLines("input.txt").ToList();
  var oxygen = lines;
  var carbonDioxide = lines;

  for (var currIndex = 0; currIndex < lines[0].Length; currIndex++)
  {
    if (oxygen.Count > 1)
    {
      oxygen = FilterBitAtIndex(oxygen, currIndex, MoreOnesAtIndex(oxygen, currIndex) ? 0 : 1);
    }

    if (carbonDioxide.Count > 1)
    {
      carbonDioxide = FilterBitAtIndex(carbonDioxide, currIndex, MoreOnesAtIndex(carbonDioxide, currIndex) ? 1 : 0);
    }
  }

  var oxy = Convert.ToUInt32(oxygen[0], 2);
  var co2 = Convert.ToUInt32(carbonDioxide[0], 2);
  Console.WriteLine($"Part 2: {oxy * co2}");
}

static bool MoreOnesAtIndex(List<string> list, int index)
{
  var ones = list.Count(x => x[index] == '1');
  var zeroes = list.Count - ones;
  return ones >= zeroes;
}

static List<string> FilterBitAtIndex(List<string> list, int index, int bit)
{
  return list.Where(x => char.GetNumericValue(x[index]) != bit).ToList();
}