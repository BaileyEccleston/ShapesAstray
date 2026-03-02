using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public GameObject[] set1Levels;
    public int currentLevel = 0;
    public int set = 1;

    public int levelsInSet;

    GameObject loadedLevel;

    //  public GridScript gridManager;


    [SerializeField] private GameObject fail;
    [SerializeField] private GameObject success;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadLevel(currentLevel);

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


    public void LoadLevel(int levelNum)
    {
        Vector2 pos = new Vector2(0, 0);
        loadedLevel = Instantiate(set1Levels[currentLevel], pos, Quaternion.identity);

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
        UnloadLevel();
        LoadLevel(currentLevel);
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
        yield return new WaitForSeconds(2f);
        CompleteLevel();
        LoadLevel(currentLevel);
    }
}
