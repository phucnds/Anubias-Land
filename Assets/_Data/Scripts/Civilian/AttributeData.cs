using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AttributeType
{
    None = 0,
    Health = 1,
    Stamina = 2,
    Satiety = 3
}

[CreateAssetMenu(fileName = "AttributeData", menuName = "Anubias-Land/AttributeData", order = 11)]
public class AttributeData : ScriptableObject
{
    public AttributeType type;      //Which attribute is this?
    public string title;            //Visual Text of the attribute

    [Space(5)]

    public float start_value = 100f; 
    public float max_value = 100f;

    public float value_per_hour = 0f; 

    [Header("Low")]
    public string low_status;           
    public float low_threshold = 25f;   

    [Header("Depleted")]
    public float deplete_hp_loss = 0f; 
    public float deplete_move_mult = 1f;  
    public float deplete_gather_mult = 1f;


    private static List<AttributeData> list = new List<AttributeData>();

    public static void Load(string folder = "")
    {
        list.Clear();
        list.AddRange(Resources.LoadAll<AttributeData>(folder));
    }

    public static AttributeData Get(AttributeType type)
    {
        foreach (AttributeData data in list)
        {
            if (data.type == type)
                return data;
        }
        return null;
    }

    public static List<AttributeData> GetAll()
    {
        return list;
    }
}
