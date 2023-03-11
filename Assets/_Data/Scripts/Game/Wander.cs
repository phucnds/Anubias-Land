using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : MonoBehaviour
{
    enum STATE {
        IDLE,
        WANDER
    }

    [Header("Wander")]
    public float wander_min = 4f;
    public float wander_range = 15f;    //How far from the starting pos can it wander
    public float wander_interval = 10f; //Interval between changing wander position

    private Character character;

    private Vector3 start_pos;
    private Vector3 wander_target;
    private float state_timer = 0f;
    private STATE state;

    void Awake()
    {
        character = GetComponent<Character>();
        start_pos = Vector3.zero;
        state_timer = 99f;
        
    }



    private void Update()
    {
        //Animations
        bool paused = GameMgr.Instance.IsPaused();


        if (paused)
            return;

        if(!character.IsReallyMoving()) return;

        float mult = GameMgr.Instance.GetSpeedMultiplier();
        state_timer += mult * Time.deltaTime;

        if (state_timer > wander_interval)
        {
            state_timer = Random.Range(-1f, 1f);
            FindWanderTarget();
            character.MoveTo(wander_target);
        }


    }

    private void FindWanderTarget()
    {
        float range = Random.Range(wander_min, wander_range);
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spos = start_pos;
        Vector3 pos = spos + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * range;
        wander_target = pos;
    }


}
