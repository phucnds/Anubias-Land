using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionBuyFood", menuName = "Anubias-Land/Actions/ActionBuyFood", order = 0)]
public class ActionBuyFood : ActionBasic
{
    public override void StartAction(Character character, Interactable target)
    {
        character.canBuyFood = false;
        
        character.FaceToward(target.transform.position);
        character.WaitFor(2f, () =>
        {      
            character.stamina = 100;
            character.canBuyFood = true;
            character.Stop();
        });
        
    }

    

    
}
