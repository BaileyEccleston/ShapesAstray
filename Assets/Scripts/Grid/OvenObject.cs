using UnityEngine;

public class OvenObject : GridObject
{
    public GameObject firePrefab;

    public override void SetGridSlots(Vector2Int origin)
    {
        base.SetGridSlots(origin);

        Oven oven = GetComponent<Oven>();

        if (!oven.beenMoved && !oven.slotSet)
        {
            oven.slotSet = true;
        }
        else if (!oven.beenMoved && oven.slotSet)
        {
            oven.beenMoved = true;

            Vector2Int firePos = grid.WorldToGrid(previousPosition);

            SpawnFire(firePos);
        }
        else
        {
            oven.beenMoved = true;
        }
    }

    void SpawnFire(Vector2Int gridPos)
    {
        Vector2 worldPos = grid.GridToWorld(gridPos);

        Instantiate(firePrefab, worldPos, Quaternion.identity, parent);

        grid.levelGrid[gridPos.x, gridPos.y] = TileType.fire;
    }
}