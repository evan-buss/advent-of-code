using System;
using System.IO;
using System.Linq;
using Day4;

var part1 =
    File.ReadAllText("input.txt")
    .Split(Environment.NewLine + Environment.NewLine)
    .Select(x => new Passport(x))
    .Count(x => x.IsValidPart1());

Console.WriteLine("Part 1: {0} valid passports", part1);

var part2 =
    File.ReadAllText("input.txt")
    .Split(Environment.NewLine + Environment.NewLine)
    .Select(x => new Passport(x))
    .Count(x => x.IsValidPart1() && x.IsValidPart2());

Console.WriteLine("Part 2: {0} valid passports", part2);