using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Move : MonoBehaviour
{
    enum State
    {
        Move,
        MoveToPotato,
        EatPotato,
        Idle,
        None,
    }

    bool isDead = false;
    bool isSafe = false;

    [SerializeField] GameObject currentPotato;
    Potato potato;

    [SerializeField] private SpriteRenderer spriteRenderer;

    PlayerShapeManager playerShapeManager;

    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;

    public BFSMovement bfs;
    GridScript grid;
    public float moveSpeed;
    public float defaultSpeed;

    public Vector2Int targetCell;

    private List<Vector2Int> path = new List<Vector2Int>();

    State currentState;

    private void Awake()
    {
        grid = FindAnyObjectByType<GridScript>();
        playerShapeManager = FindAnyObjectByType<PlayerShapeManager>();
    }

    private void Start()
    {
        currentState = State.Move;
        PickTarget();

    }

    private void Update()
    {
        if (currentState == State.Move || currentState == State.MoveToPotato)
        {
            FollowPath();
        }

        if (currentState == State.MoveToPotato && path.Count == 0)
        {
            currentState = State.EatPotato;
            potato = currentPotato.GetComponent<Potato>();
        }

        if (currentState == State.EatPotato)
        {
            if (currentPotato != null)
            {
                potato.health -= 1;
            }
            else
            {
                currentState = State.Move;
            }
        }

        //kill if in rotten potato stink
        Vector2Int currentPos = grid.WorldToGrid(transform.position);
        if (grid.levelGrid[currentPos.x, currentPos.y] == TileType.rottenPotatoScent)
        {
            Die();
        }
    }

    void PickTarget()
    {
        Vector2Int start = grid.WorldToGrid(transform.position);

        for (int i = 0; i < 100; i++)
        {
            int x = Random.Range(0, grid.gridWidth);
            int y = Random.Range(0, grid.gridHeight);
            Vector2Int cell = new Vector2Int(x, y);

            // if cell isnt walkable, try another
            if (!grid.IsWalkable(cell))
            {
                continue;
            }





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

    void TargetPotato(Vector2Int targetCell)
    {

    }

    void FollowPath()
    {
        
        if (path == null || path.Count == 0)
        {
            if (currentState == State.Move)
            {
                PickTarget();
            }

            return; 
        }

        Vector2Int nextCell = path[0];

        if (!grid.IsWalkable(nextCell))
        {
            PickTarget();
            return;
        }

        Vector2Int currentCell = grid.WorldToGrid(transform.position);

        if (grid.IsEndGoal(currentCell))
        {
            if (!isSafe && !isDead)
            {
                playerShapeManager.Safe();
                isSafe = true;
            }
            
            Destroy(gameObject);
        }
        Vector2Int direction = nextCell - currentCell;

        UpdateSprite(direction);

        Vector2 nextWorldPos = grid.GridToWorld(nextCell);
        transform.position = Vector2.MoveTowards(transform.position, nextWorldPos, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, nextWorldPos) < 0.01f)
        {
            transform.position = nextWorldPos;
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            if (currentState == State.Move)
            {
                PickTarget();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            moveSpeed = 0.5f;
        }
        if (collision.gameObject.tag == "Potato" && collision.gameObject.GetComponent<GridObject>().isDragging == false)
        {
            currentPotato = collision.gameObject;
            Debug.Log("Entered potato");
            currentState = State.MoveToPotato;

            Vector2Int potatoCell = grid.WorldToGrid(collision.transform.position);
            Vector2Int currentCell = grid.WorldToGrid(transform.position);

            targetCell = potatoCell;
            path = bfs.FindPath(currentCell, targetCell);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            moveSpeed = defaultSpeed;
        }
    }

    void UpdateSprite(Vector2Int direction)
    {
        if (direction == Vector2Int.up)
        {
            spriteRenderer.sprite = spriteUp;
        }
        else if (direction == Vector2Int.down)
        {
            spriteRenderer.sprite = spriteDown;
        }
        else if (direction == Vector2Int.left)
        {
            spriteRenderer.sprite = spriteLeft;
        }
        else if (direction == Vector2Int.right)
        {
            spriteRenderer.sprite = spriteRight;
        }
    }

    public void Die()
    {
        playerShapeManager.SetCurrentCount(-1);
        isDead = true;
        Destroy(gameObject);
    }
}