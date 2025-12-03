import re

# with open("day1.txt", "r") as file:
with open("day1-sample.txt", "r") as file:
    p1_count = 0
    p2_count = 0
    position = 50
    for line in file:
        match = re.search(r"([LR])(\d+)", line)
        if match:
            direction = match.group(1)
            distance = int(match.group(2))

            orig_position = position

            if direction == "L":
                position -= distance
            elif direction == "R":
                position += distance

            passes = abs(position) // 100
            p2_count += passes 

            if position < 0 or position > 99:
                position = position % 100

            if position == 0:
                p1_count += 1

            print("move", line.strip())
            print("position", position)
            print() 

    print("part 1 count", p1_count)
    print("part 2 count", p1_count + p2_count)