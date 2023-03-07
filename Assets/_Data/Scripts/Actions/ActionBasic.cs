using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionBasic : ScriptableObject {

    public string id;  //Id for the save file

    private static List<ActionBasic> list = new List<ActionBasic>();

    public virtual void GettActionID()
    {
        Debug.Log("Action: " + id);
    }

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

    public virtual bool OnReadyAction(Character character, Interactable target)
    {
        return true;
    }

    public static void Load(string folder = "")
    {
        list.Clear();
        list.AddRange(Resources.LoadAll<ActionBasic>(folder));
    }

    public static ActionBasic Get(string id)
    {
        foreach (ActionBasic action in list)
        {
            if (action.id == id)
                return action;
        }
        return null;
    }

    public static T Get<T>() where T : ActionBasic
    {
        foreach (ActionBasic action in list)
        {
            if (action is T)
                return (T)action;
        }
        return null;
    }

    public static List<ActionBasic> GetAll()
    {
        return list;
    }
}