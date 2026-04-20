using UnityEngine;

public class LeaveBehindObject : GridObject
{
    public enum Objects
    {
        Water,
        Fire,
        Cutlery,
    }

    public Objects objectToLeaveBehind;

    public GameObject cutleryPrefab;
    public GameObject firePrefab;
    public GameObject waterPrefab;
    public GameObject leakyPipePrefab;

    public GameObject invisibleSpongePrefab;

    public GameObject source;

    public override void SetGridSlots(Vector2Int origin)
    {
        //Normal functionality

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < yTilesToPlace; y++)
            {
                int gridX = origin.x + x;
                int gridY = origin.y + y;

                if (gameObject.tag == "Furniture")
                {
                    grid.levelGrid[gridX, gridY] = TileType.furniture;
                }

                if (gameObject.tag == "Potato")
                {
                    grid.levelGrid[gridX, gridY] = TileType.potato;
                }

                if (gameObject.tag == "RottenPotato")
                {
                    grid.levelGrid[gridX, gridY] = TileType.rottenPotato;
                }
            }
        }


        //Additional functionality

        base.SetGridSlots(origin);

        BeenMoved moved = GetComponent<BeenMoved>();

        if (!moved.beenMoved && !moved.slotSet)
        {
            moved.slotSet = true;
        }
        else if (!moved.beenMoved && moved.slotSet)
        {
            moved.beenMoved = true;

            Vector2Int placePos = grid.WorldToGrid(previousPosition);

            LeaveBehind(placePos);
        }
        else
        {
            moved.beenMoved = true;
        }
    }

    void LeaveBehind(Vector2Int gridPos)
    {
        Vector2 worldPos = grid.GridToWorld(gridPos);

        switch (objectToLeaveBehind)
        {
            case Objects.Water:
                source = Instantiate(waterPrefab, worldPos, Quaternion.identity, parent);
                Instantiate(leakyPipePrefab, worldPos, Quaternion.identity, parent);
                grid.levelGrid[gridPos.x, gridPos.y] = TileType.water;
                break;
            case Objects.Fire:
                Instantiate(firePrefab, worldPos, Quaternion.identity, parent);
                grid.levelGrid[gridPos.x, gridPos.y] = TileType.fire;
                break;
            case Objects.Cutlery:
                Instantiate(cutleryPrefab, worldPos, Quaternion.identity, parent);
                grid.levelGrid[gridPos.x, gridPos.y] = TileType.cutlery;
                break;
        }


    }

}
