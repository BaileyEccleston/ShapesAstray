using System.Collections.Generic;
using UnityEngine;

public class BFSMovement : MonoBehaviour
{
    GridScript gridMap;

    // Directions that bfs will check in
    // Can add diaginals later maybe
    private Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private void Awake()
    {
        // find reference to grid script since grid script is part of a level prefab
        gridMap = FindAnyObjectByType<GridScript>();
    }


    // Finds path between the 2 given points and outputs it
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        // this queue will store the adjacent tiles 
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        // this dictionary will store the tile a tile came from
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        // Add starting pos to queue
        queue.Enqueue(start);

        // Mark the starting pos as coming from start
        cameFrom[start] = start;

        // Loop unil the target pos is found 
        while (queue.Count > 0)
        {
            // Store the next tile to explore
            // The first traversal store start to begin pathfinding
            Vector2Int current = queue.Dequeue();

            // if current is target, the target has been found so break
            if (current == target)
            {
                 break;
            }
             
            // Check all 4 directions from the current pos
            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                // If not walkable, move on
                if (!gridMap.IsWalkable(next))
                {
                    continue;
                }
                // If the next pos is already visited (stored in the dictionary) move on
                if (cameFrom.ContainsKey(next))
                {
                    continue;
                }

                // Add next to the queue
                queue.Enqueue(next);
                // Mark next pos as coming from current pos
                cameFrom[next] = current;
            }
        }


        // Form the path by following the dictionary back to the start pos 
        // Then reverse the path to make a path from the start to target

        List<Vector2Int> path = new List<Vector2Int>();


        // No path found so return an empty path
        if (!cameFrom.ContainsKey(target))
        {
            return path;
        }

        // Move backwards through the path stoing each steps came from after it
        Vector2Int step = target;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        
        // Reverse path
        // Now you have a breadth first search path congratulations 
        path.Reverse();
        return path;
    }
}
