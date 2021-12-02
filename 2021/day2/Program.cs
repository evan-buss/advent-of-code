var commands = File.ReadAllLines("input.txt")
  .Select(x => {
    var direction = x.Split(' ')[0] switch {
      "forward" => Direction.Forward,
      "down" => Direction.Down,
      "up" => Direction.Up,
      _ => throw new Exception("Invalid direction")
    };
    return new Command(direction, int.Parse(x.Split(' ')[1]));
  })
  .ToList();

Console.WriteLine($"Part 1: {Part1(commands)}");
Console.WriteLine($"Part 2: {Part2(commands)}");

static int Part1(List<Command> commands)
{
    int x = 0, y = 0;
    foreach (var command in commands) {
      switch (command.Direction) {
        case Direction.Forward:
          x += command.Distance;
          break;
        case Direction.Down:
          y += command.Distance;
          break;
        case Direction.Up:
          y -= command.Distance;
          break;
      }
    }
    return x * y;
}

static int Part2(List<Command> commands)
{
  int x = 0, y = 0, aim = 0;
  foreach (var command in commands) {
    switch (command.Direction) {
      case Direction.Forward:
        y += aim * command.Distance;
        x += command.Distance;
        break;
      case Direction.Down:
        aim += command.Distance;
        break;
      case Direction.Up:
        aim -= command.Distance;
        break;
    }
  }

  return x * y;
}

enum Direction { Forward, Up, Down };

record Command(Direction Direction, int Distance);