using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  //  [SerializeField] private GameObject CreditsObj;
   // [SerializeField] private GameObject HowToPlayObj;
    [SerializeField] private GameObject MenuObj;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    /*   public void ToggleCredits()
    {
        if (CreditsObj.activeInHierarchy)
        {
            CreditsObj.SetActive(false);
            MenuObj.SetActive(true);
        }
        else
        {
            CreditsObj.SetActive(true);
            MenuObj.SetActive(false);
        }
    }

    public void HowToPlay()
    {
        if (HowToPlayObj.activeInHierarchy)
        {
            HowToPlayObj.SetActive(false);
            MenuObj.SetActive(true);
        }
        else
        {
            HowToPlayObj.SetActive(true);
            MenuObj.SetActive(false);
        }
    }  */

    public void Quit()
    {
        Application.Quit();
    }


    /*
    public void HowToPlayBack()
    {
        HowToPlayObj.SetActive(false);
        MenuObj.SetActive(true);
    }

    public void CreditsBack()
    {
        CreditsObj.SetActive(false);
        MenuObj.SetActive(true);
    }  */
}
