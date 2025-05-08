using UnityEngine;

public static class ScoreManager
{
    public static int Score { get; private set; }
    public static int HighScore { get; private set; }

    public static void AddScore(int linesCleared)
    {
        int points = 0;
        switch (linesCleared)
        {
            case 1: points = 100; break;
            case 2: points = 300; break;
            case 3: points = 500; break;
            case 4: points = 800; break;
            default: points = linesCleared * 100; break; // Fallback for any other case
        }

        Score += points;
        Debug.Log($"Score: {Score}");
    }

    public static void ResetScore()
    {
        Score = 0;
    }

    public static void CheckHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
            Debug.Log($"New High Score: {HighScore}");
        }
    }

    public static int GetScore()
    {
        return Score;
    }

    public static int GetHighScore()
    {
        return HighScore;
    }
}
