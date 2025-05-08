using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 10;
    public static int height = 20;
    public static Transform[,] grid = new Transform[width, height];

    public static Vector2Int Round(Vector3 pos)
    {
        return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
    }

    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < width &&
                (int)pos.y >= 0 &&
                (int)pos.y < height); // Ensures that the position is within the vertical grid boundaries
    }

    public static void AddToGrid(Transform block)
    {
        Vector2 pos = Round(block.position);
        int x = (int)pos.x;
        int y = (int)pos.y;

        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            Debug.LogWarning("Trying to add a block outside the grid: " + pos + " - " + block.name);
            return;
        }

        grid[x, y] = block;
    }

    public static Transform GetAt(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;

        return grid[x, y];
    }

    public static Transform SetAt(int x, int y, Transform block)
    {
        if (!InsideBorder(x, y))
            return null;

        if (grid == null)
        {
            Debug.LogError("GridManager.grid wurde noch nicht initialisiert!");
            return null;
        }

        Transform oldBlock = grid[x, y];
        grid[x, y] = block;
        return oldBlock;
    }

    public static bool InsideBorder(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
