using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CivilianManager : MonoBehaviour
{
    [SerializeField] private List<Civilian> listCivilian = new List<Civilian>();

    private void Awake() {
        Civilian.OnAnyCivilianrSpawned += Civilian_OnAnyCivilianrSpawned;
        Civilian.OnAnyCivilianDeath += Civilian_OnAnyCivilianDeath;
    }

    private HashSet<Civilian> registered_civilians = new HashSet<Civilian>();
    private HashSet<Civilian> working_civilians = new HashSet<Civilian>();
    private Dictionary<Interactable, HashSet<Civilian>> assigned_civilians = new Dictionary<Interactable, HashSet<Civilian>>();

    private int idle_index = 0;
    private float update_timer = 0f;

    void Update()
    {
        update_timer += Time.deltaTime;
        if (update_timer > 0.5f)
        {
            update_timer = 0f;
            SlowUpdate();
        }
    }

    private void SlowUpdate()
    {
        //Start new work
        StartNewWork();

        //Stop work
        StopWork();

    }

    private void StartNewWork()
    {
        Vector3 base_pos = GameMgr.Instance.BuildingManager.GetTownHall().transform.position;

        foreach (WorkBasic work in WorkBasic.GetAll())
        {
            //Check if work has target
            Interactable target = work.FindBestTarget(base_pos);
            if (target != null)
            {
                if (target.Task == null || !target.Task.IsFullyAssigned())
                {
                    //Find the best civilian for the work
                    Civilian civilian = FindBestCivilian(work, target);
                    if (civilian != null)
                    {
                        StartWork(civilian, work, target);
                    }
                }
            }
        }
    }

    private void StopWork()
    {
        foreach (Civilian civilian in listCivilian)
        {
            WorkBasic current_work = civilian.GetWork();
            Interactable work_target = civilian.GetWorkTarget();
            if (current_work != null)
            {
                //Stop work

                //Debug.Log(work_target.Task?.IsOverAssigned());
                if (civilian.IsIdle() && !civilian.CanDoWork(current_work, work_target))// || civilian.IsAnyDepleted())
                    civilian.StopWork();
                if (work_target != null && work_target.Task != null && work_target.Task.IsOverAssigned())
                {
                    civilian.CancelOrders();
                    civilian.StopWork();
                }
                    
            }
        }
    }

    public void StartWork(Civilian civilian, WorkBasic work, Interactable target)
    {
        //Start working on a task
        if (civilian != null && work != null && target != null)
        {
            //Debug.Log("can do work");
            civilian.StartWork(work, target);
        }
    }

    public Civilian FindBestCivilian(WorkBasic work, Interactable target)
    {
        Civilian best = null;
        float min_dist = work.range;
        foreach (Civilian civilian in listCivilian)
        {
            if (civilian.IsAuto() || civilian.IsIdle())
            {
                if(civilian.IsWorking())
                {
                    Debug.Log("work.priority: " + work.priority);
                    Debug.Log("civilian.GetPriorityWork: " + civilian.GetPriorityWork());
                }
                
                if (!civilian.IsWorking() || work.priority > civilian.GetPriorityWork())
                {
                    
                    //Debug.Log("isn't working");
                    float dist = (civilian.transform.position - target.Transform.position).magnitude;
                    if (dist < work.range && civilian.CanDoWork(work, target))
                    {
                        bool best_idle = best != null && best.IsIdle();
                        bool is_idle_better = civilian.IsIdle() || !best_idle; //Idle colonists priority over working ones
                        bool is_better = is_idle_better && dist < min_dist;
                        //Debug.Log("in range");
                        if (is_better)
                        {
                            //Debug.Log("has best");
                            min_dist = dist;
                            best = civilian;
                        }
                    }
                }
            }
        }
        return best;
    }

    public void RegisterColonist(Civilian civilian)
    {
        if (!registered_civilians.Contains(civilian))
        {
            registered_civilians.Add(civilian);
            civilian.onStartWork += OnStartWork;
            civilian.onStopWork += OnStopWork;
        }
    }

    public void UnregisterCivilian(Civilian civilian)
    {
        if (registered_civilians.Contains(civilian))
        {
            registered_civilians.Remove(civilian);
            civilian.onStartWork -= OnStartWork;
            civilian.onStopWork -= OnStopWork;
        }
    }

    private void OnStartWork(Civilian civilian, WorkBasic work)
    {
        AssignCivilian(civilian, civilian.GetWorkTarget());
        if (civilian.GetWorkTarget() == null) return;
        civilian.GetWorkTarget()?.Task?.OnChangedWorkers();
    }

    private void OnStopWork(Civilian civilian)
    {
        UnassignCivilian(civilian);
        if (civilian.GetWorkTarget() == null) return;
        civilian.GetWorkTarget()?.Task?.OnChangedWorkers();
    }

    private void AssignCivilian(Civilian civilian, Interactable select)
    {
        //Assign Civilian to selectable
        UnassignCivilian(civilian);
        working_civilians.Add(civilian);
        if (select != null)
        {
            if (!assigned_civilians.ContainsKey(select))
                assigned_civilians[select] = new HashSet<Civilian>();
            assigned_civilians[select].Add(civilian);
            
        }
        
    }

    private void UnassignCivilian(Civilian civilian)
    {
        //Unlink Civilian from all selectables
        if (working_civilians.Contains(civilian))
        {
            working_civilians.Remove(civilian);
            foreach (KeyValuePair<Interactable, HashSet<Civilian>> pair in assigned_civilians)
            {
                if (pair.Value.Contains(civilian))
                    pair.Value.Remove(civilian);
            }
        }

    }


    public List<Civilian> GetWorkingOn(Interactable target)
    {
        if (assigned_civilians.ContainsKey(target))
        {
            List<Civilian> civilians = new List<Civilian>(assigned_civilians[target]);
            return civilians;
        }
        return new List<Civilian>();
    }

    public int CountWorkingOn(Interactable target)
    {
        if (assigned_civilians.ContainsKey(target))
        {
            HashSet<Civilian> civilians = assigned_civilians[target];
            return civilians.Count;
        }
        return 0;
    }

    private void Civilian_OnAnyCivilianrSpawned(Civilian civilian)
    {
        listCivilian.Add(civilian);
    }

    private void Civilian_OnAnyCivilianDeath(Civilian civilian)
    {
        listCivilian.Remove(civilian);
    }

    public List<Civilian> GetListCivilian()
    {
        return listCivilian;
    }
}
