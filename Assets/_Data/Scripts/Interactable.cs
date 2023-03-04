using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    public float use_range = 5f;        //Distance from which characters can interact with this object
    public int use_max = 8;           //Maximum number of characters who can interact with this object

    [Header("Actions")]
    public ActionBasic[] actions;       //Default actions when interacting with this Interactable (right clicking on it)

    [Header("Interaction Point")]
    public Transform[] interact_points;

    public UnityAction<Character> onTarget;
    public UnityAction<Character> onInteract;

    public void Interact(Character character)
    {
        onInteract?.Invoke(character);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
