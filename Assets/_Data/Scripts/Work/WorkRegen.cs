using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkRegen", menuName = "Anubias-Land/Works/WorkRegen", order = 10)]
public class WorkRegen : WorkBasic
{
    public ActionWalkingAround walking;


    public override void StartWork(Civilian civilian)
    {
        civilian.AutoOrder(walking, null);
    }

}
