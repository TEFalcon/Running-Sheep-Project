using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoxBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject thisFox;

    private Vector2 StartPos;

    //const strings
    private const string FOXAXISX = "X";
    private const string FOXAXISY = "Y";

    private const string ANIMTOLEFT = "Left";
    private const string ANIMTORIGHT = "Right";
    private const string ANIMTOTOP = "Up";
    private const string ANIMTOBOTTOM = "Down";

    private const string RABBITTAG = "rabbit";
    [SerializeField] private float movementSpeed;
    
    //for delaying movement
    private bool canMove;
    private float timerTillMove = 0f;
    private float delay = 1f;

    private bool hasEaten;

    //reference to game manager
    [SerializeField] private GameManagerBehaviour gameMan;
    //reference to grid manager
    [SerializeField] private GridMan gridMan;

    //[SerializeField] private RabbitBehaviour rabbitScript;

    //called when created
    public void StartAfterSpawn()
    {
        canMove =false;
        thisFox= this.gameObject;
        StartPos = thisFox.transform.position;
        gameMan = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
        gridMan = GameObject.Find("GridView").GetComponent<GridMan>();
    }
    // Update is called once per frame
    void Update()
    {
        //when not playing destroy foxes
        if (gameMan.GetLostTheGame()) { Destroy(thisFox); }

        //warnings before start moving
        if (!canMove)
        {
            timerTillMove += Time.deltaTime;
            if (timerTillMove >= delay)
            {
                canMove = true;
                timerTillMove = 0f;
                NowCanMove();
            }
        }

        if (Vector2.Distance(thisFox.transform.position, StartPos) > 9f) {
            Destroy(thisFox);
        }
    }
    //when hits a Rabbit:
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == RABBITTAG && hasEaten ==false)
        {
            collision.GetComponent<RabbitBehaviour>().WhenDying();
            gridMan.BunnyKilled(collision.gameObject.transform.position);
            gameMan.RabbitDied();
            hasEaten= true;

            Destroy(this.gameObject);
        }
    }

    public void SetMS(float MS) {
        movementSpeed = MS;
    }
    private void NowCanMove() {

        //start moving + animation
        //movementSpeed = 1f;

        Rigidbody2D rb = thisFox.GetComponent<Rigidbody2D>();
        
        float x = animator.GetFloat(FOXAXISX);
        float y = animator.GetFloat(FOXAXISY);
        thisFox.GetComponent<SpriteRenderer>().flipX = false;
        if (x != 0)
        {

            rb.velocity = new Vector2((float)(x * movementSpeed), 0);
            if (x < 0)
            {
                animator.Play(ANIMTOLEFT);
                thisFox.transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                animator.Play(ANIMTOLEFT);
                thisFox.transform.localScale = new Vector3(1, 1, 1);
            }

        }
        if (y != 0)
        {
            rb.velocity = new Vector2(0, (float)(y * movementSpeed));
            if (y > 0)
                animator.Play(ANIMTOTOP);
            else
                animator.Play(ANIMTOBOTTOM);

        }
        hasEaten = false;
    }
}
