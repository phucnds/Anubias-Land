using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicAction", menuName = "Anubias-Land/Actions", order = 0)]
public class BasicAction : ScriptableObject {

    public virtual void StopAction(Character character, Interactable target)
    {
        //Inherit this function to run code when the character stop the action
    }

    public virtual void UpdateAction(Character character, Interactable target)
    {
        //Inherit this function to run code every frame while doing this action
    }

    public virtual void TriggerAction(Character character, Interactable target, string trigger)
    {
        //Custom action trigger
    }

    public virtual bool CanDoAction(Character character, Interactable target)
    {
        return true; //Inherit this function put a condition on if the action can be performed or not
    }
}