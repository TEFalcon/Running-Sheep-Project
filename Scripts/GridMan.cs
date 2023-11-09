using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMan : MonoBehaviour
{
    [SerializeField] private int[][] takenArray;//0 is empty, 1 is bunny
    [SerializeField] private Vector2[][] LocationsArray;
    [SerializeField] private List<GameObject> RabbitsAlive;

    private Vector2 LastPosOfBunny;

    public GameObject Rabit;

    private const int SIZE = 6;
    private Vector2 InitPos = new Vector2(-3f,4.5f);
    private Vector2 RunnerPos;

    
    public void StartUp() {
        
        LocationsArray = new Vector2[SIZE][];
        takenArray = new int[SIZE][];
        RunnerPos = InitPos;
        for (int i = 0; i < SIZE; i++)
        {
            LocationsArray[i] = new Vector2[SIZE];
            takenArray[i] = new int[SIZE];
            for (int j = 0; j < SIZE; j++)
            {
                //creating a full locations array
                LocationsArray[i][j] = RunnerPos;
                takenArray[i][j] = 0;

                //advancing the tile in the row
                RunnerPos.x++;
           }
            //going down a row 
            RunnerPos.x = InitPos.x;
            RunnerPos.y--;
        }
        
    }

    //when creating a new game
    public void ResetGameArray() {
        RabbitsAlive = new List<GameObject>(4);
        RunnerPos = InitPos;
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                //Reseting the Taken array
                takenArray[i][j] = 0;

                //advancing the tile in the row
                RunnerPos.x++;
            }
            //going down a row 
            RunnerPos.x = InitPos.x;
            RunnerPos.y--;
        }

        //adding to taken array
        takenArray[1][1] = 1;
        takenArray[4][1] = 1;
        takenArray[1][4] = 1;
        takenArray[4][4] = 1;
        //creating the bunnies in the locations
        RabbitsAlive.Add( Instantiate(Rabit, new Vector3(LocationsArray[1][1].x, LocationsArray[1][1].y, 0), Quaternion.identity));
        RabbitsAlive.Add(Instantiate(Rabit, new Vector3(LocationsArray[4][1].x, LocationsArray[4][1].y, 0), Quaternion.identity));
        RabbitsAlive.Add(Instantiate(Rabit, new Vector3(LocationsArray[1][4].x, LocationsArray[1][4].y, 0), Quaternion.identity));
        RabbitsAlive.Add(Instantiate(Rabit, new Vector3(LocationsArray[4][4].x, LocationsArray[4][4].y, 0), Quaternion.identity));
        Debug.Log(RabbitsAlive.Count);
    }

    public Vector2 AddBunnyToTile(Vector2 point) {
        Vector2 newSnap = ClosestTileToLocation(point);
        if (takenArray[(int)newSnap.x][(int)newSnap.y] == 0) {
            takenArray[(int)newSnap.x][(int)newSnap.y] = 1;
            return LocationsArray[(int)newSnap.x][(int)newSnap.y];
        }
        else 
            return new Vector2(100f,100f);//just to flag its false
    }

    public void KillLastBunny() {
        if (RabbitsAlive != null) {
            RemoveBunnyFromTile((Vector2)RabbitsAlive[0].transform.position);
            Destroy(RabbitsAlive[0]);
            RabbitsAlive.RemoveAt(0);
        }
    }
    public void BunnyKilled(Vector3 bunnyPos)
    {
        bool flag = false;
        for (int i = 0; i < RabbitsAlive.Count; i++)
        {
            if (flag == false && RabbitsAlive[i].transform.position.Equals(bunnyPos)) 
            {
                RemoveBunnyFromTile((Vector2)bunnyPos);
                RabbitsAlive.Remove(RabbitsAlive[i]);
                flag =true;
            }
        }
    }
    public void RemoveBunnyFromTile(Vector2 point)
    {
        LastPosOfBunny = ClosestTileToLocation(point);
        takenArray[(int)LastPosOfBunny.x][(int)LastPosOfBunny.y] = 0;
    }

    public void GotBackToTile() {
        takenArray[(int)LastPosOfBunny.x][(int)LastPosOfBunny.y] = 1;
    }

    //the Vector2 Returning is the i and j of locations array - so will be an int
    private Vector2 ClosestTileToLocation(Vector2 point) {
        float DistanceOfSnap = Vector2.Distance(LocationsArray[0][0], point);
        int i_end = 0, j_end = 0;
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                float distance = Vector2.Distance(LocationsArray[i][j], point);

                if (distance < DistanceOfSnap)
                {
                    DistanceOfSnap = distance;
                    i_end = i;
                    j_end = j;
                }

            }

        }
        return new Vector2(i_end,j_end);
    }


}
