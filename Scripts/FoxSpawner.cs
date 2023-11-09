using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FoxSpawner : MonoBehaviour
{
    //private Vector2[] LocationsArray;
    private Vector2[] StartingPosArray;

    [SerializeField] private GameObject foxObj;

    [SerializeField] private float Timer;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float NumToSpawn;

    private float countTimer;
    private bool SpawnWorking;
    [SerializeField] private float counterOfFoxesSpawned;

    //const strings
    private const string FOXAXISX = "X";
    private const string FOXAXISY = "Y";

    private void Start()
    {
        foxObj = Resources.Load("Prefabs/Fox", typeof(GameObject)) as GameObject;
        StartingPosArray = new Vector2[4];//0 is top, 1 is bot, 2 is right, 3 is left
        //top - goes from left to right
        StartingPosArray[0] = new Vector2(-3f, 6.5f);//top
        //bot - goes from left to right
        StartingPosArray[1] = new Vector2(-3f, -2.5f);//bot
        //right - goes from top to bot
        StartingPosArray[2] = new Vector2(4f, 4.5f);//right
        //left - goes from bot to top
        StartingPosArray[3] = new Vector2(-5f, 4.5f);//left
    }
    public void startSpawning()
    {
        
        Timer = 120f;
        MovementSpeed = 1.5f;
        NumToSpawn = 1;
        countTimer= 0;
        counterOfFoxesSpawned= 0;
        SpawnWorking= true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SpawnWorking)
        {
            countTimer++;
            if (countTimer == Timer)
            {
                SpawnFox();
                countTimer = 0;
            }
        }
    }

    public void stopSpawning() {
        SpawnWorking= false;
    }
    private void SpawnFox() {
        //spawning the fox - the amount of times that needs to be spawned in that time
        for (int i = 0; i < NumToSpawn; i++)
        {
            int rndLocal = Random.Range(0, 4);
            int rndJump = Random.Range(0, 5);
            GameObject gb;
            if (rndLocal == 0)
            {
                gb = Instantiate(foxObj,
                   new Vector3(StartingPosArray[rndLocal].x + rndJump, StartingPosArray[rndLocal].y)
                   , Quaternion.identity);
                gb.GetComponent<Animator>().SetFloat(FOXAXISY, -1f);
            }
            else if (rndLocal == 1) {
                gb = Instantiate(foxObj,
                   new Vector3(StartingPosArray[rndLocal].x + rndJump, StartingPosArray[rndLocal].y)
                   , Quaternion.identity);
                gb.GetComponent<Animator>().SetFloat(FOXAXISY, 1f);
            }

            else if (rndLocal == 2) {
                gb = Instantiate(foxObj,
                   new Vector3(StartingPosArray[rndLocal].x, StartingPosArray[rndLocal].y - rndJump)
                   , Quaternion.identity);
                Animator anim = gb.GetComponent<Animator>();
                gb.GetComponent<Animator>().SetFloat(FOXAXISX, -1f);
            }

            else {
                gb = Instantiate(foxObj,
                   new Vector3(StartingPosArray[rndLocal].x, StartingPosArray[rndLocal].y - rndJump)
                   , Quaternion.identity);
                gb.GetComponent<Animator>().SetFloat(FOXAXISX, 1f);
            }
            gb.GetComponent<FoxBehaviour>().SetMS(MovementSpeed);
            gb.GetComponent<FoxBehaviour>().StartAfterSpawn();
            
            
            
        }
        //for increasing the dificulty:
        counterOfFoxesSpawned++;
        if (counterOfFoxesSpawned % 6 == 0) {
            int rndLocal = Random.Range(1, 3);
            if (rndLocal == 1) {//decreasing Timer
                Timer--;
            }
            if (rndLocal == 2) {//increasing movementSpeed
                MovementSpeed= MovementSpeed + 0.1f;
            }
        }
        if (counterOfFoxesSpawned % 30 == 0)
        {
            //increasing numToSpawn
            NumToSpawn++;

            
        }
    }

    
}
