using System;

using System.IO;
using System.Linq;

var lines = (from line in File.ReadAllLines("input.txt")
             select line.Trim()).ToList();

void Part1()
{
    Console.WriteLine("Part 1: {0}", lines.Where(CheckLine).Count());

    bool CheckLine(string line)
    {
        var parts = line.Split(" ");

        var range = parts[0].Split("-").Select(int.Parse).ToArray();
        var (min, max) = (range[0], range[1]);

        var character = parts[1][0];
        var password = parts[2];

        var count = password.Where(x => x == character).Count();

        return count >= min && count <= max;
    }
}

void Part2()
{
    Console.WriteLine("Part 2: {0}", lines.Where(CheckLine).Count());

    bool CheckLine(string line)
    {
        var parts = line.Split(" ");

        var range = parts[0].Split("-").Select(int.Parse).ToArray();
        var (pos1, pos2) = (range[0], range[1]);

        var character = parts[1][0];
        var password = parts[2];

        return password.Where((c, i) => (i + 1 == pos1 || i + 1 == pos2) && password[i] == character).Count() == 1;
    }
}

Part1();
Part2();