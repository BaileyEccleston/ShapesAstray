using UnityEngine;
using System.Collections;

public class SinkObject : GridObject
{
    public GameObject waterPrefab;

    public override void SetGridSlots(Vector2Int origin)
    {
        base.SetGridSlots(origin);

        BeenMoved sink = GetComponent<BeenMoved>();

        if (!sink.beenMoved && !sink.slotSet)
        {
            sink.slotSet = true;
        }
        else if (!sink.beenMoved && sink.slotSet)
        {
            sink.beenMoved = true;

            Vector2Int waterPos = grid.WorldToGrid(previousPosition);

            SpawnWater(waterPos);
        }
        else
        {
            sink.beenMoved = true;
        }
    }

    void SpawnWater(Vector2Int gridPos)
    {
        Vector2 worldPos = grid.GridToWorld(gridPos);

        Instantiate(waterPrefab, worldPos, Quaternion.identity, parent);

        grid.levelGrid[gridPos.x, gridPos.y] = TileType.water;
    }
}