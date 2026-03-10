using UnityEngine;
using System.Collections;

public class SinkObject : GridObject
{
    public GameObject water;

    int spreadAmount = 0;
    Vector2 waterSource;

    int leakTimes = 2;
    int timesLeaked = 0;

    public override void SetGridSlots(Vector2Int origin)
    {
        base.SetGridSlots(origin);

        Sink sink = GetComponent<Sink>();

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

    void SpawnWater(Vector2Int sinkSlot)
    {
        waterSource = previousPosition;

        Instantiate(water, previousPosition, Quaternion.identity, parent);

        StartCoroutine(SpreadWaterTimer());
    }

    public void SpreadMoreWater()
    {
        if (timesLeaked < leakTimes)
        {
            spreadAmount++;

            Vector2Int sourceGrid = grid.WorldToGrid(waterSource);

            for (int x = spreadAmount; x >= -spreadAmount; x--)
            {
                for (int y = spreadAmount; y >= -spreadAmount; y--)
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

                                Instantiate(water, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity, parent);
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