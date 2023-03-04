using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class CharacterOrder
{
    public ActionBasic action;
    public Interactable target;
    public Vector3 pos;
    public bool automatic = false;

    public CharacterOrder() { }
    public CharacterOrder(Vector3 p) { pos = p; }
    public CharacterOrder(ActionBasic a, Interactable t, Vector3 p, bool auto)
    {
        action = a; target = t; automatic = auto; pos = p;
    }
}

public class Character : MonoBehaviour
{
    [Header("Move")]
    public float move_speed = 10f;      //How fast the colonist moves?
    public float rotate_speed = 250f;   //How fast it can rotate

    private bool is_moving = false;
    private Vector3 moving;
    private Vector3 facing;
    private Vector3 move_target;
    private Interactable move_action_target;
    private int action_target_pos;
    private bool move_action_auto = false;

    private ActionBasic current_action = null;
    private ActionBasic next_action = null;
    private Interactable action_target = null;
    private Vector3 last_target_pos;
    private float action_progress = 0f;
    private bool action_auto = false;

    private bool is_grounded = false;
    private bool is_fronted = false;
    private bool is_waiting = false;
    private bool is_dead = false;
    private float update_timer = 0f;

    private LinkedList<CharacterOrder> action_queue = new LinkedList<CharacterOrder>();

    IAstarAI ai;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        move_target = transform.position;
    }

    void OnEnable()
    {
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += UpdateMove;
    }

    void OnDisable()
    {
        if (ai != null) ai.onSearchPath -= UpdateMove;
    }

    private void Update()
    {
        UpdateAction();
        UpdateMove();
        UpdateCheckComplete();

        if (IsIdle() && action_queue.Count > 0)
            ExecuteNextOrder();

        //Slow update
        update_timer += Time.deltaTime;
        if (update_timer > 0.5f)
        {
            update_timer = 0f;
            SlowUpdate();
        }

        UpdateAnimator();
    }

    private void UpdateAction()
    {
        if (current_action == null)
            return;

        //Stop action if condition become false
        if (!current_action.CanDoAction(this, action_target))
        {
            Stop();
            return;
        }

        //Update action
        current_action.UpdateAction(this, action_target);
    }

    private void UpdateMove()
    {
        if (move_action_target != null && ai != null) ai.destination = move_target;
    }

    private void UpdateCheckComplete()
    {
        if (!is_moving || is_waiting)
            return;

        //Reached action Target
        if (HasSelectTarget() && HasReachedTarget())
            InteractTarget(move_action_target, move_action_auto);

        //Reached move Target
        else if (!HasSelectTarget() && HasReachedTarget())
            StopMove();

        //Reached attack target
        // else if (IsAttackTargetInRange())
        //     InteractTarget(move_action_target, move_action_auto);

        //Obstacles
        // else if (is_fronted)
        //     Stop();

        // //Pathfind Failed, Stop
        // if (is_moving && pathfind != null && pathfind.force_navmesh && pathfind.HasFailed())
        //     Stop();
    }

    private void SlowUpdate()
    {
        //Update move target
        if (move_action_target != null)
            move_target = move_action_target.transform.position;

        //Update target pos if its moving
        if (action_target != null)
            last_target_pos = action_target.transform.position;

        //Update Pathfind
        ai.destination = move_target;

    }

    private void ExecuteNextOrder()
    {
        if (action_queue.Count > 0)
        {
            CharacterOrder order = action_queue.First.Value;
            action_queue.RemoveFirst();

            if (order.action != null && order.action.CanDoAction(this, order.target))
            {
                ExecuteOrder(order.action, order.target, order.automatic);
            }
            else if (order.target != null)
            {
                MoveToTarget(order.target, order.automatic);
            }
            else
            {
                MoveTo(order.pos);
            }
        }
    }

    public void OrderInterupt(ActionBasic action, Interactable target, bool auto = false)
    {
        if (current_action != null)
        {
            CharacterOrder current = new CharacterOrder(current_action, action_target, move_target, action_auto);
            action_queue.AddFirst(current);
        }

        Vector3 pos = target != null ? target.transform.position : transform.position;
        CharacterOrder order = new CharacterOrder(action, target, pos, auto);
        action_queue.AddFirst(order);

        StopAction();
        ExecuteNextOrder();
    }

    private void ExecuteOrder(ActionBasic action, Interactable target, bool auto = false)
    {
        if (action != null && action.CanDoAction(this, target))
        {
            next_action = action;

            if (target != null )//&& target != Selectable)
                MoveToTarget(target, auto); //Action has target, first move closer
            else
                InteractTarget(target, auto); //Action has no target (ex: eat)
        }
        else if (target != null)
        {
            MoveToTarget(target, auto);
        }
    }

    public void MoveToTarget(Interactable target, bool auto = false)
    {
        is_moving = true;
        move_action_target = target;
        action_target = null;
        current_action = null;
        move_action_auto = auto;
        action_target_pos = 1;
        move_target = target.transform.position;
        ai.destination = move_target;
    }

    public void MoveTo(Vector3 pos)
    {
        is_moving = true;
        move_target = pos;
        move_action_target = null;
        action_target = null;
        current_action = null;
        move_action_auto = false;
        ai.destination = pos;
    }

    private void UpdateAnimator()
    {
        Vector3 localVelocity = GetLocalVelocity();
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }

    public Vector3 GetLocalVelocity()
    {
        Vector3 velocity = ai.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        return localVelocity;
    }

    private void StopMove()
    {
        is_moving = false;
        move_action_target = null;
        move_action_auto = false;
        move_target = transform.position;
        moving = Vector3.zero;
    }

    private bool HasReachedTarget()
    {
        return ai.reachedDestination;
    }

    private bool HasSelectTarget()
    {
        return move_action_target != null;
    }

    private void Stop()
    {
        StopAction();
        StopMove();
    }

    public bool IsIdle()
    {
        return current_action == null && !is_moving;
    }

    public void InteractTarget(Interactable target, bool auto = false)
    {
        StopMove();

        if (target != null)
            target.Interact(this);

        if (next_action == null)
            next_action = GetPriorityAction(target);

        if (next_action != null)
        {
            StartAction(next_action, target, auto);
        }
    }

    public void StartAction(ActionBasic action, Interactable target, bool auto = false)
    {
        if (action != null && action.CanDoAction(this, target))
        {
            current_action = action;
            next_action = null;
            action_auto = auto;
            action_target = target;
            action_progress = 0f;
            current_action.StartAction(this, target);
            //onStartAction?.Invoke();
        }
    }

    public ActionBasic GetPriorityAction(Interactable tselect)
    {
        foreach (ActionBasic action in tselect.actions)
        {
            if (action != null && action.CanDoAction(this, tselect))
            {
                return action;
            }
        }
        return null;
    }

    public void StopAction()
    {
        if (next_action != null)
            StopMove();
        if (current_action != null)
            current_action.StopAction(this, action_target);
        current_action = null;
        action_target = null;
        next_action = null;
        action_auto = false;
        
    }
}
