using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fortress : Building
{
    [SerializeField] private Transform spawnerPoint;
    [SerializeField] private int amount = 10;
    [SerializeField] private GameObject[] enemyGameObjects;

    








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
