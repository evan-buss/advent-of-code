using System.Text;

namespace Day1;

public class Board
{
  private readonly int[][] _board = new int[5][];

  public Board(string[] txtBoard)
  {
    for (var i = 0; i < txtBoard.Length; i++)
    {
      _board[i] = txtBoard[i]
          .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
          .Select(int.Parse)
          .ToArray();
    }
  }

  public bool IsWinning
  {
    get
    {
      for (int i = 0; i < 5; i++)
      {
        if (CheckRow(i) || CheckColumn(i)) return true;
      }
      return false;
    }
  }

  public int UnmarkedSum
  {
    get
    {
      var sum = 0;
      foreach (var (x, y) in GetEnumerator())
      {
        if (_board[y][x] > 0) sum += _board[y][x];
      }
      return sum;
    }
  }

  public void Mark(int value)
  {
    foreach (var (x, y) in GetEnumerator())
    {
      if (_board[y][x] == value) _board[y][x] *= -1;
    }
  }

  public override string ToString()
  {
    var sb = new StringBuilder();
    var prevY = 0;
    foreach (var (x, y) in GetEnumerator())
    {
      if (y > prevY) sb.Append('\n');
      prevY = y;
      sb.Append(_board[y][x].ToString().PadLeft(2));
      sb.Append(' ');
    }
    sb.Append('\n');
    return sb.ToString();
  }

  private bool CheckRow(int index)
  {
    foreach (var value in _board[index])
    {
      if (value > 0) return false;
    }
    return true;
  }

  private bool CheckColumn(int index)
  {
    for (var i = 0; i < _board.Length; i++)
    {
      if (_board[i][index] > 0) return false;
    }
    return true;
  }

  private IEnumerable<(int x, int y)> GetEnumerator()
  {
    for (var y = 0; y < _board.Length; y++)
    {
      for (var x = 0; x < _board[y].Length; x++)
      {
        yield return (x, y);
      }
    }
  }
}