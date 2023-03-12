using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : Building
{
    public static Shop GetNearest(Vector3 pos, float range = 999f, Shop ignore = null)
    {
        float min_dist = range;
        Shop nearest = null;
        foreach (Shop shop in GameMgr.Instance.BuildingManager.GetListShops())
        {
            float dist = (pos - shop.transform.position).magnitude;
            if (dist < min_dist && !shop.Interactable.IsInteractFull() && shop != ignore)
            {
                min_dist = dist;
                nearest = shop;
            }
        }
        return nearest;
    }

    public static Shop GetRandom(Vector3 pos, float range = 999f, Shop ignore = null)
    {
        Shop[] lst = GameMgr.Instance.BuildingManager.GetListShops().ToArray();
        int next = Random.Range(0, lst.Length - 1);

        Shop shopNext = lst[next];

        while (shopNext == ignore)
        {
            int nexts = Random.Range(0, lst.Length - 1);
            shopNext = lst[nexts];
        }

        return shopNext;
    }
}
