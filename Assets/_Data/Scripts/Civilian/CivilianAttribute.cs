using System;
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

    public int Idle = 0;
    public int Deployment = 100; 

    private Character character;
    private CharacterData characterData;
    private Civilian civilian;

    private float move_speed_mult = 1f;
    private float gather_mult = 1f;
    private bool depleting = false;

    private void Awake()
    {
        character = GetComponent<Character>();
        characterData = GetComponent<CharacterData>();
        civilian = GetComponent<Civilian>();
    }

    private void Start() {
        foreach (AttributeData attr in attributes)
        {
            if (!CharacterData.HasAttribute(attr.type))
                CharacterData.SetAttributeValue(attr.type, attr.start_value, attr.max_value);
        }
    }

    private void Update()
    {
        if (GameMgr.Instance.IsPaused())
            return;

        // if (character.IsDead())
        //     return;

        //Update attributes
        float game_speed = 0.1f * GameMgr.Instance.GetSpeedMultiplier();

        //Update Attributes
        foreach (AttributeData attr in attributes)
        {
            float update_value = attr.value_per_hour;
            update_value = update_value * game_speed * Time.deltaTime;
            CharacterData.AddAttributeValue(attr.type, update_value, attr.max_value);
        }

        //Penalty for depleted attributes
        move_speed_mult = 1f;
        gather_mult = 1f;
        depleting = false;

        foreach (AttributeData attr in attributes)
        {
            if (GetAttributeValue(attr.type) < 0.01f)
            {
                move_speed_mult = move_speed_mult * attr.deplete_move_mult;
                gather_mult = gather_mult * attr.deplete_gather_mult;
                float update_value = attr.deplete_hp_loss * game_speed * Time.deltaTime;
                AddAttribute(AttributeType.Health, update_value);
                if (attr.deplete_hp_loss < 0f)
                    depleting = true;
            }
        }
    }

    public void AddAttribute(AttributeType type, float value)
    {
        if (HasAttribute(type))// && !character.IsDead())
        {
            CharacterData.AddAttributeValue(type, value, GetAttributeMax(type));
        }
    }

    public void SetAttribute(AttributeType type, float value)
    {
        if (HasAttribute(type))// && !character.IsDead())
        {
            CharacterData.SetAttributeValue(type, value, GetAttributeMax(type));
        }
    }

    public bool HasAttribute(AttributeType type)
    {
        return CharacterData.HasAttribute(type) &&  GetAttribute(type) != null;
    }

    public float GetAttributeValue(AttributeType type)
    {
        return CharacterData.GetAttributeValue(type);
    }

    public float GetAttributeMax(AttributeType type)
    {
        AttributeData adata = GetAttribute(type);
        if (adata != null)
            return adata.max_value;
        return 100f;
    }

    public AttributeData GetAttribute(AttributeType type)
    {
        foreach (AttributeData attr in attributes)
        {
            if (attr.type == type)
                return attr;
        }
        return null;
    }

    public bool IsLow(AttributeType type)
    {
        AttributeData attr = GetAttribute(type);
        float val = GetAttributeValue(type);
        return (val <= attr.low_threshold);
    }

    public bool IsDepleted(AttributeType type)
    {
        float val = GetAttributeValue(type);
        return (val <= 0f);
    }

    public bool IsAnyDepleted()
    {
        foreach (AttributeData attr in attributes)
        {
            float val = GetAttributeValue(attr.type);
            if (val <= 0f)
                return true;
        }
        return false;
    }

    public bool IsFull(AttributeType type)
    {
        if(GetAttributeValue(type) >= GetAttributeMax(type)) return true;
        return false;
    }

    public int GetPriorityWork(string id)
    {
        if(id == "regen")
        {
            return Idle;
        }

        else{
            return Deployment;
        }
    }

    public Character Character { get { return character; } }
    public CharacterData CharacterData { get { return characterData; } }
    public Civilian Civilian { get { return civilian; } }
}
