using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilianAttribute : MonoBehaviour
{
    [Header("Attributes")]
    public AttributeData[] attributes;  //List of available attributes

    // [Header("Levels")]
    // public LevelData[] levels;  //List of available levels

    [Header("Auto Eat")]
    public bool auto_eat = true;        //Will the colonist try to eat automatically when hungry?
 

    private Character character;
  

    private float move_speed_mult = 1f;
    private float gather_mult = 1f;
    private bool depleting = false;
}
