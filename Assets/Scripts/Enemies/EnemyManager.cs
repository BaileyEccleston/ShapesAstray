using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;

    GridScript grid;
    Transform parent;

    void Start()
    {
        parent = transform.parent;
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();

        //Search for enemy spawn tile in the grid
        for (int x = 0; x < grid.gridWidth; x++)
        {
            for (int y = 0; y < grid.gridHeight; y++)
            {
                if (grid.levelGrid[x, y] == TileType.enemySpawn)
                {
                    Vector2Int gridPos = new Vector2Int(x, y);
                    Vector2 pos = grid.GridToWorld(gridPos);
                    SpawnEnemy(pos);
                }
            }
        }
    }

    void SpawnEnemy(Vector2 spawnPos)
    {
        Instantiate(enemy, spawnPos, Quaternion.identity, parent);
    }
}
