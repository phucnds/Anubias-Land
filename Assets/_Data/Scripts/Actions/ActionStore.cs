using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionStore", menuName = "Anubias-Land/Actions/ActionStore", order = 0)]
public class ActionStore : ActionBasic
{
    public override void StartAction(Character character, Interactable target)
    {
        Storage storage = target.GetComponent<Storage>();
        if (storage != null)
        {
            character.FaceToward(target.transform.position);
            character.WaitFor(0.5f, () =>
            {
                character.inventoryItem -= 10;
                storage.inventoryItem += 10;
                character.StopAnimate();
                character.Stop();
            });
        }
    }
}
