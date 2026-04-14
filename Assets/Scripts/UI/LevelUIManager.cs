using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] GameObject levelUI;
    [SerializeField] GameObject levelSelect;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelSelect.SetActive(true);
        levelUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleLevelSelect(bool toggle)
    {
        if (toggle)
        {
            levelSelect.SetActive(true);
        }
        else
        {
            levelSelect.SetActive(false);
        }
    }

    public void ToggleLevelUI(bool toggle)
    {
        if (toggle)
        {
            levelUI.SetActive(true);
        }
        else
        {
            levelUI.SetActive(false);
        }
    }



}
