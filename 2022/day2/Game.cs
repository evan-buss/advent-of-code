namespace day2;

public enum Move
{
    Rock = 1,
    Paper = 2,
    Scissor = 3
}

public static class Game
{
    public static int GetScore(Move opponent, Move mine)
    {
        if (opponent == mine) return 3;

        return mine switch
        {
            Move.Rock when opponent == Move.Paper => 0,
            Move.Rock when opponent == Move.Scissor => 6,
            Move.Paper when opponent == Move.Scissor => 0,
            Move.Paper when opponent == Move.Rock => 6,
            Move.Scissor when opponent == Move.Rock => 0,
            Move.Scissor when opponent == Move.Paper => 6,
        };
    }

    public static Move GetMove(string move)
    {
        return move switch
        {
            "A" or "X" => Move.Rock,
            "B" or "Y" => Move.Paper,
            "C" or "Z" => Move.Scissor
        };
    }
    
    public static Move GetNeededMove(Move opMove, string result)
    {
        return result switch
        {
            // Lose
            "X" => opMove switch
            {
                Move.Rock => Move.Scissor,
                Move.Paper => Move.Rock,
                Move.Scissor => Move.Paper
            },
            // Draw
            "Y" => opMove,
            // Win
            "Z" => opMove switch
            {
                Move.Rock => Move.Paper,
                Move.Paper => Move.Scissor,
                Move.Scissor => Move.Rock
            },
        };
    }
}