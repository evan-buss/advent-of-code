from typing import Any, Union
from math import floor


class Seat:
    seat_id: int

    def __init__(self, bpass: str):
        row = self._binary_search(
            bpass[:7].replace("F", "L").replace("B", "R"),
            0, 127)
        col = self._binary_search(bpass[7:], 0, 7)
        self.seat_id = row * 8 + col

    def _binary_search(self, bpass: str, left: int, right: int) -> int:
        for char in bpass:
            m = floor((left + right) / 2)
            if char == "L":
                right = m
            elif char == "R":
                left = m + 1
        return left


with open("input.txt") as file:
    content = file.readlines()
    passes = [Seat(boarding_pass) for boarding_pass in content]


def part1():
    print("Part 1:", max(bpass.seat_id for bpass in passes))


def part2():
    sorted_passes = sorted(passes, key=lambda x: x.seat_id)
    for i in range(len(sorted_passes)):
        prev, curr = sorted_passes[i - 1], sorted_passes[i]
        if curr.seat_id - prev.seat_id > 1:
            print("Part2: ", prev.seat_id + 1)


part1()
part2()
