using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var code = File.ReadAllLines("input.txt")
    .Select(x => new Instruction
    {
        Operation = x.Split(' ')[0],
        Argument = int.Parse(x.Split(' ')[1])
    }).ToList();

Part1();
Part2();

void Part1()
{
    var seen = new HashSet<Instruction>();
    var accum = 0;
    for (int i = 0; i < code.Count; i++)
    {
        var op = code[i];
        if (seen.Contains(op))
        {
            break;
        }

        switch (op)
        {
            case { Operation: "nop" }:
                break;

            case { Operation: "acc" }:
                accum += op.Argument;
                break;

            case { Operation: "jmp" }:
                i += op.Argument - 1;
                break;
        }
        seen.Add(op);
    }

    Console.WriteLine("Part 1: {0}", accum);
}

void Part2()
{
    var accum = 0;
    var done = false;
    var prev_flip = -1;
    while (!done)
    {
        var seen = new HashSet<Instruction>();
        var has_flipped = false;

        Console.WriteLine("================== {0} {1}", prev_flip, accum);
        accum = 0;
        for (int i = 0; i < code.Count;)
        {
            var op = code[i];
            //Console.WriteLine($"[{i}] {op}");
            if (seen.Contains(op))
            {
                break;
            }

            if (op.Operation is "nop" or "jmp" && !has_flipped && prev_flip < i)
            {
                has_flipped = true;
                prev_flip++;

                Console.Write("[{0}] Swapping: {1}", i, op);
                op = new Instruction
                {
                    Argument = op.Argument,
                    Operation = op.Operation == "nop" ? "jmp" : "nop"
                };
                Console.WriteLine(" with {0}", op);
            }

            seen.Add(op);
            i = op switch
            {
                { Operation: "nop" } or { Operation: "acc" } => i + 1,
                { Operation: "jmp" } => i + op.Argument,
                _ => throw new Exception("WTF")
            };

            if (op.Operation == "acc")
            {
                accum += op.Argument;
            }

            if (i == code.Count || i == code.Count - 1) done = true;
        }
    }

    Console.WriteLine("Part 2: {0}", accum);
}

internal class Instruction
{
    public string Operation { get; set; }
    public int Argument { get; set; }

    public override string ToString()
    {
        return $"{Operation} {Argument}";
    }
}