using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreated : GAction
{
    public override bool PrePerform()
    {
        Debug.Log("Starting GetTreated");
        target = inventory.FindItemWithTag("Cubicle");
        if (target == null)
            return false;
        return true;
    }

    public override bool PostPerform()
    {
        Debug.Log("Finished GetTreated");
        GWorld.Instance.GetWorld().ModifyState("Treated", 1);
        beliefs.ModifyState("isCured", 1);
        inventory.RemoveItem(target);
        return true;
    }
}
