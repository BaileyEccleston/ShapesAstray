using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    bool paused = false;
    public GameObject pauseElements;

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
}
