using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inns : Building
{
    [SerializeField] private int appear = 0;
    public static Inns GetNearest(Vector3 pos, float range = 999f, Inns ignore = null)
    {
        float min_dist = range;
        Inns nearest = null;
        foreach (Inns inns in GameMgr.Instance.BuildingManager.GetListInns())
        {
            float dist = (pos - inns.transform.position).magnitude;
            if (dist < min_dist && !inns.Interactable.IsInteractFull() && inns != ignore)
            {
                min_dist = dist;
                nearest = inns;
            }
        }
        return nearest;
    }


    public static Inns GetRandom(Vector3 pos, float range = 999f, Inns ignore = null)
    {
        Inns[] lst = GameMgr.Instance.BuildingManager.GetListInns().ToArray();
        int next = Random.Range(0,lst.Length - 1);

        Inns innsNext = lst[next];

        while (innsNext == ignore)
        {
            int nexts = Random.Range(0, lst.Length - 1);
            innsNext = lst[nexts];
        }
        
        return innsNext;
    }
    

    
}
