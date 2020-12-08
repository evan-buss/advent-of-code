import re
from collections import deque
from typing import List, Optional, Set
from dataclasses import dataclass


@dataclass
class Bag:
    color: str
    children: List[tuple[int, str]]

    def __init__(self, rule: str):
        color_regex = re.compile(r"^(\w+ \w+) bags contain .+")
        self.color = color_regex.match(rule).group(1)

        self.children = []
        inner_regex = r"(\d) (\w+ \w+) bags?"
        matches = re.findall(inner_regex, rule)
        for match in matches:
            self.children.append((int(match[0]), match[1]))


with open('input.txt') as f:
    rules = [Bag(line) for line in f.read().splitlines()]
    

print(rules)
