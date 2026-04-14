using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour
{
    [SerializeField] int waterDepthIncreaseTime;
    //The number of times depth increases until the water becomes deadly
    [SerializeField] public  int waterDepthIncreaseFinalAmount;
    public int waterDepthIncreaseCurrentAmount;
    Renderer renderer;
    Color color;

    GridScript grid;


    public bool isSource = false;

    public GameObject waterPrefab;

    Vector2Int gridPos;


    float fadeSpeed = 2f;

    public bool shouldSpread = true;
    bool maxDepth = false;

    void Start()
    {

        renderer = GetComponent<Renderer>();
        color = renderer.material.color;
        color.a = 0.2f;
        renderer.material.color = color;
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();

        gridPos = grid.WorldToGrid(transform.position);

        StartCoroutine(SpreadWater());
    }



    IEnumerator SpreadWater()
    {
        yield return new WaitForSeconds(2f);

        TrySpread(Vector2Int.up);
        TrySpread(Vector2Int.down);
        TrySpread(Vector2Int.left);
        TrySpread(Vector2Int.right);

        if (!maxDepth)
        {
            StartCoroutine(IncreaseDepth());
        }
        else
        {
            StartCoroutine(SpreadWater());
        }
    }



    IEnumerator IncreaseDepth()
    {
        if (shouldSpread)
        {
            yield return new WaitForSeconds(waterDepthIncreaseTime);
            waterDepthIncreaseCurrentAmount++;
            color.a = (float)waterDepthIncreaseCurrentAmount / waterDepthIncreaseFinalAmount;
            renderer.material.color = color;



            if (waterDepthIncreaseCurrentAmount < waterDepthIncreaseFinalAmount)
            {
                 StartCoroutine(SpreadWater());
            }
            else
            {
                maxDepth = true;
            }
        }

    }


    void TrySpread(Vector2Int dir)
    {
        Vector2Int newPos = gridPos + dir;

        if (newPos.x < 0 || newPos.x >= grid.gridWidth || newPos.y < 0 || newPos.y >= grid.gridHeight)
        {
            return;
        }

        if (grid.levelGrid[newPos.x, newPos.y] == TileType.floor || grid.levelGrid[newPos.x, newPos.y] == TileType.openDoor || grid.levelGrid[newPos.x, newPos.y] == TileType.fire)
        {
            Vector2 worldPos = grid.GridToWorld(newPos);

            if (shouldSpread)
            {
                Instantiate(waterPrefab, worldPos, Quaternion.identity, transform.parent);
                grid.levelGrid[newPos.x, newPos.y] = TileType.water;
            }

 
        }

    }
    bool callOnce = false;
    private void Update()
    {
        if (maxDepth && !callOnce)
        {
            StartCoroutine (SpreadWater());
            callOnce = true;
        }
    }





}
