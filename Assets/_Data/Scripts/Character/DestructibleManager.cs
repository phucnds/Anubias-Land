using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleManager : MonoBehaviour
{
    private List<Destructible> listDestruct = new List<Destructible>();

    private void Awake() {
        Destructible.OnAnyDestructibleCreated += Destructible_OnAnyDestructibleCreated;
        Destructible.OnAnyDestructibleDestroyed += Destructible_OnAnyDestructibleDestroyed;
    }

    private void Destructible_OnAnyDestructibleCreated(Destructible destruct)
    {
        listDestruct.Add(destruct);
    }

    private void Destructible_OnAnyDestructibleDestroyed(Destructible destruct)
    {
        listDestruct.Remove(destruct);
    }

    public List<Destructible> GetListDestruct()
    {
        return listDestruct;
    }
}
