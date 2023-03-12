using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Civilian : MonoBehaviour
{

    public static event UnityAction<Civilian> OnAnyCivilianrSpawned;
    public static event UnityAction<Civilian> OnAnyCivilianDeath;

    private Character character;
    public WorkBasic current_work = null;
    private Interactable work_target = null;

    public bool manual_order = false;
    private float update_timer;

    public UnityAction<Civilian, WorkBasic> onStartWork;
    public UnityAction<Civilian> onStopWork;

    private CivilianAttribute attributes;

    private void Awake()
    {
        character = GetComponent<Character>();
        attributes = GetComponent<CivilianAttribute>();
        GameMgr.Instance.CivilianManager?.RegisterColonist(this);

    }

    // Start is called before the first frame update
    private void Start()
    {
        OnAnyCivilianrSpawned?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnAnyCivilianDeath?.Invoke(this);
        GameMgr.Instance.CivilianManager?.UnregisterCivilian(this);
    }

    private void Update()
    {


        if (GameMgr.Instance.IsPaused())
            return;

        // if (character.IsDead())
        //     return;

        //Current Work
       

        current_work?.UpdateWork(this);


    }

    public bool IsAnyDepleted()
    {
        

        return false;
    }

    public bool CanDoWork(WorkBasic work, Interactable target)
    {
        return work != null && work.CanDoWork(this, target);
    }

    public void StartWork(WorkBasic work, Interactable target)
    {
        if (work != null && CanDoWork(work, target))
        {
            StopWork();
            current_work = work;
            work_target = target;
            work.StartWork(this);
            onStartWork?.Invoke(this, work);
        }
    }

    public void StopWork()
    {
        if (current_work != null)
            current_work.StopWork(this);
        StopAutoAction();
        current_work = null;
        work_target = null;
        onStopWork?.Invoke(this);
    }

    public void Order(ActionBasic action)
    {
        //Not target, target self
        Order(action, null);
    }

    public void Order(ActionBasic action, Interactable selectable)
    {
        StopWork();
        character.Order(action, selectable);
        manual_order = true;
    }

    public void OrderInterupt(ActionBasic action, Interactable selectable)
    {
        StopWork();
        character.OrderInterupt(action, selectable);
        manual_order = true;
    }

    public void OrderNext(ActionBasic action, Interactable selectable)
    {
        StopWork();
        character.OrderNext(action, selectable);
        manual_order = true;
    }

    public void AutoOrder(ActionBasic action, Interactable selectable)
    {
        character.Order(action, selectable);
        manual_order = false;
    }

    public void AutoOrderInterupt(ActionBasic action, Interactable selectable)
    {
        character.OrderInterupt(action, selectable);
        manual_order = false;
    }

    public void AutoOrderNext(ActionBasic action, Interactable selectable)
    {
        character.OrderNext(action, selectable);
        manual_order = false;
    }

    public void StopAutoAction()
    {
        character.CancelAutoOrders();
    }

    public WorkBasic GetWork()
    {
        return current_work;
    }

    public Interactable GetWorkTarget()
    {
        return work_target;
    }

    public bool IsWorking()
    {
        return current_work != null;
    }

    public bool IsIdle()
    {
        return character.IsIdle();
    }

    public bool IsManual()
    {
        return manual_order; //Manual order
    }

    public bool IsAuto()
    {
        return !manual_order; //Automatic order
    }

    public Character Character { get { return character; } }
    public CivilianAttribute Attributes {get {return attributes;}}
}
