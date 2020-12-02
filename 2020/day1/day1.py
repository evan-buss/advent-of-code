def part1():
    with open("input.txt") as f:
        content = [int(x.strip()) for x in f.readlines()]
        
        pairs = set()

        for num in content:
            if 2020 - num in pairs:
                print("Part 1:", num * (2020 - num))
                break
            else:
                pairs.add(num)

def part2():
    with open("input.txt") as f:
        content = [int(x.strip()) for x in f.readlines()]

    content.sort()

    for i, num in enumerate(content):

        l, r = i + 1, len(content) - 1
        while l < r:
            threeSum = num + content[l] + content[r]
            if threeSum < 2020:
                l += 1
            elif threeSum > 2020:
                r -= 1
            else:
                print("Part 2:", num * content[l] * content[r])
                break


part1()
part2()