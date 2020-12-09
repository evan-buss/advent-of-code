using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var code = File.ReadAllLines("input.txt")
               .Select(x => new Instruction
               {
                   Operation = x.Split(' ')[0],
                   Argument = int.Parse(x.Split(' ')[1])
               })
               .ToList();

Part1();
Part2();

void Part1()
{
    var seen = new HashSet<Instruction>();
    var accum = 0;
    for (int i = 0; i < code.Count; i++)
    {
        var op = code[i];
        if (seen.Contains(op)) break;
        seen.Add(op);

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
    }

    Console.WriteLine("Part 1: {0}", accum);
}

void Part2()
{
    // Find all instructions that can be swapped and store their indices.
    var swappable = code.Select((x, i) => new { Index = i, Op = x.Operation })
                        .Where(x => x.Op is "nop" or "jmp")
                        .Select(x => x.Index)
                        .ToList();

    foreach (var swap_index in swappable)
    {
        // Swap the reference and store it so we can put it back
        var original = code[swap_index];
        code[swap_index] = new Instruction
        {
            Operation = code[swap_index].Operation == "nop" ? "jmp" : "nop",
            Argument = code[swap_index].Argument
        };

        var seen = new HashSet<Instruction>();
        int accum = 0;
        for (int i = 0; i < code.Count; i++)
        {
            var op = code[i];
            if (seen.Contains(op)) break;
            seen.Add(op);

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

            if (i == code.Count - 1) Console.WriteLine("Part 2: {0}", accum);
        }

        code[swap_index] = original;
    }
}

public class Instruction
{
    public string Operation { get; set; }
    public int Argument { get; set; }

    public override string ToString()
    {
        return $"{Operation} {Argument}";
    }
}