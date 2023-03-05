using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableManager : MonoBehaviour
{
    [SerializeField] private List<Gatherable> listGatherable = new List<Gatherable>();

    private void Awake() {
        Gatherable.OnAnyGatherableCreated += Gatherable_OnAnyGatherableCreated;
        Gatherable.OnAnyGatherableDestroyed += Gatherable_OnAnyGatherableDestroyed;
    }

    private void Gatherable_OnAnyGatherableCreated(Gatherable gatherable)
    {
        listGatherable.Add(gatherable);
    }

    private void Gatherable_OnAnyGatherableDestroyed(Gatherable gatherable)
    {
        listGatherable.Remove(gatherable);
    }

    public List<Gatherable> GetListGatherable()
    {
        return listGatherable;
    }
}
