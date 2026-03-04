using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    enum State
    {
        Stationary,
        Patrol,
        Waiting,
        Attack
    }

    public BFSMovement bfs;
    GridScript grid;

    public float moveSpeed = 5f;
    public int detectionRange = 4; // Manhattan grid distance

    private State currentState;

    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int lastTargetCell;

    private Move targetMove;
    private GameObject targetPlayer;

    private int lightOverlapCount = 0;

    private void Awake()
    {
        grid = FindAnyObjectByType<GridScript>();
    }

    private void Start()
    {
        currentState = State.Patrol;
        PickRandomPatrolTarget();
    }

    private void Update()
    {
        if (currentState == State.Stationary)
            return;

        if (currentState == State.Patrol)
        {
            CheckForPlayerInRange();
            FollowPath();
        }
        else if (currentState == State.Attack)
        {
            HandleAttack();
        }
    }

 

    void CheckForPlayerInRange()
    {
        Move[] players = FindObjectsByType<Move>(FindObjectsSortMode.InstanceID);

        Vector2Int enemyCell = grid.WorldToGrid(transform.position);

        foreach (Move player in players)
        {
            Vector2Int playerCell = grid.WorldToGrid(player.transform.position);

           // int distance = Mathf.Abs(enemyCell.x - playerCell.x) + Mathf.Abs(enemyCell.y - playerCell.y);
           float distance = Vector2.Distance(enemyCell, playerCell);



            // I can alter detection to check specific x and y sizes
            if (distance <= detectionRange)
            {
                targetMove = player;
                targetPlayer = player.gameObject;
                currentState = State.Waiting;
                StartCoroutine(BeginAttackAfterDelay());
                return;
            }
        }
    }

    IEnumerator BeginAttackAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        if (targetPlayer == null || lightOverlapCount > 0)
        {
            currentState = State.Patrol;
            yield break;
        }

        currentState = State.Attack;
    }

    // ================= ATTACK =================

    void HandleAttack()
    {
        if (targetPlayer == null)
        {
            ReturnToPatrol();
            return;
        }

        Vector2Int currentCell = grid.WorldToGrid(transform.position);
        Vector2Int targetCell = grid.WorldToGrid(targetPlayer.transform.position);

        if (targetCell != lastTargetCell || path.Count == 0)
        {
            path = bfs.FindPath(currentCell, targetCell);
            lastTargetCell = targetCell;
        }

        if (path.Count == 0)
            return;

        MoveAlongPath();

        currentCell = grid.WorldToGrid(transform.position);

        if (currentCell == targetCell)
        {
            if (targetMove != null)
                targetMove.Die();

            ReturnToPatrol();
        }
    }

    // ================= PATROL =================

    void PickRandomPatrolTarget()
    {
        Vector2Int start = grid.WorldToGrid(transform.position);

        for (int i = 0; i < 100; i++)
        {
            int x = Random.Range(0, grid.gridWidth);
            int y = Random.Range(0, grid.gridHeight);
            Vector2Int cell = new Vector2Int(x, y);

            if (!grid.IsWalkable(cell))
                continue;

            List<Vector2Int> newPath = bfs.FindPath(start, cell);

            if (newPath.Count > 0)
            {
                path = newPath;
                return;
            }
        }

        path.Clear();
    }

    void FollowPath()
    {
        if (path == null || path.Count == 0)
        {
            PickRandomPatrolTarget();
            return;
        }

        MoveAlongPath();
    }

    // ================= MOVEMENT =================

    void MoveAlongPath()
    {
        Vector2Int nextCell = path[0];

        if (!grid.IsWalkable(nextCell))
        {
            path.Clear();
            return;
        }

        Vector2 nextWorldPos = grid.GridToWorld(nextCell);

        transform.position = Vector2.MoveTowards(
            transform.position,
            nextWorldPos,
            moveSpeed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, nextWorldPos) < 0.01f)
        {
            transform.position = nextWorldPos;
            path.RemoveAt(0);
        }
    }

    void ReturnToPatrol()
    {
        targetMove = null;
        targetPlayer = null;
        path.Clear();
        currentState = lightOverlapCount > 0 ? State.Stationary : State.Patrol;

        if (currentState == State.Patrol)
            PickRandomPatrolTarget();
    }

    // ================= LIGHT =================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            lightOverlapCount++;
            currentState = State.Stationary;
            path.Clear();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Light"))
        {
            lightOverlapCount--;

            if (lightOverlapCount <= 0)
            {
                lightOverlapCount = 0;
                currentState = State.Patrol;
                PickRandomPatrolTarget();
            }
        }
    }
}