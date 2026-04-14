using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseElements;
    public LevelManager levelManager;
    public LevelUIManager levelUIManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //play
            if(paused)
            {
                Resume();
            }
            //pause
            else
            {
                Pause();
            }


        }
    }

    public void Pause()
    {
        if (!pauseElements.activeInHierarchy)
        {
            pauseElements.SetActive(true);
        }
        paused = true;
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        if (pauseElements.activeInHierarchy)
        {
            pauseElements.SetActive(false);
        }
        paused = false;
        Time.timeScale = 1.0f;
    }

    public void LevelSelect()
    {
        if (pauseElements.activeInHierarchy)
        {
            pauseElements.SetActive(false);
        }
        levelManager.UnloadLevel();
        levelUIManager.ToggleLevelSelect(true);
        levelUIManager.ToggleLevelUI(false);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
    }
}
