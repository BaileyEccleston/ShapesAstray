using UnityEngine;

public class GrandmaSpawnManager : MonoBehaviour
{
    public GridScript grid;
    public LevelManager levelManager;
    Transform parent;
    public GameObject grandma;
    Vector2 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();


        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        parent = transform.parent;
        for (int x = 0; x < grid.gridWidth; x++)
        {
            for (int y = 0; y < grid.gridHeight; y++)
            {
                if (grid.levelGrid[x, y] == TileType.startPos)
                {
                    Vector2Int gridPos = new Vector2Int(x, y);
                    pos = grid.GridToWorld(gridPos);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnGrandma()
    {
        Instantiate(grandma, pos, Quaternion.identity, parent);
    }
}
