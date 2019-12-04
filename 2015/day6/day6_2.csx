using System.Text.RegularExpressions;
var lines = File.ReadAllLines("input.txt");
var regex = new Regex(
    @"^(turn off|toggle|turn on) (.+),(.+) through (.+),(.+)",
    RegexOptions.None, TimeSpan.FromMilliseconds(150));

int[,] lights = new int[1000, 1000];

foreach (var line in lines)
{
    languageParser(line);
}

void languageParser(string line)
{
    int x1 = 0, x2 = 0, y1 = 0, y2 = 0;

    var data = regex.Match(line);
    x1 = Int32.Parse(data.Groups[2].Value);
    y1 = Int32.Parse(data.Groups[3].Value);
    x2 = Int32.Parse(data.Groups[4].Value);
    y2 = Int32.Parse(data.Groups[5].Value);

    // Console.WriteLine("{0} {1} to {2} {3}", x1, y1, x2, y2);
    switch (data.Groups[1].Value)
    {
        case "toggle":
            // Console.WriteLine("toggle");
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    lights[x, y] += 2;
                }
            }
            break;
        case "turn off":
            // Console.WriteLine("off");
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    if (lights[x, y] > 0) lights[x, y]--;
                }
            }
            break;
        case "turn on":
            // Console.WriteLine("on");
            for (int x = x1; x <= x2; x++)
            {
                for (int y = y1; y <= y2; y++)
                {
                    lights[x, y]++;
                }
            }
            break;
    }
}

var total = 0;
for (int x = 0; x < 1000; x++)
{
    for (int y = 0; y < 1000; y++)
    {
        total += lights[x, y];
    }
}

Console.WriteLine(total);