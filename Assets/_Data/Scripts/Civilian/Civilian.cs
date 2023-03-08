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
    private WorkBasic current_work = null;
    private Interactable work_target = null;
    private float update_timer;

    public UnityAction<Civilian, WorkBasic> onStartWork;
    public UnityAction<Civilian> onStopWork;

    private void Awake()
    {
        character = GetComponent<Character>();
        //ColonistManager.Get()?.RegisterColonist(this);
    }

    // Start is called before the first frame update
    private void Start()
    {
        OnAnyCivilianrSpawned?.Invoke(this);
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
    }

    public void OrderInterupt(ActionBasic action, Interactable selectable)
    {
        StopWork();
        character.OrderInterupt(action, selectable);
    }

    public void OrderNext(ActionBasic action, Interactable selectable)
    {
        StopWork();
        character.OrderNext(action, selectable);
    }

    public void AutoOrder(ActionBasic action, Interactable selectable)
    {
        character.Order(action, selectable, true);
    }

    public void AutoOrderInterupt(ActionBasic action, Interactable selectable)
    {
        character.OrderInterupt(action, selectable, true);
    }

    public void AutoOrderNext(ActionBasic action, Interactable selectable)
    {
        character.OrderNext(action, selectable, true);
    }

    public void StopAutoAction()
    {
        character.CancelAutoOrders();
    }


    private void OnDestroy()
    {
        OnAnyCivilianDeath?.Invoke(this);
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

    public Character Character { get { return character; } }
}
