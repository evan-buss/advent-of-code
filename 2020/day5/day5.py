from typing import Any, Union

from dataclasses import dataclass
from math import floor


@dataclass
class Seat:
    seat_id: int
    col: int
    row: int

    def __init__(self, bpass: str):
        self._parse_rows(bpass)
        self._parse_cols(bpass)
        self.seat_id = self.row * 8 + self.col

    def _parse_rows(self, bpass: str):
        row_parts = bpass[:7]
        left, right = 0, 127
        for char in row_parts:
            m = floor((left + right) / 2)
            if char == "F":
                right = m
            elif char == "B":
                left = m + 1
        self.row = left

    def _parse_cols(self, bpass: str):
        col_parts = bpass[7:]
        left, right = 0, 7
        for char in col_parts:
            m = floor((left + right) / 2)
            if char == "L":
                right = m
            elif char == "R":
                left = m + 1
        self.col = left


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
