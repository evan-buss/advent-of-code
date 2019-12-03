var vowels = new List<char> { 'a', 'e', 'i', 'o', 'u' };
var banned = new List<string> { "ab", "cd", "pq", "xy" };
var lines = File.ReadAllLines("input.txt")
    .Where(line =>
    {
        var total = 0;
        foreach (char c in line)
        {
            if (vowels.Contains(c))
            {
                total++;
            }
        }
        return total >= 3;
    }).Where(line =>
    {
        var prev = line[0];
        foreach (char c in line.Skip(1))
        {
            if (prev == c)
            {
                return true;
            }
            prev = c;
        }
        return false;
    }).Where(line =>
    {
        foreach (var ban in banned)
        {
            if (line.Contains(ban)) return false;
        }
        return true;
    }).Count();

Console.WriteLine(lines);