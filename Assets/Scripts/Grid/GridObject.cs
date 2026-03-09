using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridObject : MonoBehaviour
{

    [SerializeField] private Vector2Int size = Vector2Int.one;
    bool isDraggable;
    public bool isDragging;
    Collider2D objectCollider;

    GridScript grid;
    private Vector2 startPosition;
    private Vector2 previousPosition;
    private Vector2Int currentPosition;

    Vector2Int cell;


    float offsetX;
    float offsetY;
    // Start is called before the first frame update
    void Start()
    {
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();
        objectCollider = GetComponent<Collider2D>();
        isDragging = false;

        PlaceInGrid();


    }

    // Update is called once per frame
    void Update()
    {
        DragAndDrop();
    }

    void DragAndDrop()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (objectCollider.OverlapPoint(mousePosition))
            {
                isDraggable = true;
            }
            else
            {
                isDraggable = false;
            }

            if (isDraggable)
            {
                isDragging = true;
                ClearGridSlots(currentPosition);
            }
        }
        if (isDragging)
        {
            this.transform.position = mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging && grid != null) 
            {
                isDraggable = false;
                isDragging = false;
                PlaceInGrid();
            }
        }
    }

    bool CanPlace(Vector2Int origin)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int checkX = origin.x + x;
                int checkY = origin.y + y;

                if (checkX < 0 || checkX >= grid.gridWidth || checkY < 0 || checkY >= grid.gridHeight)
                {
                    return false;
                }
                
                //Make sure to add the other tiles here cant be bothered doing it now
                if (grid.levelGrid[checkX, checkY] != TileType.floor && grid.levelGrid[checkX, checkY] != TileType.potato && grid.levelGrid[checkX, checkY] != TileType.rottenPotato)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void PlaceInGrid()
    {
        Vector3 pos = transform.position;
       // pos += Vector3.one * 0.001f;

        cell = grid.WorldToGrid(pos);
        previousPosition = startPosition;
        if (CanPlace(cell))
        {
            Vector2 offset;

            Vector2 baseWorld = grid.GridToWorld(cell.x, cell.y);
            if (size.x >= 3)
            {
                offset = new Vector2(((size.x - 1) * 0.5f) - 1, (size.y - 1) * 0.5f);
            }
            else
            {
                offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
            }
                

           // Vector2 offset = new Vector2(size.x / 0.5f, size.y / 0.5f);
            transform.position = baseWorld + offset;
            //transform.position = baseWorld;
            // currentPosition = cell;
            Vector2Int adjustCell = cell;
            if (size.x >= 3)
            {
                adjustCell.x -= 1;
            }
            currentPosition = adjustCell;
            startPosition = transform.position;

            SetGridSlots(currentPosition);
        }
        else
        {
            transform.position = startPosition;
            SetGridSlots(currentPosition);
        }
    }

    public void SetGridSlots(Vector2Int origin)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int gridX = origin.x + x;
                int gridY = origin.y + y;

                if (gameObject.tag == "Furniture")
                {
                    if (gameObject.name.Contains("Sink"))
                    {
                        Sink sink = gameObject.GetComponent<Sink>();
                        if (!sink.beenMoved && !sink.slotSet)
                        {
                            sink.slotSet = true;
                        }
                        else if (!sink.beenMoved && sink.slotSet)
                        {
                            sink.beenMoved = true;
                            Vector2Int pos = Vector2Int.RoundToInt(startPosition);
                            SpawnWater(pos);
                        }
                        else
                        {
                            sink.beenMoved = true;
                        }
                    }
                    grid.levelGrid[gridX, gridY] = TileType.furniture;
                }

                if (gameObject.tag == "Potato")
                {
                    grid.levelGrid[gridX, gridY] = TileType.potato;
                }

                if (gameObject.tag == "RottenPotato")
                {
                    grid.levelGrid[gridX, gridY] = TileType.rottenPotato;
                    Vector2Int slot = new Vector2Int(gridX, gridY);
                    SetRottenPotatoScent(slot);
                }
            }
        }
    }

    void ClearGridSlots(Vector2Int origin)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                int gridX = origin.x + x;
                int gridY = origin.y + y;

                if (gridX >= 0 && gridX < grid.gridWidth &&
                    gridY >= 0 && gridY < grid.gridHeight)
                {
                    grid.levelGrid[gridX, gridY] = TileType.floor;
                }
            }
        }
    }

    public void SetRottenPotatoScent(Vector2Int potatoSlot)
    {
        int minX = Mathf.Max(0, potatoSlot.x - 3);
        int maxX = Mathf.Min(grid.gridWidth - 1, potatoSlot.x + 3);

        int minY = Mathf.Max(0, potatoSlot.y - 3);
        int maxY = Mathf.Min(grid.gridHeight - 1, potatoSlot.y + 3);

        bool appliedScent = false;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (grid.levelGrid[x, y] == TileType.floor)
                {
                    grid.levelGrid[x, y] = TileType.rottenPotatoScent;
                    appliedScent = true;
                }
            }
        }

        if (appliedScent)
        {
            RottenPotato rottenPotato = this.gameObject.GetComponent<RottenPotato>();
            rottenPotato.Placed();
            StartCoroutine(RottenPotatoLife(potatoSlot));
        }
    }




    public void RemoveRottenPotatoScent(Vector2Int potatoSlot)
    {
        Debug.Log("Remove Scent");
        int minX = Mathf.Max(0, potatoSlot.x - 3);
        int maxX = Mathf.Min(grid.gridWidth - 1, potatoSlot.x + 3);

        int minY = Mathf.Max(0, potatoSlot.y - 3);
        int maxY = Mathf.Min(grid.gridHeight - 1, potatoSlot.y + 3);

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (grid.levelGrid[x, y] == TileType.rottenPotatoScent)
                {
                    grid.levelGrid[x, y] = TileType.floor;
                }
            }
        }

    }

    IEnumerator RottenPotatoLife(Vector2Int potatoSlot)
    {
        yield return new WaitForSeconds(13f);
        RemoveRottenPotatoScent(potatoSlot);
    }

    public GameObject water;
    int spreadAmount = 0;
    Vector2 waterSource;
    int leakTimes = 2;
    int timesLeaked = 0;
    public void SpawnWater(Vector2Int sinkSlot)
    {
        waterSource = previousPosition;
        Instantiate(water, previousPosition, Quaternion.identity);
        StartCoroutine(SpreadWaterTimer());
    }


    public void SpreadMoreWater()
    {
        if (timesLeaked < leakTimes)
        {
            spreadAmount++;

            Vector2Int sourceGrid = grid.WorldToGrid(waterSource);

            for (int x = -spreadAmount; x <= spreadAmount; x++)
            {
                for (int y = -spreadAmount; y <= spreadAmount; y++)
                {
                    if (Mathf.Abs(x) == spreadAmount || Mathf.Abs(y) == spreadAmount)
                    {
                        Vector2Int gridPos = sourceGrid + new Vector2Int(x, y);

                        if (gridPos.x >= 0 && gridPos.x < grid.levelGrid.GetLength(0) &&
                            gridPos.y >= 0 && gridPos.y < grid.levelGrid.GetLength(1))
                        {
                            if (grid.levelGrid[gridPos.x, gridPos.y] == TileType.floor)
                            {
                                Vector2 worldPos = grid.GridToWorld(gridPos);

                                Instantiate(water, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                            }
                        }
                    }
                }
            }
            timesLeaked++;
            StartCoroutine(SpreadWaterTimer());
        }
    }



    IEnumerator SpreadWaterTimer()
    {
        yield return new WaitForSeconds(3f);
        SpreadMoreWater();
    }

}