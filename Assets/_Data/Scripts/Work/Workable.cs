using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml;
using UnityEngine;

public class Workable : MonoBehaviour
{
    public int workers_max = 5;        
    private Interactable interact;

    public int workers = 0;

    private void Awake()
    {
        interact = GetComponent<Interactable>();
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

    public Interactable Interactable { get { return interact; } }
}
