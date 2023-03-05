using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionVisitTown", menuName = "Anubias-Land/Actions/ActionVisitTown", order = 0)]
public class ActionVisitTown : ActionBasic
{

    public override void StartAction(Character character, Interactable target)
    {
        GameMgr.Instance.BuildingManager.GetTownHall().CharacterVisitTown(character);
    }

    public override void UpdateAction(Character character, Interactable target)
    {

        if (target == null)
        {
            character.StopAction();
            return;
        }

        if (character.HasReachedTarget())
        {
            character.StopAction();
            Gatherable next = Gatherable.GetNearestUnassigned(character.transform.position, 500);
            if(next != null)
            {
                character.OrderNext(null, next.Interactable);
            } 
        }
            
    }

    public override bool CanDoAction(Character character, Interactable target)
    {
        return target != null;
    }


    

}