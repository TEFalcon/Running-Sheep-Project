using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class GameManagerBehaviour : MonoBehaviour
{
    private int RabitsAlive;


    //the tile manager script:
    [SerializeField] private GridMan GridMan;
    [SerializeField] private MouseManager MouseMan;
    [SerializeField] private FoxSpawner FoxSpawnerObj;

    [SerializeField] private int score = 0;
    private float timerForScore;
    private bool lostTheGame;

    private int highScore;

    //UI veriable
    [SerializeField] private TextMeshProUGUI ScoreCounterTestMesh;
    [SerializeField] private TextMeshProUGUI ScoreMultTextMesh;
    [SerializeField] private GameObject openingScreen;
    [SerializeField] private GameObject BtnResume;
    [SerializeField] private GameObject BtnStart;
    [SerializeField] private GameObject BtnRules;
    [SerializeField] private GameObject BtnPause;
    [SerializeField] private TextMeshProUGUI HighScoreText1;
    [SerializeField] private TextMeshProUGUI HighScoreText2;

    // Start is called before the first frame update
    private void Start()
    {
        MouseMan = GameObject.Find("Mouse").GetComponent<MouseManager>();
        highScore = score = 0;
        lostTheGame = true;
        //set up grid:
        GridMan.StartUp();

        showOnStart();
    }

    // Update is called once per frame
    private void Update()
    {
        if (RabitsAlive < 2 && lostTheGame ==false) {
            GameOver();
        }
        if (lostTheGame == false)
        {
            timerForScore = timerForScore +(1* Time.deltaTime);
            Debug.Log(timerForScore);
            if (timerForScore > 5f) {
                UpdateScore();
                timerForScore= 0f;
            }
        }
    }

    //called every time the score is going up - every 60 frames
    private void UpdateScore(){
            score = score + RabitsAlive;
            ScoreCounterTestMesh.SetText("Score: " + score);
    }

    //when a rabbit got eaten
    public void RabbitDied()
    {
        RabitsAlive--;
        ScoreMultTextMesh.SetText("Multiplier: x" + RabitsAlive);
    }
    public bool GetLostTheGame() { return lostTheGame; }

    //when all the rabbits died - game over, this is called
    private void GameOver() {
        Debug.Log("Game Over!");
        lostTheGame=true;
        FoxSpawnerObj.stopSpawning();
        UIBtnHandler btnHandler = GameObject.Find("Main Camera").GetComponent<UIBtnHandler>();
        btnHandler.SetPlayingGame(false);

        showOnRetry();
        //destroying the ramaining Rabbit
        GridMan.KillLastBunny();
    }

    public void showOnStart()
    {
        BtnResume.SetActive(false);
        BtnPause.SetActive(false);
        BtnStart.SetActive(true);
        HighScoreText1.SetText("High Score: -");
        HighScoreText2.SetText("High Score: -");
        MouseMan.GotInMenu();

        openingScreen.SetActive(true);
    }
    public void showOnPause()
    {
        BtnResume.SetActive(true);
        BtnPause.SetActive(true);
        BtnStart.SetActive(false);
        MouseMan.GotInMenu();

        openingScreen.SetActive(true);
    }
    public void showOnRetry()
    {
        if (highScore < score) { highScore = score; }
        BtnResume.SetActive(false);
        BtnPause.SetActive(false);
        BtnStart.SetActive(true);
        HighScoreText1.SetText("High Score: "+ highScore);
        HighScoreText2.SetText("High Score: " + highScore);
        MouseMan.GotInMenu();

        openingScreen.SetActive(true);
    }

    public void ResumeGame()
    {
        BtnPause.SetActive(true);
        openingScreen.SetActive(false);
    }

    //for when clicked 
    public void StartNewGame()
    {
        
        score = 0;
        ScoreCounterTestMesh.SetText("Score: 0");
        RabitsAlive = 4;
        ScoreMultTextMesh.SetText("Multiplier: x" + RabitsAlive);
        GridMan.ResetGameArray();
        timerForScore = 0f;
        lostTheGame = false;
        BtnPause.SetActive(true);
        openingScreen.SetActive(false);
        FoxSpawnerObj.startSpawning();
        Debug.Log("New Game Started!");
    }
}
