using System.Collections.Generic;
using UnityEngine;

public class LineClearManager
{
    public static int ClearFullLines()
    {
        int clearedLines = 0;

        for (int y = 0; y < GridManager.height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                ShiftLinesDown(y);
                y--; // gleiche Zeile nochmal prüfen
                clearedLines++;
            }
        }

        return clearedLines;
    }

    private static bool IsLineFull(int y)
    {
        for (int x = 0; x < GridManager.width; x++)
        {
            if (GridManager.GetAt(x, y) == null)
                return false;
        }
        return true;
    }

    private static void ClearLine(int y)
    {
        for (int x = 0; x < GridManager.width; x++)
        {
            Transform block = GridManager.GetAt(x, y);
            if (block != null)
            {
                Object.Destroy(block.gameObject);
                GridManager.SetAt(x, y, null);
            }
        }
    }

    private static void ShiftLinesDown(int fromY)
    {
        for (int y = fromY + 1; y < GridManager.height; y++)
        {
            for (int x = 0; x < GridManager.width; x++)
            {
                Transform block = GridManager.GetAt(x, y);
                if (block != null)
                {
                    GridManager.SetAt(x, y - 1, block);
                    GridManager.SetAt(x, y, null);
                    block.position += Vector3.down;
                }
            }
        }
    }
}
