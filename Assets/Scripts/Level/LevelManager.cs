using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    LevelUIManager levelUIManager;
    public GameObject[] set1Levels;
    public bool[] completedLevels;

    string hasPlayed = "HasNotPlayedBefore";
    string saveString = "";
    public int currentLevel = 0;
    public int set = 1;

    public int levelsInSet = 20;

    GameObject loadedLevel;


    [SerializeField] private GameObject fail;
    [SerializeField] private GameObject success;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string findPlayedBefore = PlayerPrefs.GetString("HasPlayed", "HasNotPlayedBefore");

        if (findPlayedBefore == "HasNotPlayedBefore")
        {
            Debug.Log("Has not played before");
            findPlayedBefore = "HasPlayedBefore";
            PlayerPrefs.SetString("HasPlayed", findPlayedBefore);
            BeginGame();
        }
        LoadGame();
        SaveGame();

        levelUIManager = GetComponent<LevelUIManager>();
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

    public void BeginGame()
    {
        completedLevels = new bool[levelsInSet];
        saveString = "00000000000000000000";
        PlayerPrefs.SetString("Completed Levels", saveString);
        PlayerPrefs.Save();

        for (int i = 0; i < levelsInSet; i++)
        {
            completedLevels[i] = false;
        }
    }


    public void SaveGame()
    {
        saveString = "";
        for (int i = 0; i < levelsInSet; i++)
        {
            if (completedLevels[i] == true)
            {
                saveString += "1";
            }
            else
            {
                saveString += "0";
            }
        }
        PlayerPrefs.SetString("Completed Levels", saveString);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        saveString = PlayerPrefs.GetString("Completed Levels", "00000000000000000000");

        completedLevels = new bool[levelsInSet];

        for (int i = 0; i < levelsInSet; i++)
        {
            if (saveString[i] == '1')
            {
                completedLevels[i] = true;
            }
            else
            {
                completedLevels[i] = false;
            }
        }



    }



    public void LoadLevel(int levelNum)
    {
        levelUIManager.ToggleLevelSelect(false);
        levelUIManager.ToggleLevelUI(true);
        Vector2 pos = new Vector2(0, 0);
        loadedLevel = Instantiate(set1Levels[levelNum], pos, Quaternion.identity);

    }

    public void UnloadLevel()
    {
        Destroy(loadedLevel);
    }

    public void CompleteLevel()
    {
        currentLevel++;
    }


    public void LevelComplete()
    {
        //Store this level as complete if not already completed
        if (!completedLevels[currentLevel])
        {
            completedLevels[currentLevel] = true;
            SaveGame();
        }

        if (success.activeInHierarchy)
        {
            success.SetActive(false);
        }
        else
        {
            success.SetActive(true);
        }
    }

    public void levelFailed()
    {
        if (fail.activeInHierarchy)
        {
            fail.SetActive(false);
        }
        else
        {
            fail.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RetryLevel()
    {
        Debug.Log("Retry Level");
        UnloadLevel();
        StartCoroutine(RetryLevelAfterDelay());
        if (fail.activeInHierarchy)
        {
            fail.SetActive(false);
        }
    }

    public void NextLevel()
    {
        UnloadLevel();
        StartCoroutine(SwapLevelAfterDelay());

        if (success.activeInHierarchy)
        {
            success.SetActive(false);
        }
    }

    IEnumerator SwapLevelAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        CompleteLevel();
        LoadLevel(currentLevel);
    }

    IEnumerator RetryLevelAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        LoadLevel(currentLevel);
    }


    public void SpeedUpGame()
    {
        //Speed up players
        Move[] players = FindObjectsByType<Move>(FindObjectsSortMode.InstanceID);

        foreach (Move player in players)
        {
            player.ChangeSpeed();
        }

        //Speed up enemies
        EnemyMove[] enemies = FindObjectsByType<EnemyMove>(FindObjectsSortMode.InstanceID);

        foreach (EnemyMove enemy in enemies)
        {
            enemy.ChangeSpeed();
        }


    }

}
