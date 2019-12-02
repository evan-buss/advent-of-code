from itertools import accumulate;

def calcFuel(next):
    return  (next//3) - 2 

with open("input.txt") as fs:
    content = fs.readlines()

content = [int(x.strip()) for x in content]
fuels = [calcFuel(x) for x in content]
print("Total Fuel: " + str(sum(fuels)))