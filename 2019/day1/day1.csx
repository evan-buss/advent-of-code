var lines = File.ReadLines("input.txt");

// Calculate fuel by converting string
Func<string, int> calcFuelStr = (line) => Convert.ToInt32(line) / 3 - 2;
// Calculate fuel
Func<int, int> calcFuel = (num) => num / 3 - 2;

// Part 1
int totalFuel = lines.Select(calcFuelStr).Sum();

Console.WriteLine(totalFuel);

// Part 2
totalFuel = lines.Select(line => {
  var sum = 0;
  var fuel = calcFuelStr(line);
  while(fuel > 0) 
  {
    sum += fuel;
    fuel = calcFuel(fuel);
  }
  return sum;
}).Sum();

Console.WriteLine(totalFuel);
