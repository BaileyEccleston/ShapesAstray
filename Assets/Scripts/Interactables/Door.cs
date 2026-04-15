using NUnit.Framework.Constraints;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] protected Vector2Int size = Vector2Int.one;
    enum State
    {
        open,
        closed,
    }

    GridScript grid;
    Vector2Int cell;
    private Vector2 startPosition;
    private Vector2Int currentPosition;

    SpriteRenderer spriteRenderer;
    public Sprite closedDoor;
    public Sprite openDoor;



    State doorState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        grid = FindAnyObjectByType<GridScript>();
        doorState = State.closed;
        PlaceInGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaceInGrid()
    {
        Vector3 pos = transform.position;
        pos += Vector3.one * 0.001f;
        cell = grid.WorldToGrid(pos);

        if (cell.x >= 0 && cell.x < grid.gridWidth && cell.y >= 0 && cell.y < grid.gridHeight)
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

            SetGridSlot(currentPosition); 
        }
        else
        {
            transform.position = startPosition;
            SetGridSlot(currentPosition);
        }
    }

    public void SetGridSlot(Vector2Int pos)
    {
        grid.levelGrid[pos.x, pos.y] = TileType.closedDoor;
    }





    private void OnMouseDown()
    {
        //open door
        if (doorState == State.closed)
        {
            doorState = State.open;
            Debug.Log("Opened Door");
            grid.levelGrid[cell.x, cell.y] = TileType.openDoor;
            spriteRenderer.sprite = openDoor;
        }
        else
        {
            doorState = State.closed;
            Debug.Log("Closed Door");
            grid.levelGrid[cell.x, cell.y] = TileType.closedDoor;
            spriteRenderer.sprite = closedDoor;
        }
    }
}
