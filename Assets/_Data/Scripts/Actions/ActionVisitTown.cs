using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionVisitTown", menuName = "Anubias-Land/Actions", order = 0)]
public class ActionVisitTown : ActionBasic
{

    public override void UpdateAction(Character character, Interactable target)
    {

        if (target == null)
        {
            character.StopAction();
            return;
        }

        if (character.HasReachedTarget())
        {
            GameMgr.Instance.BuildingManager.GetTownHall().CharacterVisitTown(character);
            character.StopAction();
        }
            
    }

    public override bool CanDoAction(Character character, Interactable target)
    {
        return target != null;
    }

}