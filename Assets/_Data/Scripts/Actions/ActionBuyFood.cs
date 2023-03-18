using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionBuyFood", menuName = "Anubias-Land/Actions/ActionBuyFood", order = 0)]
public class ActionBuyFood : ActionBasic
{
    public override void StartAction(Character character, Interactable target)
    {
        
        
        character.FaceToward(target.transform);
        character.WaitFor(2f, () =>
        {      
            
            character.Stop();
        });
        
    }


    public override bool CanDoAction(Character character, Interactable target)
    {
        return target != null;
    }    

    
}
