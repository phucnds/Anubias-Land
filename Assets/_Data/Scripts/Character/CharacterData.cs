using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    private float Stamina = 0;
    private float Satiety = 0;

    public Dictionary<AttributeType, float> attributes = new Dictionary<AttributeType, float>();


    private void Update() {
        Stamina = GetAttributeValue(AttributeType.Stamina);
        Satiety = GetAttributeValue(AttributeType.Satiety);
    }

    public bool HasAttribute(AttributeType type)
    {
        return attributes.ContainsKey(type);
    }

    public float GetAttributeValue(AttributeType type)
    {
        if (attributes.ContainsKey(type))
            return attributes[type];
        return 0f;
    }

    public void SetAttributeValue(AttributeType type, float value)
    {
        attributes[type] = value;
    }

    public void AddAttributeValue(AttributeType type, float value)
    {
        if (attributes.ContainsKey(type))
            attributes[type] += value;
    }

    public void SetAttributeValue(AttributeType type, float value, float max)
    {
        attributes[type] = Mathf.Clamp(value, 0f, max);
    }

    public void AddAttributeValue(AttributeType type, float value, float max)
    {
        if (attributes.ContainsKey(type))
        {
            attributes[type] += value;
            attributes[type] = Mathf.Clamp(attributes[type], 0f, max);
        }
    }
}
