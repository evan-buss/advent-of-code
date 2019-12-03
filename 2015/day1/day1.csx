var line = File.ReadAllText("input.txt");
var floors = 0;
var index = 0;
var found = false;
foreach (var c in line)
{
    if (c == '(')
    {
        floors++;
    }
    else if (c == ')')
    {
        floors--;
    }

    if (floors == -1 && !found)
    {
        Console.WriteLine("Reached the basement {0}", index);
        found = true;
    }
    index++;
}

Console.WriteLine("Final Floor: {0}", floors);
