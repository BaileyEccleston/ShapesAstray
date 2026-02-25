using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Net;

public class EnemyMove : MonoBehaviour
{
    enum State
    {
        Stationary,
        Patrol,
        Pick,
        Shake,
        Attack,
        Idle,
        None,
    }



    private bool inDarkRoom = false;

    [SerializeField] private SpriteRenderer spriteRenderer;

    public BFSMovement bfs;
    GridScript grid;
    float moveSpeed;
    public float attackSpeed;
    public float patrolSpeed;


    public Vector2Int targetCell;

    private List<Vector2Int> path = new List<Vector2Int>();

    State currentState;
    [SerializeField]
    GameObject targetPlayer;

    Move move;


    private void Awake()
    {
        grid = FindAnyObjectByType<GridScript>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = patrolSpeed;
        currentState = State.Stationary;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (targetPlayer == null)
        {
            targetPlayer = null;
            currentState = State.Patrol;
        } */

        if (currentState == State.Attack)
        {
            moveSpeed = attackSpeed;
            targetCell = grid.WorldToGrid(targetPlayer.transform.position);
            Vector2Int currentCell = grid.WorldToGrid(transform.position);

            path = bfs.FindPath(currentCell, targetCell);

            if (Vector2.Distance(transform.position, targetPlayer.transform.position) < 1f)
            {
                move.Die();
                targetPlayer = null;
                currentState = State.Patrol;

            }
            // Debug.Log(path[0]);
            if (path != null && path.Count > 0)
            {
                Vector2 nextWorldPos = grid.GridToWorld(path[0]);
                transform.position = Vector2.MoveTowards(transform.position, nextWorldPos, Time.deltaTime * moveSpeed);
            }
            else
            {
                targetPlayer = null;
                currentState = State.Patrol;
            }


        }
        if (currentState == State.Patrol)
        {
            moveSpeed = patrolSpeed;
            if (path == null || path.Count == 0)
            {
                PickRandomPatrolTarget();
            }

            if (path != null && path.Count > 0)
            {
                Vector2Int nextCell = path[0];
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
        }


    }


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

            if (newPath != null && newPath.Count > 0)
            {
                path = newPath;
                return;
            }
        }

        path.Clear();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Light")
        {
            targetPlayer = null;
    
            currentState = State.Stationary;
        }
        if (currentState == State.Patrol)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("Player shape detected");
                targetPlayer = collision.gameObject;
                currentState = State.Shake;
                StartCoroutine(Shake());
                move = targetPlayer.GetComponent<Move>();
            }
        }

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            currentState = State.Patrol;
        }
    }

    IEnumerator Shake()
    {
        yield return new WaitForSeconds(5);
        currentState = State.Attack;
    }
}
