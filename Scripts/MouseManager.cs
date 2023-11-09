using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private GameObject objSelected = null;
    private Vector3 oldPosForCurrentObjHold;

    [SerializeField] private Vector3 ScreenPosition;
    [SerializeField] private Vector3 WorldPosition;
    [SerializeField] private GameObject MouseObj;

    private Animator animator;

    //audio sounds
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip grabClip;
    [SerializeField] private AudioClip landClip;


    //for changing the grid
    [SerializeField] private GridMan grid;

    private const string GRABBING = "Grabbing";
    private const string FOXTAG = "fox";
    private const string RABBITTAG = "rabbit";
    private const string MOUSEMENUPARAMETER = "IsInMenu";

    private void Start()
    {
        animator = MouseObj.GetComponent<Animator>();
        GotInMenu();
        //unless btn pressed changed it..

        Cursor.visible = false;
        
    }

    public void GotInMenu() {
        this.animator.SetBool(MOUSEMENUPARAMETER,true);
    }

    public void GotOutMenu() { 
       animator.SetBool(MOUSEMENUPARAMETER,false);
    }


    // Update is called once per frame
    void Update()
    {
        ScreenPosition = Input.mousePosition;

        WorldPosition = Camera.main.ScreenToWorldPoint(ScreenPosition);
        WorldPosition.z = 0f;
        MouseObj.transform.position= WorldPosition;

        //if going out of screen turning off and on cursur
        if (ScreenPosition.x > Screen.width || ScreenPosition.x < 0 || ScreenPosition.y > Screen.height || ScreenPosition.y < 0)
        {
            //maybe pause the game
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }
        //RaycastHit2D hit2D = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(ScreenPosition));
        //if (hit2D.collider != null && !animator.GetBool(MOUSEMENUPARAMETER)) {
        //    if (hit2D.transform.gameObject.tag == "Btn") { GotInMenu(); }
        //    else { GotOutMenu(); }
        //}


        //grabbing  
        if (Input.GetMouseButtonDown(0)) {
            //grabbing animation
            animator.SetBool(GRABBING, true);
            audioSource.PlayOneShot(grabClip);

            //ray casting to check if we hit an object and which one
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(ScreenPosition));

            if (hit2D.collider != null) {
                objSelected = hit2D.transform.gameObject;
                if (objSelected.tag != FOXTAG)
                {
                    //updating the grid that the bunny isnt there anymore
                    grid.RemoveBunnyFromTile(objSelected.transform.position);

                    //save original position of object
                    oldPosForCurrentObjHold = objSelected.transform.position;
                    objSelected.tag = "Untagged";
                }
                else
                {
                    objSelected = null;
                }
            }

            
        }

        //dragging
        if (Input.GetMouseButton(0) && objSelected != null) {
            //dragging to where the mouse is currently is
            objSelected.transform.position = WorldPosition;
        }

        //ungrabbing
        if (Input.GetMouseButtonUp(0) && animator.GetBool(GRABBING)) {
            //ungrabbing animation
            animator.SetBool(GRABBING, false);
            if (objSelected != null ) 
            {
                //snapping - and if unable to snap: return to original position
                if (CheckIfInRanch((Vector3)ScreenPosition))
                {
                    //ungrabbing the object in the new position + updating the tile
                    SnapToRanch();
                    audioSource.PlayOneShot(landClip);

                }
                else {
                    ReturningToOldPos();
                    audioSource.PlayOneShot(landClip);
                }
                objSelected.tag = RABBITTAG;
                objSelected = null;
            }
            
        }

    }

    private void SnapToRanch() {
        Vector2 newSnap = grid.AddBunnyToTile(objSelected.transform.position);
        if (newSnap != (new Vector2(100f,100f))) {
            objSelected.transform.position = newSnap;
        }
        else
        {
            ReturningToOldPos();
        }
    }

    private void ReturningToOldPos() {
        objSelected.transform.position = oldPosForCurrentObjHold;
        grid.GotBackToTile();
        Debug.Log("taken");
    }
    private bool CheckIfInRanch(Vector3 pos) {
        if (ScreenPosition.x > 620f || ScreenPosition.x < 300f || ScreenPosition.y > 400f || ScreenPosition.y < 100f)
        { 
            return false;
        }
        return true;
        }




}
