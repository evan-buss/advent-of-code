// Part 2 - We need to get the shortest actual wire distnace. Not the taxicab distance.
// IDEA: Keep track of the total distance each time we place a position in the dictionary
var lines = File.ReadAllLines("input.txt");
var record = new Dictionary<(int, int), (char, int)>();

Mapper(lines[0], 'a');
Mapper(lines[1], 'b');

void Mapper(string path, char id)
{

    int x = 0, y = 0;
    var wireDistance = 0;
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
            wireDistance++;

            (char, int) cross;
            record.TryGetValue((x, y), out cross);
            // Add the current position to the dict and increment total distance to get there
            record[(x, y)] = cross.Item1 != '\0' && cross.Item1 != id
                ? ('c', wireDistance + cross.Item2)
                : (id, wireDistance + cross.Item2);
        }
    }
}

// Get all cross points, and retrieve the cross point with the lowest distance
var minCrossDistance = record
    .Where(kvp => kvp.Value.Item1 == 'c') // Get crossed locations
    .ToArray()
    .Select(kvp => kvp.Value.Item2) // Get the total distance at the cross
    .Min();
Console.WriteLine(minCrossDistance);
