using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log("CivilianManager");
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
    }

    private void OnStopWork(Civilian civilian)
    {
        UnassignCivilian(civilian);
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
