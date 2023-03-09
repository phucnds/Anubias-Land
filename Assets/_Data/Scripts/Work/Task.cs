using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class Task : MonoBehaviour
{
    public int workers_max = 5;        
    private Interactable interact;

    public int workers = 0;

    public static event UnityAction<Task> OnAnyTaskCreate;
    public static event UnityAction<Task> OnAnyTaskDestroy;

    public event UnityAction OnChangedAssignedWorkers;

    private void Awake()
    {
        interact = GetComponent<Interactable>();
    }

    private void Start()
    {
        OnAnyTaskCreate?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnAnyTaskDestroy?.Invoke(this);
    }

    public void SetWorkerAmount(int value)
    {
        workers = Mathf.Clamp(value, 0, workers_max);
    }

    public int GetWorkerAmount()
    {
        return workers;
    }

    public List<Civilian> GetAssignedWorkers()
    {
        return GameMgr.Instance.CivilianManager.GetWorkingOn(interact);
    }

    public int CountAssignedWorkers()
    {
        return GameMgr.Instance.CivilianManager.CountWorkingOn(interact);
    }

    public bool IsFullyAssigned()
    {
        return IsValid() && CountAssignedWorkers() >= GetWorkerAmount();
    }

    public bool IsOverAssigned()
    {
        return IsValid() && CountAssignedWorkers() > GetWorkerAmount();
    }

    public bool IsValid()
    {
        //return contruct == null || contruct.IsCompleted();
        return true;
    }

    public void OnChangedWorkers() => OnChangedAssignedWorkers();
    
    public Interactable Interactable { get { return interact; } }
}
