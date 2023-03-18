using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionAttack", menuName = "Anubias-Land/Actions/ActionAttack", order = 0)]
public class ActionAttack : ActionBasic
{
    public override void StartAction(Character character, Interactable target)
    {
        character.FaceToward(target.transform);
    }

    public override void StopAction(Character character, Interactable target)
    {
        character.StopAnimate();
    }

    public override void UpdateAction(Character character, Interactable target)
    {
        Destructible destruct = target.Destructible;
        if (destruct.IsDead())
        {
            FindNextTarget(character);
            return;
        }

        if (character.Attack.IsCooldownReady())
        {
            if (!character.Attack.IsAttackTargetInRange(destruct.Interactable))
            {
                AttackTarget(character, destruct);
                Debug.Log("Move to target");
            }
                
            else
            {
                character.FaceToward(target.transform);
                Debug.Log("in range");
                character.Attack.AttackStrike(destruct);
            }
                
        }
    }

    public override bool CanDoAction(Character character, Interactable target)
    {
        Destructible destruct = target != null ? target.Destructible : null;
        return destruct != null && character.Attack != null && character.Attack.CanAttack(destruct);
    }

    private void AttackTarget(Character character, Destructible target)
    {
        character.StopAction();
        ActionAttack attack = ActionBasic.Get<ActionAttack>();
        if (attack != null && target != null)
        {
            character.OrderInterupt(attack, target.Interactable);
        }
    }

    private void FindNextTarget(Character character)
    {
        character.Stop();
    }
}