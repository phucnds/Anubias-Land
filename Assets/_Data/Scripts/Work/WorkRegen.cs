using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkRegen", menuName = "Anubias-Land/Works/WorkRegen", order = 10)]
public class WorkRegen : WorkBasic
{
    public ActionBuyFood action_buyfood;


    public override void StartWork(Civilian civilian)
    {
        Interactable target = civilian.GetWorkTarget();
        Market market = target.GetComponent<Market>();

        if (market != null)
        {
            Debug.Log("AutoOrder");
            Debug.Log(market.Interactable);
            civilian.AutoOrder(action_buyfood, market.Interactable);
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
        Market market = Market.GetNearestUnassigned(pos, range);
        if (market != null)
            return market.Interactable;
        return null;
    }
}
