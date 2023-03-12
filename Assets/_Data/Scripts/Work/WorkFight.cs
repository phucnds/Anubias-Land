using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkFight", menuName = "Anubias-Land/Works/WorkFight", order = 10)]
public class WorkFight : WorkBasic
{
    public ActionAttack action_attack;

    public override void StartWork(Civilian civilian)
    {
        Interactable target = civilian.GetWorkTarget();
        civilian.Character.AttackTarget(target);
    }

    public override void StopWork(Civilian civilian)
    {

    }

    public override bool CanDoWork(Civilian civilian, Interactable target)
    {
        if (target != null)
        {
            Destructible destruct = target.Destructible;
            bool azone = destruct != null && destruct.CanBeAttacked();
            return azone;
        }
        return false;
    }

    public override Interactable FindBestTarget(Vector3 pos)
    {
        Destructible destruct = Destructible.GetNearest(AttackTeam.Enemy, pos);
        if (destruct != null)
            return destruct.Interactable;
        return null;
    }
}
