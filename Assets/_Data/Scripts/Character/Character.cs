using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

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
    private float move_speed = 2;      //How fast the colonist moves?
    private float rotate_speed = 100;   //How fast it can rotate

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

    public static event UnityAction<Character> OnAnyCharacterSpawned;
    public static event UnityAction<Character> OnAnyCharacterDeath;

    private CharacterAnim anim;
    private Civilian civilian;
    private Interactable interact;
    private CharacterAttack attack;

    #region Debug
    [Header("Debug")]
    public bool is_Idle;
    public bool hasReachedTarget;

    private bool debug = false;
    public int inventoryItem = 0;
    private GameObject model;

    public void ToggleModel(bool flag)
    {
        model.SetActive(flag);
    }
    #endregion

    IAstarAI ai;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        move_target = transform.position;
        anim = GetComponent<CharacterAnim>();
        civilian = GetComponent<Civilian>();
        interact = GetComponent<Interactable>();
        attack = GetComponent<CharacterAttack>();
    }

    private void Start()
    {
        OnAnyCharacterSpawned?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnAnyCharacterDeath?.Invoke(this);
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

        is_Idle = IsIdle();
        hasReachedTarget = HasReachedTarget();

        UpdateAction();
        UpdateMove();
        UpdateCheckComplete();

        if (IsIdle() && action_queue.Count > 0)
            ExecuteNextOrder();

        //Slow update
        update_timer += Time.deltaTime;
        if (update_timer > 0.25f)
        {
            update_timer = 0f;
            SlowUpdate();
        }
        
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
        float mult = GameMgr.Instance.GetSpeedMultiplier();
        ai.maxSpeed = move_speed * mult;
        //GetComponent<AILerp>().rotationSpeed = rotate_speed * mult;
        if (move_action_target != null && ai != null) ai.destination = move_target;
        moving = ai.velocity;
    }

    private void UpdateCheckComplete()
    {
        if (!is_moving || is_waiting)
            return;

        //Reached action Target
        if (HasSelectTarget() && HasReachedTarget())
        {
            //Debug.Log("Reached action Target");
            InteractTarget(move_action_target, move_action_auto);
        }


        //Reached move Target
        else if (!HasSelectTarget() && HasReachedTarget())
        {
            //Debug.Log("Reached move Target");
            StopMove();
        }


        //Reached attack target
        else if (IsAttackTargetInRange())
            InteractTarget(move_action_target, move_action_auto);

        //Obstacles
        // else if (is_fronted)
        //     Stop();

        // //Pathfind Failed, Stop
        // if (is_moving && ai != null  && !ai.hasPath)
        //     Stop();
    }

    public bool IsAttackTargetInRange()
    {
        if (attack != null && IsAttackMode())
        {
            return attack.IsAttackTargetInRange(move_action_target);
        }
        return false;
    }

    public bool IsAttackMode()
    {
        bool attack_next = next_action != null && next_action is ActionAttack;
        bool attack_now = current_action != null && current_action is ActionAttack;
        return attack_next || attack_now;
    }

    private void SlowUpdate()
    {
        //Update move target
        if (move_action_target != null)
            move_target = move_action_target.GetInteractPosition(action_target_pos);

        //Update target pos if its moving
        if (action_target != null)
            last_target_pos = action_target.GetInteractPosition(action_target_pos);

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

    private void ExecuteOrder(ActionBasic action, Interactable target, bool auto = false)
    {
        if (action != null && action.CanDoAction(this, target))
        {
            next_action = action;

            if (target != null)//&& target != Selectable)
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
        if (target.IsInteractFull())
            return;

        is_moving = true;
        move_action_target = target;
        action_target = null;
        current_action = null;
        move_action_auto = auto;
        action_target_pos = target.GetInteractPositionIndex(this);
        Debug.Log(this.name + action_target_pos);
        move_target = target.GetInteractPosition(action_target_pos);
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

    public void Move(Vector3 pos)
    {
        is_moving = true;
        move_target = pos;
        move_action_target = null;
        action_target = null;
        //current_action = null;
        move_action_auto = true;
        ai.destination = pos;
    }

    public Vector3 GetLocalVelocity()
    {
        Vector3 velocity = ai.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        return localVelocity;
    }

    public void StopMove()
    {
        is_moving = false;
        move_action_target = null;
        move_action_auto = false;
        move_target = transform.position;
        moving = Vector3.zero;
    }

    public bool HasReachedTarget()
    {
        //return ai.remainingDistance <= 0.1f;
        return ai.reachedDestination;
    }

    public ActionBasic GetCurrentAction()
    {
        return current_action;
    }

    public Vector3 GetMoveTargetPos()
    {
        if (ai != null && ai.hasPath)
            return ai.destination;
        return move_target;
    }

    private bool HasSelectTarget()
    {
        return move_action_target != null;
    }

    public void Stop()
    {
        StopAction();
        StopMove();
    }

    public bool IsIdle()
    {
        return current_action == null && !is_moving;
    }

    public bool IsWaiting()
    {
        return is_waiting;
    }

    public void Wait()
    {
        is_waiting = true;
    }

    public void InteractTarget(Interactable target, bool auto = false)
    {
        StopMove();

        if (target != null)
            target.Interact(this);

        if (next_action == null)
        {
            next_action = GetPriorityAction(target);
            //Debug.Log("GetPriorityAction");
        }


        if (next_action != null)
        {
            StartAction(next_action, target, auto);
            //Debug.Log("StartAction: " + next_action);
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
            if (debug) current_action.GettActionID();
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

    public static List<Character> GetAllTargeting(Interactable select)
    {
        List<Character> targeting_list = new List<Character>();
        foreach (Character character in GameMgr.Instance.CharacterManager.GetListCharacter())
        {
            if (character.GetTarget() == select)
                targeting_list.Add(character);
        }
        return targeting_list;
    }

    private Interactable GetTarget()
    {
        if (current_action != null && action_target != null)
            return action_target;
        return move_action_target;
    }

    public int GetTargetPosIndex()
    {
        return action_target_pos;
    }

    public static int CountTargetingTarget(Interactable target)
    {
        int count = 0;
        foreach (Character character in GameMgr.Instance.CharacterManager.GetListCharacter())
        {
            if (character.GetTarget() == target)
                count++;
        }
        return count;
    }

    public void Order(ActionBasic action, Interactable target, bool auto = false)
    {
        CancelOrders();
        ExecuteOrder(action, target, auto);
    }

    //Do action now, but resume previous order right after
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

    //Do action after current one
    public void OrderNext(ActionBasic action, Interactable target, bool auto = false)
    {
        Vector3 pos = target != null ? target.transform.position : transform.position;
        CharacterOrder order = new CharacterOrder(action, target, pos, auto);
        action_queue.AddLast(order);
        if (IsIdle())
        {
            ExecuteNextOrder();
        }
    }

    public void OrderMoveTo(Vector3 pos)
    {
        CancelOrders();
        MoveTo(pos);
    }

    public void OrderMoveToNext(Vector3 pos)
    {
        CharacterOrder order = new CharacterOrder();
        order.pos = pos;
        action_queue.AddLast(order);
    }

    public void CancelOrders()
    {
        action_queue.Clear();
        StopAction();
    }

    public void CancelAutoOrders()
    {
        if (action_queue.Count > 0)
        {
            for (LinkedListNode<CharacterOrder> node = action_queue.First; node != null; node = node.Next)
            {
                if (node.Value.automatic)
                    action_queue.Remove(node);
            }
        }
        if (action_auto)
            StopAction();
    }

    public int CountQueuedOrders()
    {
        return action_queue.Count;
    }

    public void Animate(string anim_id, bool active)
    {
        anim?.Animate(anim_id, active);
    }

    public void FaceToward(Transform trans)
    {
        // facing = pos - transform.position;
        // facing.y = 0f;
        // facing.Normalize();
        transform.LookAt(trans);
    }

    public bool IsReallyMoving()
    {
        return is_moving && !is_waiting && GetLocalVelocity().z > 0.1f;
    }

    public void StopAnimate()
    {
        anim?.StopAnimate();
    }


    public float GetActionProgress()
    {
        return action_progress;
    }

    public void SetActionProgress(float value)
    {
        action_progress = value;
    }

    public void AddActionProgress(float value)
    {
        action_progress += value;
    }

    public void WaitFor(float duration, UnityAction callback = null)
    {
        if (!is_waiting)
        {
            StopMove();
            is_waiting = true;
            StartCoroutine(RunWaitRoutine(duration, callback));
        }
    }

    private IEnumerator RunWaitRoutine(float action_duration, UnityAction callback = null)
    {
        is_waiting = true;

        float mult = GameMgr.Instance.GetSpeedMultiplier();
        float duration = mult > 0.001f ? action_duration / mult : 0f;
        yield return new WaitForSeconds(duration);

        is_waiting = false;
        if (callback != null)
            callback.Invoke();
    }

    public void StopWait()
    {
        is_waiting = false;
    }

    public void AttackTarget(Interactable target)
    {
        ActionAttack attack = ActionBasic.Get<ActionAttack>();
        if (attack.CanDoAction(this, target))
            Order(attack, target);
    }

    public Civilian Civilian { get { return civilian; } }
    public Interactable Interactable { get { return interact; } }
    public CharacterAttack Attack { get { return attack; } }

}
