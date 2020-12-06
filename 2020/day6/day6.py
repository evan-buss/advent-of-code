from os import linesep
from typing import List

with open("input.txt") as f:
    input = f.read()


def part1():
    groups: List[str] = [line.strip().replace('\n', '').replace(' ', '')
                         for line in input.split('\n\n')]

    total = 0
    for group in groups:
        total += len(set([c for c in group]))

    print('Part 1:', total)


def part2():
    groups: List[str] = [line.strip().replace(' ', '')
                         for line in input.split('\n\n')]
    groups: List[List[str]] = [group.split('\n') for group in groups]

    total = 0
    for group in groups:
        s = {}
        for line in group:
            for char in line:
                s[char] = s[char] + 1 if char in s else 1

        for key in s:
            total += 1 if s[key] == len(group) else 0

    print('Part 2:', total)


part1()
part2()
