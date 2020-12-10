using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var sampleInput = File.ReadAllLines("sample.txt").Select(ulong.Parse).ToArray();
var input = File.ReadAllLines("input.txt").Select(ulong.Parse).ToArray();

var sample = Process(5, sampleInput);
var part1 = Process(25, input);
Console.WriteLine("Part 1: {0}", part1);

Console.WriteLine(Crack(sample, sampleInput));
Console.WriteLine(Crack(part1, input));

ulong Crack(ulong weakness, ulong[] input)
{
    var queue = new Queue<ulong>();
    for (int i = 0; i < input.Length; i++)
    {
        ulong accum = input[i];
        for (int j = i + 1; j < input.Length; j++)
        {
            queue.Enqueue(input[j]);
            accum += input[j];
            if (accum > weakness) break;
            if (accum == weakness)
            {
                var sorted = queue.OrderBy(x => x).ToArray();
                return sorted[0] + sorted.Last();
            }
        }
        queue.Dequeue();
    }

    return 0;
}

ulong Process(int window, ulong[] input)
{
    for (int i = window; i < input.Length; i++)
    {
        var valid = CalculatePermutations(input[(i - window)..i]);
        if (!valid.Contains(input[i]))
        {
            return input[i];
        };
    }

    return 0;
}

HashSet<ulong> CalculatePermutations(ulong[] range)
{
    var combos = new HashSet<ulong>();
    for (int i = 0; i < range.Length; i++)
    {
        for (int j = 1; j < range.Length; j++)
        {
            if (i != j) combos.Add(range[i] + range[j]);
        }
    }

    return combos;
}