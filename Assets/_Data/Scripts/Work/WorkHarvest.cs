using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WorkHarvest", menuName = "Anubias-Land/Works/WorkHarvest", order = 10)]
public class WorkHarvest : WorkBasic
{
    public ActionGather action_gather;
    

    public override void StartWork(Civilian civilian)
    {
        Interactable target = civilian.GetWorkTarget();
        Gatherable gather = target.GetComponent<Gatherable>();

        if (gather != null)
        {
            Debug.Log("AutoOrder");
            Debug.Log(gather.Interactable);
            civilian.AutoOrder(action_gather, gather.Interactable);
        }
            
    }

    public override void StopWork(Civilian civilian)
    {
        
    }

    public override bool CanDoWork(Civilian civilian, Interactable target)
    {
        //if (target != null)
        //{
        //    Gatherable gather = target.GetComponent<Gatherable>();
        //    bool agather = gather != null && gather.CanHarvest(civilian.Character);
        //    return agather;
        //}
        return true;
    }

    public override Interactable FindBestTarget(Vector3 pos)
    {
        Gatherable gather = Gatherable.GetNearestUnassigned(pos, range);
        if (gather != null)
            return gather.Interactable;
        return null;
    }
}
