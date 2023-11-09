using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBehaviour : MonoBehaviour
{

    private bool hasExploded = false;
    private float timer= -1f;
    public void WhenDying() {
        this.GetComponent<Animator>().SetBool("Dead", true);
        hasExploded = true;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0)
        {
            if (hasExploded && timer >=0.5f)
            {
                Destroy(this.gameObject);
            }
            timer = timer + Time.deltaTime;
        }
    }
}
