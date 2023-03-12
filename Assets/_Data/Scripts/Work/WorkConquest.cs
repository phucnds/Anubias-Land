using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkConquest", menuName = "Anubias-Land/Works/WorkConquest", order = 10)]
public class WorkConquest : WorkBasic
{
    public ActionConquest conquest;

    public override void StartWork(Civilian civilian)
    {
        civilian.AutoOrder(conquest, civilian.GetWorkTarget());
    }

    public override Interactable FindBestTarget(Vector3 pos)
    {
        Fortress fortress = Fortress.GetNearest(Vector3.zero);
        if (fortress != null)
            return fortress.Interactable;
        return null;
    }
}
