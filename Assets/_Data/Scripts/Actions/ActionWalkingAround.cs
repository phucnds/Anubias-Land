using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionWalkingAround", menuName = "Anubias-Land/Actions/ActionWalkingAround", order = 0)]
public class ActionWalkingAround : ActionBasic
{
    public float wander_min = 5f;
    public float wander_range = 15f;    //How far from the starting pos can it wander
    public float wander_interval = 10f; //Interval between changing wander position

    private Vector3 start_pos = Vector3.zero;
    private float state_timer = 0f;

    public override void StartAction(Character character, Interactable target)
    {

    }

    public override void UpdateAction(Character character, Interactable target)
    {
        if (character.HasReachedTarget() || !character.IsReallyMoving())
        {
            float mult = GameMgr.Instance.GetSpeedMultiplier();
            state_timer += mult * Time.deltaTime;
        }

        if (state_timer > wander_interval)
        {
            Inns inns = Inns.GetNearest(character.transform.position);
            Shop shop = Shop.GetNearest(character.transform.position);

            if (character.Civilian.Attributes.IsLow(AttributeType.Stamina) && inns != null)
            {
                ActionRest rest = ActionBasic.Get<ActionRest>();
                character.OrderInterupt(rest, inns.Interactable);
            }
            else if (character.Civilian.Attributes.IsLow(AttributeType.Satiety) && shop != null)
            {
                ActionShopping shopping = ActionBasic.Get<ActionShopping>();
                character.OrderInterupt(shopping, shop.Interactable);
            }
            else
            {
                state_timer = Random.Range(-1f, 1f);
                FindWanderTarget(character, target);
            }

        }

    }

    private void FindWanderTarget(Character character, Interactable target)
    {
        float range = Random.Range(wander_min, wander_range);
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 spos = start_pos;
        Vector3 pos = spos + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * range;
        character.Move(pos);
    }
}
