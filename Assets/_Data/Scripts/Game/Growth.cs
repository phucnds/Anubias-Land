using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Growth : MonoBehaviour
{

    enum STATE
    {
        BEGIN,
        GROWING,
        COMPLETE
    }


    [SerializeField] private Transform stages;

    [SerializeField] private float timerValue;

    private bool grown = false;
    private float timer;
    private STATE state;
    

    private void Start()
    {
        InitTree();
        StartCoroutine(StartGrown());
    }

    private void Update()
    {
        if (!grown) return;

        timer -= Time.deltaTime;


        switch (state)
        {


            case STATE.BEGIN:
                Debug.Log("BEGIN");
                SetPrefabActive(0);
                break;

            case STATE.GROWING:
                Debug.Log("GROWING");
                SetPrefabActive(1);
                break;

            case STATE.COMPLETE:
                Debug.Log("COMPLETE");
                SetPrefabActive(2);
                ActionComplete();
                break;
        }


        if (timer <= 0)
        {
            NextStep();
        }


    }

    private void NextStep()
    {
        switch (state)
        {
            case STATE.BEGIN:
                state = STATE.GROWING;
                timer = timerValue * 2;
                break;
            case STATE.GROWING:
                state = STATE.COMPLETE;
                break;
        }
    }

    private IEnumerator StartGrown()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("growing");
        grown = true;
        state = STATE.BEGIN;
        timer = timerValue;
    }

    private void ActionComplete()
    {
        grown = false;
    }

    private void InitTree()
    {
        SetPrefabActive(stages.childCount - 1);
    }

    private void HideAllPrefab()
    {
        foreach (Transform tranns in stages)
        {
            tranns.gameObject.SetActive(false);
        }
    }

    private void SetPrefabActive(int index)
    {
        HideAllPrefab();

        int count = stages.childCount;
        if (count <= 0) return;

        stages.GetChild(index).gameObject.SetActive(true);
    }
}
