using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionRest", menuName = "Anubias-Land/Actions/ActionRest", order = 0)]
public class ActionRest : ActionBasic
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
        float speed = 0.1f * GameMgr.Instance.GetSpeedMultiplier();
        Inns inns = target.GetComponent<Inns>();
        character.AddActionProgress(speed * Time.deltaTime);
        if (character.GetActionProgress() > 1f)
        {
            float value = character.Civilian.Attributes.GetAttributeMax(AttributeType.Stamina);
            character.SetActionProgress(0f);
            character.Civilian.Attributes.AddAttribute(AttributeType.Stamina, value);
            character.Stop();
        }

    }

    





}
