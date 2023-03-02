using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class Character : MonoBehaviour
{
    [SerializeField] private BasicAction currentAction = null;

    public bool isIdle = false;
    public bool is_moving = false;
    public bool is_waiting = false;

    public Interactable actionTarget;
    public Mover mover;
    public Interactable move_action_target;
    public bool move_action_auto = false;

    private void Awake()
    {
        mover = GetComponent<Mover>();
    }

    private void Start()
    {
    }

    

    void Update()
    {
        UpdateAction();
        UpdateCheckComplete();
        
    }

    private void UpdateCheckComplete()
    {
        if (!is_moving || is_waiting)
            return;

        if (HasSelectTarget() && HasReachedTarget())
            InteractTarget(move_action_target, move_action_auto);
    }

    public void InteractTarget(Interactable target, bool auto = false)
    {
        StopMove();

        // if (target != null)
        //     target.Interact(this);

        // if (next_action == null)
        //     next_action = GetPriorityAction(target);

        // if (next_action != null)
        // {
        //     StartAction(next_action, target, auto);
        // }
    }

    private bool HasReachedTarget()
    {
        return mover.HasReachedTarget();
    }

    private bool HasSelectTarget()
    {
        return move_action_target != null;
    }

    private void UpdateAction()
    {
        if (currentAction == null)
            return;

        if (!currentAction.CanDoAction(this, actionTarget))
        {
            Stop();
            return;
        }

        currentAction.UpdateAction(this, actionTarget);
    }

    

    private void Stop()
    {
        StopAction();
        StopMove();
    }

    private void StopAction()
    {

    }
    private void StopMove()
    {
        mover.Stop();
    }
}
