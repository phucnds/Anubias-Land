using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHome : GAction
{
    public override bool PrePerform()
    {
        beliefs.RemoveState("atHospital");
        return true;
    }

    public override bool PostPerform()
    {
        //Destroy(gameObject, 1);
        return true;
    }
}
