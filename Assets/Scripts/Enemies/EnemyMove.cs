using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMove : MonoBehaviour
{
    enum State
    {
        Stationary,
        Patrol,
        Waiting,
        Attack,
        MoveToFish,
        EatFish,
    }
    [SerializeField] GameObject currentFish;
    Fish fish;


    [SerializeField] private SpriteRenderer spriteRenderer;

    // Stores all frames
    [SerializeField] private SpriteVariant[] variants;
    SpriteVariant currentVariant;

    [SerializeField] float animationSpeed = 0.15f;

    float animationTimer;
    int frameIndex;
    Sprite[] currentFrames;

    public BFSMovement bfs;
    GridScript grid;

    public float moveSpeed;
    public float defaultSpeed;

    int moveSpeedMultiplyer = 1;
    public int maxMoveSpeedMultiplyer;


    public int detectionRange;
    public float waitTime;

    public float defaultWaitTime;

    private State currentState;

    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int lastTargetCell;

    private Move targetMove;
    private GameObject targetPlayer;

    private int lightOverlapCount = 0;

    public Vector2Int targetCell;

    private void Awake()
    {
        grid = FindAnyObjectByType<GridScript>();
    }

    private void Start()
    {
        currentState = State.Patrol;
        currentVariant = variants[1];
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
        if (currentState == State.MoveToFish && path.Count == 0 && currentFish != null)
        {
            if (currentState == null)
            {
                currentState = State.Patrol;
                PickRandomPatrolTarget();
            }
            if (path.Count > 0)
            {
                MoveAlongPath();
            }
            else
            {
                currentState = State.EatFish;
                fish = currentFish.GetComponent<Fish>();
            }
                currentState = State.EatFish;
            fish = currentFish.GetComponent<Fish>();
        }
        if (currentState == State.EatFish)
        {
            if (currentFish != null)
            {
                fish.health -= 1;
            }
            else
            {
                currentState = State.Patrol;

            }
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
                player.targeted = true;
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
        yield return new WaitForSeconds(waitTime);

        if (targetPlayer == null || lightOverlapCount > 0)
        {
            currentState = State.Patrol;
            yield break;
        }
        currentVariant = variants[0];
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
                targetCell = cell;
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

        Vector2Int currentCell = grid.WorldToGrid(transform.position);
        Vector2Int direction = nextCell - currentCell;

        UpdateSprite(direction); 

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
        currentVariant = variants[1];
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

    void UpdateSprite(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            currentFrames = currentVariant.up;
        }
        else if (direction == Vector2Int.down)
        {
            currentFrames = currentVariant.down;
        }
        else if (direction == Vector2Int.left)
        {
            currentFrames = currentVariant.left;
        }
        else if (direction == Vector2Int.right)
        {
            currentFrames = currentVariant.right;
        }

        Animate();
    }

    void Animate()
    {
        if (currentFrames == null || currentFrames.Length == 0) return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;

            frameIndex++;
            if (frameIndex >= currentFrames.Length)
            {
                frameIndex = 0;
            }

            spriteRenderer.sprite = currentFrames[frameIndex];
        }
    }

    public void TargetFish(GameObject fish)
    {
        if (fish.tag == "Fish" && fish.gameObject.GetComponent<GridObject>().isDragging == false)
        {
            currentFish = fish.gameObject;
            currentState = State.MoveToFish;

            Vector2Int fishCell = grid.WorldToGrid(fish.transform.position);
            Vector2Int currentCell = grid.WorldToGrid(transform.position);

            targetCell = fishCell;
            path = bfs.FindPath(currentCell, targetCell);
        }
    }

    public void ChangeSpeed()
    {
        moveSpeedMultiplyer++;
        if (moveSpeedMultiplyer > maxMoveSpeedMultiplyer)
        {
            moveSpeedMultiplyer = 1;
            moveSpeed = defaultSpeed;
            waitTime = defaultWaitTime;
        }
        moveSpeed = defaultSpeed * moveSpeedMultiplyer;
        waitTime = defaultWaitTime / moveSpeedMultiplyer;
    }
}