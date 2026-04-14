using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScript : MonoBehaviour
{
    public Tilemap levelTileMap;


    public GameObject leftDoor;
    public GameObject upDoor;
    public GameObject potato;
    public GameObject rottenPotato;
    Transform parent;


    public int gridWidth = 36;
    public int gridHeight = 16;

    public Vector2 gridOrigin = new Vector2(-17.5f, -7.5f);

    public float cellSize = 1;


    public TileType[,] levelGrid;
    public bool[,] lit;

    private void Awake()
    {



        parent = transform.parent;
        levelGrid = new TileType[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                levelGrid[x, y] = TileType.none;

                Vector3Int tilePos = levelTileMap.WorldToCell(GridToWorld(x,y));
               
              //  Debug.Log(wallTileMap.GetSprite(tilePos));

                Sprite sprite = levelTileMap.GetSprite(tilePos);

                if (sprite != null)
                {
                    Vector2 pos;
                    // This outputs all the names of the sprite in every tile
                    // This was used to see check if the name of a tilemap tile could be read
                    // This then let me reduce the amount of tilemaps to one
                    //Debug.Log(sprite.name + sprite.name[0]);
                    switch(sprite.name[0])
                    {

                        case 'W':
                            levelGrid[x, y] = TileType.wall;
                            break;
                        case 'B':
                            levelGrid[x,y] = TileType.bottomWall;
                            break;
                        case 'F':
                            levelGrid[x, y] = TileType.floor;
                            break;
                        case 'P':
                            levelGrid[x, y] = TileType.startPos;
                            break;
                        case 'O':
                            levelGrid[x, y] = TileType.grandmaStartPos;
                            break;
                        case 'G':
                            levelGrid[x, y] = TileType.goal;
                            break;
                        case 'E':
                            levelGrid[x, y] = TileType.enemySpawn;
                            break;
                        case 'L':
                            pos = GridToWorld(x, y);
                            Instantiate(leftDoor, pos, Quaternion.identity, parent);
                            break;
                        case 'U':
                            pos = GridToWorld(x, y);
                            Instantiate(upDoor, pos, Quaternion.Euler(0, 0, -90), parent);
                            break;
                        case 'p':
                            pos = GridToWorld(x, y);
                            levelGrid[x, y] = TileType.potato;
                            Instantiate(potato, pos, Quaternion.identity, parent);
                            break;
                        case 'R':
                            pos = GridToWorld(x, y);
                            levelGrid[x, y] = TileType.rottenPotato;
                            Instantiate(rottenPotato, pos, Quaternion.identity, parent);
                            break;
                    }
                }

            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    private void Update()
    {
        
    }

    public Vector2 GridToWorld(int gridX, int gridY)
    {
        float worldX = gridOrigin.x + (gridX * cellSize);
        float worldY = gridOrigin.y + (gridY * cellSize);

        return new Vector2(worldX, worldY);
    }

    public Vector2 GridToWorld(Vector2Int cell)
    {
        return GridToWorld(cell.x, cell.y);
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        int x = Mathf.FloorToInt((worldPos.x - gridOrigin.x) / cellSize);
        int y = Mathf.FloorToInt((worldPos.y - gridOrigin.y) / cellSize);

        return new Vector2Int(x, y);
    }

    public bool IsWalkable(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x >= gridWidth || cell.y < 0 || cell.y >= gridHeight)
        {
            return false; 
        }
        return levelGrid[cell.x, cell.y] == TileType.floor || levelGrid[cell.x, cell.y] == TileType.openDoor || levelGrid[cell.x, cell.y] == TileType.potato || levelGrid[cell.x, cell.y] == TileType.startPos || levelGrid[cell.x, cell.y] == TileType.enemySpawn || levelGrid[cell.x, cell.y] == TileType.goal || levelGrid[cell.x, cell.y] == TileType.enemySpawn || levelGrid[cell.x, cell.y] == TileType.water || levelGrid[cell.x, cell.y] == TileType.grandmaStartPos;
    }

    public bool IsEndGoal(Vector2Int cell)
    {
        if (cell.x < 0 || cell.x >= gridWidth || cell.y < 0 || cell.y >= gridHeight)
        {
            return false;
        }
        return levelGrid[cell.x, cell.y] == TileType.goal;
    }



}
