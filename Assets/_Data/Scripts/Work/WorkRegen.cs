using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkRegen", menuName = "Anubias-Land/Works/WorkRegen", order = 10)]
public class WorkRegen : WorkBasic
{
    public ActionWalkingAround walking;


    public override void StartWork(Civilian civilian)
    {
            civilian.AutoOrder(walking, civilian.GetWorkTarget());
    }

    public override Interactable FindBestTarget(Vector3 pos)
    {
        TownHall townHall = GameMgr.Instance.BuildingManager.GetTownHall();
        if (townHall != null)
            return townHall.Interactable;
        return null;
    }

}
