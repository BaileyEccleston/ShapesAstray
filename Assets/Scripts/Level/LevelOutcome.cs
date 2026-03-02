using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOutcome : MonoBehaviour
{

    [SerializeField] private GameObject fail;
    [SerializeField] private GameObject success;

    public LevelManager levelManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        levelManager.UnloadLevel();
        levelManager.LoadLevel(levelManager.currentLevel);
        if (fail.activeInHierarchy)
        {
            fail.SetActive(false);
        }
    }

    public void NextLevel()
    {
        levelManager.UnloadLevel();
        levelManager.CompleteLevel();
        levelManager.LoadLevel(levelManager.currentLevel);
        if (success.activeInHierarchy)
        {
            success.SetActive(false);
        }
    }

}
