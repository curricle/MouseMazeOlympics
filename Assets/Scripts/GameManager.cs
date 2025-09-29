using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Text displayCheesePoints;
    public Text displayTimeLeft;
///////////////////////////////////////////
    public int totalPlayerScore;
    public int currentCheesePoints;
    public int totalLevelCheesePoints;
////////////////////////////////////////////
    public int playerErrorCount;
    public int totalErrorCount;
    
///////////////////////////////////////////
    public float timer;
    public float totalTimeToCompleteLevel;
    //public static bool gameIsPaused;
///////////////////////////////////////////

    public int totalLevels;
    public int currentLevel;

///////////////////////////////////////////

    public GameObject GameOverMenu;

///////////////////////////////////////////    
    public void Start()
    {
        timer = totalTimeToCompleteLevel;
        currentCheesePoints = 0;
        GameOverMenu.SetActive(false);
    }
    //TODO:Add Pause of game
 
    public void Update()
    {
        //PauseGame();
        displayCheesePoints.text = currentCheesePoints.ToString();
        displayTimeLeft.text = timer.ToString();

        ControlTimer();
        CheckForWinCondition();
    }

    public void CheckForWinCondition()
    {
        if (currentCheesePoints >= totalLevelCheesePoints)
        {
            Debug.Log("You collected all the cheese!");
            PauseMenu.gamePaused = true;
            WinGame();
        }
    }

    public void PauseGame()
    {
        PauseMenu.gamePaused = true;
    }
    public void WinGame()
    {
        Debug.Log("High score screen! You won!");
    }
    public void LoseGame()
    {
        GameOverMenu.SetActive(true);
    }
    public void ControlTimer()
    {
     if(PauseMenu.gamePaused == false)
        {
            Time.timeScale = 1;
            timer -= Time.deltaTime;
            if (timer < 0)
            {
            timer = 0;
            PauseMenu.gamePaused = true;
            Debug.Log("Time's up! Retry Trial?");
            LoseGame();
            }
        }
       
    }
    public void AddCheesePointsToPlayer()
    {
        currentCheesePoints++;
    }

    public void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name.Contains("Big Cheese"))
            {
                AddCheesePointsToPlayer();
                col.gameObject.tag = "collected";
                //play collected sound!
            }
        else{
            Debug.Log("Go find cheese!!");
        }
    }

}
