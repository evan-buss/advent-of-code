var directions = File.ReadAllText("input.txt").ToCharArray();
int x1 = 0, y1 = 0;
int x2 = 0, y2 = 0;

var visited = new Dictionary<(int, int), int>();
visited.Add((0, 0), 2);

bool turn = true;
foreach (var command in directions)
{
    var (x, y) = turn ?
        commandRouter(command, x1, y1) :
        commandRouter(command, x2, y2);

    int currentVisits;
    visited.TryGetValue((x, y), out currentVisits);
    visited[(x, y)] = currentVisits + 1;

    if (turn)
    {
        x1 = x;
        y1 = y;
    }
    else
    {
        x2 = x;
        y2 = y;
    }
    turn = !turn;
}

var houses = visited.Where(kvp => kvp.Value > 0).ToList().Count();
Console.WriteLine(houses);

// Calculate new position from command and current position
(int, int) commandRouter(char command, int x, int y)
{
    return command switch
    {
        '>' => (x - 1, y),
        '<' => (x + 1, y),
        '^' => (x, y + 1),
        'v' => (x, y - 1),
        _ => (x, y)
    };
}