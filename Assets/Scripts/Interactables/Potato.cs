using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Potato : MonoBehaviour
{

    public int health = 1000;

    GridScript grid;
    public int detectionRange = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = FindAnyObjectByType<GridScript>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForPlayerInRange();
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void CheckForPlayerInRange()
    {
        Move[] players = FindObjectsOfType<Move>();

        Vector2Int enemyCell = grid.WorldToGrid(transform.position);

        foreach (Move player in players)
        {
            Vector2Int playerCell = grid.WorldToGrid(player.transform.position);

            int distance =
                Mathf.Abs(enemyCell.x - playerCell.x) +
                Mathf.Abs(enemyCell.y - playerCell.y);

            if (distance <= detectionRange)
            {
                player.TargetPotato(this.gameObject);
            }
        }
    }
}
