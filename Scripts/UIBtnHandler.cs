using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnHandler : MonoBehaviour
{

    [SerializeField] private GameManagerBehaviour gameManager;
    [SerializeField] private MouseManager mouseMan;

    private bool paused;
    private bool PlayingGame;
    //UI variables


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        mouseMan = GameObject.Find("Mouse").GetComponent<MouseManager>();
        paused= true;
        PlayingGame = false;
    }
    public void SetPlayingGame(bool b) {
        this.PlayingGame = b;
    }

    public void StartBtnClicked() {
        paused = false;
        gameManager.StartNewGame();
        mouseMan.GotOutMenu();
        PlayingGame= true;
    }

    public void ResumeBtnClicked() {
        paused = false;
        mouseMan.GotOutMenu();
        gameManager.ResumeGame();
    }

    public void CreditsBtnClicked()
    {
        paused = true;
        mouseMan.GotInMenu();
    }

    public void RulesBtnClicked() {
        
    }

    public void PauseBtnClicked() {
        if (paused == false)
        {
            paused = true;
            mouseMan.GotInMenu();
            gameManager.showOnPause();
            
        }
        else {ResumeBtnClicked(); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayingGame == true) {
            if (paused) {
                ResumeBtnClicked();
            }
            else
            {
                PauseBtnClicked();
            }
        }
    }
}
