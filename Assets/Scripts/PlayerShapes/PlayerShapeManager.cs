using TMPro;
using UnityEngine;

public class PlayerShapeManager : MonoBehaviour
{
    [SerializeField] int spawnCount;
    public int goalCount;

    int shapesSafe;

    int currentCount;

    public GameObject playerShape;

    public GridScript grid;

    TMP_Text startingShapeCount;
    TMP_Text safeShapesCount;
    TMP_Text goalShapeCount;
    TMP_Text currentShapeCount;
    Transform parent;



    public void GetGrid()
    {
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parent = transform.parent;
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();
        startingShapeCount = GameObject.Find("StartingShapeCount").GetComponent<TMP_Text>();
        safeShapesCount = GameObject.Find("SafeShapes").GetComponent<TMP_Text>();
        goalShapeCount = GameObject.Find("TargetShapes").GetComponent<TMP_Text>();
        currentShapeCount = GameObject.Find("CurrentShapes").GetComponent<TMP_Text>();
        currentCount = spawnCount;
        startingShapeCount.text = "Starting Shapes - " + spawnCount;
        goalShapeCount.text = "Target Safe Shapes - " + spawnCount * 0.6f;
        shapesSafe = 0;
        goalCount = spawnCount;
        for (int x = 0; x < grid.gridWidth; x++)
        {
            for (int y = 0; y < grid.gridHeight; y++)
            {
                if (grid.levelGrid[x, y] == TileType.startPos)
                {
                    Vector2Int gridPos = new Vector2Int(x, y);
                    Vector2 pos = grid.GridToWorld(gridPos);
                    SpawnShapes(pos);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        safeShapesCount.text = "Safe Shapes - " + shapesSafe;
        currentShapeCount.text = "Current Shapes - " + currentCount;
    }

    void SpawnShapes(Vector2 spawnPos)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(playerShape, spawnPos, Quaternion.identity, parent);
        }
    }

    public void Safe()
    {
        shapesSafe++;
        SetCurrentCount(-1);
    }

    public void SetCurrentCount(int count)
    {
        currentCount += count;
    }
}
