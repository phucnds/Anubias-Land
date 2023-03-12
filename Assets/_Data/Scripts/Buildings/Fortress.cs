using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : Building
{
    public static Fortress GetNearest(Vector3 pos, float range = 999f, Fortress ignore = null)
    {
        float min_dist = range;
        Fortress nearest = null;
        foreach (Fortress fortress in GameMgr.Instance.BuildingManager.GetListFortress())
        {
            float dist = (pos - fortress.transform.position).magnitude;
            if (dist < min_dist && !fortress.Interactable.IsInteractFull() && fortress != ignore)
            {
                min_dist = dist;
                nearest = fortress;
            }
        }
        return nearest;
    }
}
