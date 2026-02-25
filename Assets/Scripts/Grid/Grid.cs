using UnityEngine;

public class Grid : MonoBehaviour
{


    public int gridWidth = 30;
    public int gridHeight = 12;

    public Vector2 gridOrigin = new Vector2(-14.5f, -5.5f);

    public float cellSize = 1;


    public TileType[,] levelGrid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelGrid = new TileType[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                levelGrid[x, y] = TileType.floor;
            }
        }
    }


    public Vector2 GridToWorld(int gridX, int gridY)
    {
        float worldX = gridOrigin.x + (gridX * cellSize);// + cellSize;// * 0.5f;
        float worldY = gridOrigin.y + (gridY * cellSize);// + cellSize;// * 0.5f;

        return new Vector3(worldX, worldY, 0f);
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - gridOrigin.y) / cellSize);

        return new Vector2Int(x, y);
    }


}
