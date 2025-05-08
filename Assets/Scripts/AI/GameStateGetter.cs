using UnityEngine;

public class GameStateGetter : MonoBehaviour
{
    public static GameStateGetter instance;
    private void Awake()
    {
        instance = this;
    }

    public int[,] GetBoardState()
    {
        int width = GridManager.width;
        int height = GridManager.height;
        int[,] board = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (board[x, y] != 0)
                    board[x, y] = 1; // Block placed
                else
                    board[x, y] = 0; // No block placed
            }
        }

        return board;
    }
}
