using System.Text.RegularExpressions;
var lines = File.ReadAllLines("input.txt");

// Hold the wire name and the current value
var wires = new Dictionary<string, int>();

var regex = new Regex(@"");

foreach (var line in lines)
{
    processor(line);
}

void processor(string line)
{
    if (line.Contains("NOT"))
    {

    }
    else if (line.Contains("OR"))
    {

    }
    else if (line.Contains("AND"))
    {

    }
    else if (line.Contains("RSHIFT"))
    {

    }
    else if (line.Contains("LSHIFT"))
    {

    }
    else
    {

    }
}