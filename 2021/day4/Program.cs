using Day1;

var input = File.ReadAllLines("input.txt");
var moves = input
  .Take(1)
  .SelectMany(x => x.Split(","))
  .Select(int.Parse)
  .ToList();
var boards = input.Skip(2).Chunk(6).Select(x => new Board(x[0..5]));

Part1();
Part2();

void Part1()
{
  var p1Boards = boards.ToList();
  foreach (var move in moves)
  {
    foreach (var board in p1Boards)
    {
      board.Mark(move);
      if (board.IsWinning)
      {
        Console.WriteLine($"Part 1: {board.UnmarkedSum * move}");
        return;
      }
    }
  }
}

void Part2()
{
  int lastWinningSum = 0;
  int lastWinningMove = 0;
  var p2Boards = boards.ToList();

  foreach (var move in moves)
  {
    foreach (var board in p2Boards)
    {
      var prevWinning = board.IsWinning;
      board.Mark(move);
      if (!prevWinning && board.IsWinning)
      {
        lastWinningSum = board.UnmarkedSum;
        lastWinningMove = move;
      }
    }
  }

  Console.WriteLine($"Part 2: {lastWinningSum * lastWinningMove}");
}