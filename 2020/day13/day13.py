from typing import List

# input = '7,13,x,x,59,x,31,19'
input = '19,x,x,x,x,x,x,x,x,41,x,x,x,x,x,x,x,x,x,743,x,x,x,x,x,x,x,x,x,x,x,x,13,17,x,x,x,x,x,x,x,x,x,x,x,x,x,x,29,x,643,x,x,x,x,x,37,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,23'


def part1():
    # target = 939
    target = 1015292
    schedule = [int(x) for x in input.split(',') if x != 'x']
    sched = {}
    for bus in schedule:
        sched[bus] = target + bus - (target % bus)

    best = min(sched, key=sched.get)
    print((sched[best] - target) * best)


part1()
