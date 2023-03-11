using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : Building
{
    public static Market GetNearestUnassigned(Vector3 pos, float range = 999f)
    {
        float min_dist = range;
        Market nearest = null;
        foreach (Market market in GameMgr.Instance.BuildingManager.GetListMarkets())
        {
            float dist = (pos - market.transform.position).magnitude;
            if (dist < min_dist && !market.Interactable.IsInteractFull())
            {
                min_dist = dist;
                nearest = market;
            }
        }
        return nearest;
    }
}
