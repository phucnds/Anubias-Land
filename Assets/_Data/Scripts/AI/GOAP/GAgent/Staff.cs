using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : GAgent
{
    public override void Start()
    {
        base.Start();
        SubGoal s1 = new SubGoal("order", 1, true);
        goals.Add(s1, 3);
    }
}
