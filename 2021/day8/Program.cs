var input = File.ReadAllLines("input.txt").Select(Entry.Parse).ToArray();

var map = new Dictionary<int, int>()
{
  [0] = 6,
  [1] = 2,
  [2] = 5,
  [3] = 5,
  [4] = 4,
  [5] = 5,
  [6] = 6,
  [7] = 3,
  [8] = 7,
  [9] = 6
};

Part1();
Part2();

void Part1()
{
  var uniques = new int[] { 2, 4, 3, 7 };
  var sum = input
    .SelectMany(entry => entry.Output)
    .Sum(x => uniques.Contains(x.Length) ? 1 : 0);

  Console.WriteLine($"Part 1: {sum}");
}

void Part2()
{

}

record Entry(string[] Patterns, string[] Output)
{
  public static Entry Parse(string text)
  {
    var sections = text.Split(" | ");
    return new Entry(
      sections[0].Split(" ").ToArray(),
      sections[1].Split(" ").ToArray()
    );
  }
}