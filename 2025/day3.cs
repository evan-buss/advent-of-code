using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;

var file = args switch
{
    [var path] => path,
    _ => Console.ReadLine() ?? throw new ArgumentException("No file path provided")
};

var lines = File.ReadLines(file);

// Console.WriteLine($"Part 1: {Parse(lines).Sum(bank => Solve(bank, 2))}");
Console.WriteLine($"Part 2: {Parse(lines).Sum(bank => Solve(bank, 12))}");

var Part1 = (int[] bank) => Solve(bank, 2);
var Part2 = (int[] bank) => Solve(bank, 12);

static long Solve(int[] bank, int limit)
{
    var bankSize = bank.Length;
    var cells = new int[bankSize];
    int found = 0;
    int start = 0;

    for (int target = 9; target >= 1 && found < limit; target--)
    {
        for (int i = start; i < bank.Length; i++)
        {
            if (bank[i] == target)
            {
                cells[i] = target;
                found++;

                if (i != bankSize - found) // not last
                {
                    start = Math.Min(i, i + 1);
                }
            }

            if (found == limit)
            {
                break;
            }
        }
    }

    var sb = new StringBuilder();
    for (int i = 0; i < cells.Length; i++)
    {
        if (cells[i] != 0)
            sb.Append(cells[i]);
    }

    Console.WriteLine(sb.ToString());

    return long.Parse(sb.ToString());
}

static IEnumerable<int[]> Parse(IEnumerable<string> lines)
{
    return lines.Select(row => row.Select(c => c - '0').ToArray());
}
