var lines = File.ReadAllLines("input.txt");

// (x,y) -> cross ('a' = "a crossed", 'b' = "b crossed", 'c' = "both crossed")
// Char codes give a way to tell if the wire has crossed itself. We don't count those
var record = new Dictionary<(int, int), char>();

Mapper(lines[0], 'a');
Mapper(lines[1], 'b');

void Mapper(string path, char id)
{
    int x = 0, y = 0;
    var commands = path.Split(",");

    foreach (var command in commands)
    {
        for (int i = 0; i < Int32.Parse(command.Substring(1)); i++)
        {
            switch (command[0])
            {
                case 'R': x--; break;
                case 'L': x++; break;
                case 'U': y++; break;
                case 'D': y--; break;
                default: break;
            };
            char crosses;
            record.TryGetValue((x, y), out crosses);
            // Don't consider wire crossing itself 
            record[(x, y)] = crosses != '\0' && crosses != id ? 'c' : id;
        }
    }
}

var minCross = record
    .Where(kvp => kvp.Value == 'c') // Get crossed intersections
    .Select(kvp => Math.Abs(kvp.Key.Item1) + Math.Abs(kvp.Key.Item2)) // Add the x and y distances
    .ToArray()
    .Min();

Console.WriteLine(minCross);
