using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[System.Serializable]
public class SpriteVariant
{
    public Sprite[] up;
    public Sprite[] down;
    public Sprite[] left;
    public Sprite[] right;
}


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

    public enum Shape
    {
        Circle,
        Triangle,
        Square,
    }

    public enum Colour
    {
        Blue,
        Pink,
        Orange
    }

    bool isDead = false;
    bool isSafe = false;

    [SerializeField] GameObject currentPotato;
    Potato potato;

    [SerializeField] private SpriteRenderer spriteRenderer;

    PlayerShapeManager playerShapeManager;




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

    int moveSpeedMultiplyer = 1;
    public int maxMoveSpeedMultiplyer;

    public float spedUpSpeed;
    public float defaultSpeed;

    public Vector2Int targetCell;

    private List<Vector2Int> path = new List<Vector2Int>();

    State currentState;

    public bool targeted = false;

    public GameObject splat;

    Transform parent;

    private void Awake()
    {
        grid = FindAnyObjectByType<GridScript>();
        playerShapeManager = FindAnyObjectByType<PlayerShapeManager>();
    }

    private void Start()
    {
        parent = transform.parent;
        int randomVariant = Random.Range(0, variants.Length);
        currentVariant = variants[randomVariant];
        currentState = State.Move;
        PickTarget();
    

    }

    private void Update()
    {
        if (currentState == State.Move || currentState == State.MoveToPotato)
        {
            FollowPath();
        }

        if (currentState == State.MoveToPotato && path.Count == 0 && currentPotato != null)
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

        if (targeted)
        {
            spriteRenderer.color = Color.red;
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
            if (spedUpSpeed > 1)
            {
                moveSpeed = spedUpSpeed / 4;
            }
            else
            {
                moveSpeed = defaultSpeed / 4;
            }
        }
        if (collision.gameObject.tag == "Fire")
        {
            Die();
        }
        if (collision.gameObject.tag == "Water")
        {
            Water water = collision.gameObject.GetComponent<Water>();
            if (water.waterDepthIncreaseCurrentAmount >= water.waterDepthIncreaseFinalAmount)
            {
                Die();
            }
        }
        if (collision.gameObject.tag == "Trap")
        {
            Die();
        }

    }

    public void TargetPotato(GameObject potato)
    {
        if (potato.tag == "Potato" && potato.gameObject.GetComponent<GridObject>().isDragging == false)
        {
            currentPotato = potato.gameObject;
            Debug.Log("Entered potato");
            currentState = State.MoveToPotato;

            Vector2Int potatoCell = grid.WorldToGrid(potato.transform.position);
            Vector2Int currentCell = grid.WorldToGrid(transform.position);

            targetCell = potatoCell;
            path = bfs.FindPath(currentCell, targetCell);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light")
        {
            if (spedUpSpeed > 1)
            {
                moveSpeed = spedUpSpeed;
            }
            else
            {
                moveSpeed = defaultSpeed;
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

    public void Die()
    {
        playerShapeManager.Dead();
        isDead = true;
        Instantiate(splat, transform.position, Quaternion.identity, parent);
        Destroy(gameObject);
    }

    public void ChangeSpeed()
    {
        moveSpeedMultiplyer++;
        if (moveSpeedMultiplyer > maxMoveSpeedMultiplyer)
        {
            moveSpeedMultiplyer = 1;
            moveSpeed = defaultSpeed;
            spedUpSpeed = 1;
        }
        spedUpSpeed = defaultSpeed * moveSpeedMultiplyer;
        moveSpeed = spedUpSpeed;
    }
}