using TMPro;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{

    LevelManager levelManager;
    int displayedLevelNum;
    public TMP_Text displayedLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayedLevelNum = 1;
        displayedLevel.text = displayedLevelNum.ToString();
        levelManager = GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginLevel()
    {   
        levelManager.LoadLevel(displayedLevelNum - 1);
    }

    public void DisplayNextLevel()
    {
        if (levelManager.completedLevels[displayedLevelNum] == true || (levelManager.completedLevels[displayedLevelNum - 1] == true && levelManager.completedLevels[displayedLevelNum] == false && levelManager.completedLevels[displayedLevelNum + 1] == false))
        {
            displayedLevelNum++;
            displayedLevel.text = displayedLevelNum.ToString();
        }
    }

    public void DisplayPreviousLevel()
    {
        if (displayedLevelNum != 1)
        {
            displayedLevelNum--;
            displayedLevel.text = displayedLevelNum.ToString();
        }
    }
}
