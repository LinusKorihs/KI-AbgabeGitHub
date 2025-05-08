using UnityEngine;

public class GridVisualiser : MonoBehaviour
{
    public int width = 10;
    public int height = 20;
    public float cellSize = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0); // Mid of the cell
                Gizmos.DrawWireCube(pos, Vector3.one * cellSize);
            }
        }
    }
}
