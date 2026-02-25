using System.Collections.Generic;
using UnityEngine;

public class BFSMovement : MonoBehaviour
{
    GridScript gridMap;


    private Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private void Awake()
    {
        gridMap = FindAnyObjectByType<GridScript>();
    }

    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        cameFrom[start] = start;

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == target)
                break;

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                if (!gridMap.IsWalkable(next))
                    continue;

                if (cameFrom.ContainsKey(next))
                    continue;

                queue.Enqueue(next);
                cameFrom[next] = current;
            }
        }

        List<Vector2Int> path = new List<Vector2Int>();

        if (!cameFrom.ContainsKey(target))
            return path;

        Vector2Int step = target;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }

        path.Reverse();
        return path;
    }
}
