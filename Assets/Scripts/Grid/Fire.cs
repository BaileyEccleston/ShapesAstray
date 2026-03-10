using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour
{
    GridScript grid;

    public GameObject firePrefab;

    Vector2Int gridPos;

    void Start()
    {
        grid = GameObject.Find("GridManager").GetComponent<GridScript>();

        gridPos = grid.WorldToGrid(transform.position);

        StartCoroutine(SpreadFire());
    }

    IEnumerator SpreadFire()
    {
        yield return new WaitForSeconds(2f);

        TrySpread(Vector2Int.up);
        TrySpread(Vector2Int.down);
        TrySpread(Vector2Int.left);
        TrySpread(Vector2Int.right);
    }

    void TrySpread(Vector2Int dir)
    {
        Vector2Int newPos = gridPos + dir;

        if (newPos.x < 0 || newPos.x >= grid.gridWidth ||
            newPos.y < 0 || newPos.y >= grid.gridHeight)
            return;

        if (grid.levelGrid[newPos.x, newPos.y] == TileType.floor || grid.levelGrid[newPos.x, newPos.y] == TileType.openDoor)
        {
            Vector2 worldPos = grid.GridToWorld(newPos);

            Instantiate(firePrefab, worldPos, Quaternion.identity, transform.parent);

            grid.levelGrid[newPos.x, newPos.y] = TileType.fire;
        }
    }
}
