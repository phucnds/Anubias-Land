using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionRest", menuName = "Anubias-Land/Actions/ActionRest", order = 0)]
public class ActionRest : ActionBasic
{
    public override void StartAction(Character character, Interactable target)
    {
        character.canRest = false;
        character.StopAnimate();
        character.ToggleModel(false);
        
    }

    public override void StopAction(Character character, Interactable target)
    {
        character.StopAnimate();
        character.ToggleModel(true);
    }

    public override void UpdateAction(Character character, Interactable target)
    {
        
        float speed = 0.5f;
        character.AddActionProgress(speed * Time.deltaTime);
        if (character.GetActionProgress() > 1f)
        {
            character.SetActionProgress(0f);
            character.stamina++;
        }

        if(character.stamina >= 100)
        {
            character.StopAction();
            character.canRest = true;
        }
    }

    



}
