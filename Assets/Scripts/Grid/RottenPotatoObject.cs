using UnityEngine;
using System.Collections;

public class RottenPotatoObject : GridObject
{
    public override void SetGridSlots(Vector2Int origin)
    {
        base.SetGridSlots(origin);

        Vector2Int slot = origin;

        SetRottenPotatoScent(slot);
    }

    void SetRottenPotatoScent(Vector2Int potatoSlot)
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
            RottenPotato rottenPotato = GetComponent<RottenPotato>();
            rottenPotato.Placed();

            StartCoroutine(RottenPotatoLife(potatoSlot));
        }
    }

    void RemoveRottenPotatoScent(Vector2Int potatoSlot)
    {
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
}
