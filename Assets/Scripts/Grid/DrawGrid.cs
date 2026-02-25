using UnityEngine;

public class DrawGrid : MonoBehaviour
{
    public GridScript grid;
    private void OnDrawGizmos()
    {


        Gizmos.color = Color.gray;

        for (int x = 0; x < grid.gridWidth; x++)
        {
            for (int y = 0; y < grid.gridHeight; y++)
            {
                Gizmos.DrawWireCube(grid.GridToWorld(x, y),Vector3.one * grid.cellSize);
            }
        }
    }
}
