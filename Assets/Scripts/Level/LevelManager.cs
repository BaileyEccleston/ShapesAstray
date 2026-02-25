using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject[] set1Levels;
    public int currentLevel = 0;
    public int set = 1;

    public int levelsInSet;

    GameObject loadedLevel;

    public GridScript gridManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            LoadLevel(currentLevel);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnloadLevel();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CompleteLevel();
        }
    }


    void LoadLevel(int levelNum)
    {
        Vector2 pos = new Vector2 (0, 0);
        loadedLevel = Instantiate(set1Levels[currentLevel], pos, Quaternion.identity);

    }

    void UnloadLevel()
    {
        Destroy(loadedLevel);
    }

    void CompleteLevel()
    {
        currentLevel++;
    }
}
