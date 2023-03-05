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

            Inns inns = GameMgr.Instance.BuildingManager.GetListInns()[0];
            Interactable interact = inns.GetComponent<Interactable>();
            character.StopAction();
            //character.OrderNext(null,interact);
        }
            
    }

    public override bool CanDoAction(Character character, Interactable target)
    {
        return target != null;
    }

}