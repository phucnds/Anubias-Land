using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public GameObject player;

    enum STATE
    {
        Idle = 0,
        Regen = 1,
        Work = 2,
        Conquest = 3
    }

    private bool isActive = false;
    private float timer;
    private STATE state;


    void Start()
    {
        isActive = true;
        timer = 5;
        state = STATE.Idle;
    }


    private void Update()
    {

        if (!isActive) return;

        timer -= Time.deltaTime;

        switch (state)
        {
            case STATE.Idle:
                Debug.Log("Idle");
                break;
            case STATE.Regen:
                Debug.Log("Regen");
                break;
            case STATE.Work:
                Debug.Log("Work");
                break;
            case STATE.Conquest:
                Debug.Log("Conquest");
                break;


        }



        if (timer <= 0)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case STATE.Idle:
                state = STATE.Regen;
                timer = 5f;
                break;
            case STATE.Regen:
                state = STATE.Work;
                timer = 5f;
                break;
            case STATE.Work:
                state = STATE.Conquest;
                timer = 5f;
                break;
            case STATE.Conquest:
                state = STATE.Idle;
                timer = 5f;
                break;
        }
    }
}
