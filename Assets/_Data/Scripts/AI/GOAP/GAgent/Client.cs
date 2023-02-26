using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : GAgent
{
    public override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("isIdle", 1, true);
        goals.Add(s1, 3);

       

    }
}
