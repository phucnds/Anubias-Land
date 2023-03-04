using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionBasic", menuName = "Anubias-Land/Actions", order = 0)]
public class ActionBasic : ScriptableObject {

    public virtual void StartAction(Character character, Interactable target)
    {
        Debug.Log("StartAction");
    }

    public virtual void StopAction(Character character, Interactable target)
    {
        Debug.Log("StopAction");
    }

    public virtual void UpdateAction(Character character, Interactable target)
    {
        Debug.Log("UpdateAction");
    }

    public virtual void TriggerAction(Character character, Interactable target, string trigger)
    {
        Debug.Log("StartAction");
    }

    public virtual bool CanDoAction(Character character, Interactable target)
    {
        return true; 
    }
}