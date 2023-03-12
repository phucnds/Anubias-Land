using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionShopping", menuName = "Anubias-Land/Actions/ActionShopping", order = 0)]
public class ActionShopping : ActionBasic
{
    public override void StartAction(Character character, Interactable target)
    {
        character.StopAnimate();
    }

    public override void StopAction(Character character, Interactable target)
    {
        character.StopAnimate();
    }

    public override void UpdateAction(Character character, Interactable target)
    {
        float speed = 0.05f * GameMgr.Instance.GetSpeedMultiplier();
       
        character.AddActionProgress(speed * Time.deltaTime);
        if (character.GetActionProgress() > 1f)
        {
            float value = character.Civilian.Attributes.GetAttributeMax(AttributeType.Satiety);
            character.SetActionProgress(0f);
            character.Civilian.Attributes.AddAttribute(AttributeType.Satiety, 10);
            character.Stop();

            Shop shop = target.GetComponent<Shop>();
            if (character.Civilian.Attributes.GetAttributeValue(AttributeType.Satiety) < character.Civilian.Attributes.GetAttributeMax(AttributeType.Satiety) * 0.8)
            {
                Shop shopNext = Shop.GetRandom(character.transform.position,999,shop);
                character.OrderInterupt(this, shopNext.Interactable);
            }
        }
    }

    public override bool CanDoAction(Character character, Interactable target)
    {
        return target != null;
    }
}
