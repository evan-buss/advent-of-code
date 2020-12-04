using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

char[][] map = (from line in File.ReadAllLines("input.txt")
                select line.ToCharArray()).ToArray();

void Part1()
{
    Console.WriteLine("Part 1: {0} Trees", Traverse(3, 1));
}

void Part2()
{
    List<(int x, int y)> slopes = new()
    {
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2),
    };

    Console.WriteLine("Part 2: {0} Trees", slopes.Select(tup => Traverse(tup.x, tup.y)).Aggregate(1U, (x, y) => x * y));
}

// Traverse a slope by the given steps. Wraps on the horizontal.
int Traverse(int stepX, int stepY)
{
    int x = 0, trees = 0;
    for (var y = 0; y < map.Length; y += stepY)
    {
        if (map[y][x] == '#')
        {
            trees++;
        }
        x = (x + stepX) % map[0].Length;
    }

    return trees;
}

Part1();
Part2();