// Part 2 - We need to get the shortest actual wire distnace. Not the taxicab distance.
// IDEA: Keep track of the total distance each time we place a position in the dictionary
var lines = File.ReadAllLines("input.txt");
var path1 = lines[0];
var path2 = lines[1];

var record = new Dictionary<(int, int), (char, int)>();

Mapper(path1, 'a');
Mapper(path2, 'b');

void Mapper(string path, char id)
{

    int x = 0, y = 0;
    var wireDistance = 0;
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
            (char, int) cross;
            wireDistance++;
            record.TryGetValue((x, y), out cross);
            // Don't consider wire crossing itself 
            // Add the existing distance from other wire to current 
            //  distance to get total distnace for both wires to reach the point
            record[(x, y)] = cross.Item1 != '\0' && cross.Item1 != id ? ('c', wireDistance + cross.Item2) : (id, wireDistance + cross.Item2);
        }
    }
}

// Get all cross points, and retrieve the cross point with the lowest distance
var minCrossDistance = record
    .Where(kvp => kvp.Value.Item1 == 'c')
    .ToArray()
    .Select(kvp => kvp.Value.Item2)
    .Min();
Console.WriteLine(minCrossDistance);
