using UnityEngine;
using System.Collections;
using System.Collections;

public class Fire : MonoBehaviour
{
    GridScript grid;

    public GameObject firePrefab;

    Vector2Int gridPos;
    Vector2Int newPos;


    void Start()
    {
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();

        gridPos = grid.WorldToGrid(transform.position);

        StartCoroutine(SpreadFire());
    }

    IEnumerator SpreadFire()
    {
        yield return new WaitForSeconds(3f);

        TrySpread(Vector2Int.up);
        TrySpread(Vector2Int.down);
        TrySpread(Vector2Int.left);
        TrySpread(Vector2Int.right);

        StartCoroutine(SpreadFire());
    }

    void TrySpread(Vector2Int dir)
    {
        newPos = gridPos + dir;

        if (newPos.x < 0 || newPos.x >= grid.gridWidth || newPos.y < 0 || newPos.y >= grid.gridHeight)
        {
            return;
        }

        if (grid.levelGrid[newPos.x, newPos.y] == TileType.floor || grid.levelGrid[newPos.x, newPos.y] == TileType.openDoor || grid.levelGrid[newPos.x, newPos.y] == TileType.furniture)
        {
            Vector2 worldPos = grid.GridToWorld(newPos);

            Instantiate(firePrefab, worldPos, Quaternion.identity, transform.parent);

            grid.levelGrid[newPos.x, newPos.y] = TileType.fire;
        }
        if (grid.levelGrid[newPos.x, newPos.y] == TileType.closedDoor)
        {
            StartCoroutine(BreakDoor(newPos));
        }
    }

    IEnumerator BreakDoor(Vector2Int pos)
    {
        yield return new WaitForSeconds(10f);

        grid.levelGrid[pos.x, pos.y] = TileType.fire;
        Vector2 worldPos = grid.GridToWorld(pos);

        Instantiate(firePrefab, worldPos, Quaternion.identity, transform.parent);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            grid.levelGrid[gridPos.x, gridPos.y] = TileType.floor;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Door")
        {
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Furniture")
        {
            Destroy(collision.gameObject);
        }
    }
}
