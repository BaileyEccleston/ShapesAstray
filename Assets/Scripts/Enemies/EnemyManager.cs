using UnityEngine;

public class EnemyManager : MonoBehaviour
{



    public GameObject enemy;

    GridScript grid;
    Transform parent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent;
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();
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

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy(Vector2 spawnPos)
    {
        Instantiate(enemy, spawnPos, Quaternion.identity, parent);
    }
}
