using UnityEngine;

public class GridObject : MonoBehaviour
{

    [SerializeField] private Vector2Int size = Vector2Int.one;
    bool isDraggable;
    public bool isDragging;
    Collider2D objectCollider;

    GridScript grid;
    private Vector2 startPosition;
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
                if (grid.levelGrid[checkX, checkY] != TileType.floor)
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
        pos += Vector3.one * 0.001f;

        cell = grid.WorldToGrid(pos);

        if (CanPlace(cell))
        {
            Vector2 baseWorld = grid.GridToWorld(cell.x, cell.y);
            Vector2 offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
            // Vector2 offset = new Vector2((size.x - 1), (size.y - 1));
            transform.position = baseWorld + offset;
            //transform.position = baseWorld;

            currentPosition = cell;
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
                    grid.levelGrid[gridX, gridY] = TileType.furniture;

                if (gameObject.tag == "Potato")
                    grid.levelGrid[gridX, gridY] = TileType.potato;

                if (gameObject.tag == "RottenPotato")
                    grid.levelGrid[gridX, gridY] = TileType.rottenPotato;
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

    public void SetRottenPotatoScent(Vector2Int potato)
    {
        int width = grid.gridWidth;
        int height = grid.gridHeight;

        void TrySet(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return;

            if (grid.levelGrid[x, y] == TileType.floor)
            {
                grid.levelGrid[x, y] = TileType.rottenPotatoScent;
            }
        }

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0)
                    continue; 

                TrySet(potato.x + dx, potato.y + dy);
            }
        }

    
        TrySet(potato.x + 2, potato.y);
        TrySet(potato.x - 2, potato.y);
        TrySet(potato.x, potato.y + 2);
        TrySet(potato.x, potato.y - 2);
    }


    public void RemoveRottenPotatoScent(Vector2 potato)
    {

    }
}