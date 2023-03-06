using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionGather", menuName = "Anubias-Land/Actions/ActionGather", order = 0)]
public class ActionGather : ActionBasic
{
    public float storage_dist = 200f;   
    public float next_dist = 20f;

    public override void StartAction(Character character, Interactable target)
    {
        Gatherable gather = target.GetComponent<Gatherable>();
        if (gather != null)
        {
            character.Animate(gather.harvest_anim, true);
            character.FaceToward(target.transform.position);
        }
    }

    public override void StopAction(Character character, Interactable target)
    {
        character.StopAnimate();
        //character.HideTools();

        // Gatherable gather = target != null ? target.GetComponent<Gatherable>() : null;
        // if (gather != null && gather.GetValue() <= 0 && character.CountQueuedOrders() == 0)
        //     FindReturnTarget(character, gather);
    }

    public override void UpdateAction(Character character, Interactable target)
    {
        Gatherable gather = target.GetComponent<Gatherable>();
        float speed = 1f;
        character.AddActionProgress(speed * gather.harvest_speed * Time.deltaTime);
        if (character.GetActionProgress() > 1f)
        {
            character.SetActionProgress(0f);
            character.inventoryItem++;
            gather.value--;
        }

        if (gather.value<= 0 || character.inventoryItem == 10)
        {
            bool found = FindReturnTarget(character, gather);
            if (!found)
                character.Stop();
        }
        
    }

    private bool FindReturnTarget(Character character, Gatherable gather)
    {
        character.StopAnimate();
        Storage storage = Storage.GetNearestActive(character.transform.position, storage_dist);
        if (storage != null)
        {
            //Return to storage
            ActionStore store = ActionBasic.Get<ActionStore>();
            character.OrderInterupt(store, storage.Interactable);

            //Find next resource to harvest
            Gatherable next = Gatherable.GetNearest(character.transform.position, next_dist);
            if (next != null && character.CountQueuedOrders() <= 2)
                character.OrderNext(this, next.Interactable, true);

            return true;
        }
        return false;
    }
}