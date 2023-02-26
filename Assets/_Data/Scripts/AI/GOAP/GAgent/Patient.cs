using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patient : GAgent
{
    public override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isWaiting", 1, true);
        goals.Add(s1, 3);

        SubGoal s2 = new SubGoal("isTreated", 1, true);
        goals.Add(s2, 5);

        SubGoal s3 = new SubGoal("isHome", 1, true);
        goals.Add(s3, 1);

        SubGoal s4 = new SubGoal("relief", 1, false);
        goals.Add(s4, 2);

        Invoke("NeedRelief", Random.Range(2, 5));
    }

    void NeedRelief()
    {
        beliefs.ModifyState("busting", 0);
        Invoke("NeedRelief", Random.Range(2, 5));
    }
}
