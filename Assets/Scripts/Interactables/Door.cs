using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{

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
            transform.position = grid.GridToWorld(cell.x, cell.y);
            currentPosition = cell;
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
