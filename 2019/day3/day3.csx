var lines = File.ReadAllLines("input.txt");
var path1 = lines[0];
var path2 = lines[1];

// (x,y) -> cross ('a' = "a crossed", 'b' = "b crossed", 'c' = "both crossed")
// Char codes give a way to tell if the wire has crossed itself. We don't count those
var record = new Dictionary<(int, int), char>();

Mapper(path1, 'a');
Mapper(path2, 'b');

void Mapper(string path, char id)
{

    int x = 0, y = 0;
    var commands = path.Split(",");

    foreach (var dir in commands)
    {

        for (int i = 0; i < Int32.Parse(dir.Substring(1)); i++)
        {
            switch (dir[0])
            {
                case 'R': x--; break;
                case 'L': x++; break;
                case 'U': y++; break;
                case 'D': y--; break;
                default: Console.WriteLine("error"); break;
            };
            char crosses;
            record.TryGetValue((x, y), out crosses);
            // Don't consider wire crossing itself 
            record[(x, y)] = crosses != '\0' && crosses != id ? 'c' : id;
        }
    }
}

// Get all crosses and find the cross with the minimum distnance from orgin
var minCross = record
    .Where(kvp => kvp.Value == 'c')
    .Select(kvp => Math.Abs(kvp.Key.Item1) + Math.Abs(kvp.Key.Item2))
    .ToArray()
    .Min();

Console.WriteLine(minCross);
