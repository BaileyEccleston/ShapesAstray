using UnityEngine;
using System.Collections;

public class GridObject : MonoBehaviour
{
    [SerializeField] protected Vector2Int size = Vector2Int.one;

    protected bool isDraggable;
    public bool isDragging;

    protected Collider2D objectCollider;
    protected Transform parent;

    protected GridScript grid;

    protected Vector2 startPosition;
    protected Vector2 previousPosition;

    protected Vector2Int currentPosition;
    protected Vector2Int cell;

    public int yTilesToPlace;


    void Start()
    {
        parent = transform.parent;
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();
        objectCollider = GetComponent<Collider2D>();

        isDragging = false;

        PlaceInGrid();
        Physics2D.queriesHitTriggers = true;
    }

    void Update()
    {
        DragAndDrop();
    }

    void DragAndDrop()
    {
        // Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit.collider != null && hit.collider == objectCollider)
            {
                isDraggable = true;
                isDragging = true;
                ClearGridSlots(currentPosition);
            }
            else
            {
                isDraggable = false;
            }

        }

        if (isDragging)
        {
            // Snap while dragging using the same logic as PlaceInGrid
            Vector2Int previewCell = grid.WorldToGrid(mousePos);
            Vector2 baseWorld = grid.GridToWorld(previewCell.x, previewCell.y);

            Vector2 offset;
            if (size.x >= 3)
                offset = new Vector2(((size.x - 1) * 0.5f) - 1, (size.y - 1) * 0.5f);
            else
                offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

            transform.position = baseWorld + offset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isDragging && grid != null)
            {
                isDragging = false;
                isDraggable = false;

                PlaceInGrid();
            }
        }
    }

    bool CanPlace(Vector2Int origin)
    {
        for (int x = 0; x < size.x; x++)
        {
            // Only place the amount of yTiles from the bottom of the furniture item
            // This allows furniture to be placed behind other furniture or for shapes and enemies to move behind furniture
            for (int y = 0; y < yTilesToPlace; y++)
            {
                int checkX = origin.x + x;
                int checkY = origin.y + y;

                if (size.x >= 3)
                {
                    checkX -= 1;
                }

                if (checkX < 0 || checkX >= grid.gridWidth || checkY < 0 || checkY >= grid.gridHeight)
                {
                    return false;
                }

                if (size.y >= 2)
                {
                    if (grid.levelGrid[checkX, checkY] != TileType.floor &&
                       // grid.levelGrid[checkX, checkY] != TileType.bottomWall &&
                        grid.levelGrid[checkX, checkY] != TileType.potato &&
                        grid.levelGrid[checkX, checkY] != TileType.rottenPotato &&
                        grid.levelGrid[checkX, checkY] != TileType.water)
                        return false;
                }
                else
                {
                    if (grid.levelGrid[checkX, checkY] != TileType.floor &&
                        grid.levelGrid[checkX, checkY] != TileType.potato &&
                        grid.levelGrid[checkX, checkY] != TileType.rottenPotato &&
                        grid.levelGrid[checkX, checkY] != TileType.water)
                        return false;
                }

            }
        }

        return true;
    }

    public void PlaceInGrid()
    {
        Vector3 pos = transform.position;

        cell = grid.WorldToGrid(pos);
        previousPosition = startPosition;

        if (CanPlace(cell))
        {
            Vector2 offset;
            Vector2 baseWorld = grid.GridToWorld(cell.x, cell.y);

            if (size.x >= 3)
                offset = new Vector2(((size.x - 1) * 0.5f) - 1, (size.y - 1) * 0.5f);
            else
                offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);

            transform.position = baseWorld + offset;
            Vector3 sortLayer = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            transform.position = sortLayer;

            Vector2Int adjustCell = cell;
            if (size.x >= 3)
                adjustCell.x -= 1;

            currentPosition = adjustCell;
            startPosition = transform.position;

            SetGridSlots(currentPosition);
        }
        else
        {
            transform.position = startPosition;
            Vector3 sortLayer = new Vector3(transform.position.x, transform.position.y, transform.position.y);
            transform.position = sortLayer;
            SetGridSlots(currentPosition);
        }
    }

    public virtual void SetGridSlots(Vector2Int origin)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < yTilesToPlace; y++)
            {
                int gridX = origin.x + x;
                int gridY = origin.y + y;

                if (gameObject.tag == "Furniture")
                    grid.levelGrid[gridX, gridY] = TileType.furniture;

                if (gameObject.tag == "Potato")
                    grid.levelGrid[gridX, gridY] = TileType.potato;

                if (gameObject.tag == "RottenPotato")
                    grid.levelGrid[gridX, gridY] = TileType.rottenPotato;

                if (gameObject.tag == "Fish")
                    grid.levelGrid[gridX, gridY] = TileType.fish;
            }
        }
    }

    void ClearGridSlots(Vector2Int origin)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < yTilesToPlace; y++)
            {
                int gridX = origin.x + x;
                int gridY = origin.y + y;

                if (gridX >= 0 && gridX < grid.gridWidth &&
                    gridY >= 0 && gridY < grid.gridHeight)
                    grid.levelGrid[gridX, gridY] = TileType.floor;
            }
        }
    }



}