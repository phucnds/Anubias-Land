using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTracker : MonoBehaviour
{
    enum STATE
    {
        Idle = 0,
        Regen = 1,
        Work = 2,
        Conquest = 3
    }

    [SerializeField] private STATE state = STATE.Idle;

    public float Idle = 100;
    public float Work = 0;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
