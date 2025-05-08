using UnityEngine;
public static class BlockValidation
{
    public static bool IsValidPosition(Transform block)
    {
        foreach (Transform child in block)
        {
            Vector2Int pos = GridManager.Round(child.position);
            if (!IsValid(pos, child, block)) return false;
        }

        return true; // All children are valid
    }

    private static bool IsValid(Vector2Int pos, Transform t, Transform block)
    {
        if (!GridManager.InsideBorder(pos) || pos.y >= GridManager.height) return false;
        
        Transform existing = GridManager.GetAt(pos.x, pos.y);
        return existing == null || existing.parent == block;
    }

    public static bool IsMoveBlocked(Transform block, Vector2Int direction)
    {
        foreach (Transform child in block)
        {
            Vector2Int pos = GridManager.Round(child.position);
            Vector2Int newPos = pos + direction;

            if (!GridManager.InsideBorder(newPos) || GridManager.GetAt(newPos.x, newPos.y) != null)
            {
                return true; // Move is blocked
            }
        }
        return false;
    }
}
