using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MenuObj;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    public void Quit()
    {
        Application.Quit();
    }
}
