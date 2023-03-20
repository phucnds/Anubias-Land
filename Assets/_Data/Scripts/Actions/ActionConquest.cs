using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionConquest", menuName = "Anubias-Land/Actions/ActionConquest", order = 0)]
public class ActionConquest : ActionBasic
{
    public float wander_min = 8f;
    public float wander_range = 40f;    //How far from the starting pos can it wander
    public float wander_interval = 10f; //Interval between changing wander position

    private Vector3 start_pos = Vector3.zero;
    private float state_timer = 0f;

    public override void StartAction(Character character, Interactable target)
    {
        Fortress fortress = Fortress.GetNearest(Vector3.zero);
        start_pos = fortress.transform.position;
    }

    public override void UpdateAction(Character character, Interactable target)
    {
        if (character.HasReachedTarget())
        {
            float mult = GameMgr.Instance.GetSpeedMultiplier();
            state_timer += mult * Time.deltaTime;
        }

        if (state_timer > wander_interval)
        {
            state_timer = Random.Range(-1f, 1f);
            FindWanderTarget(character, target);

            character.Civilian.Attributes.Deployment -= 5;
            if (character.Civilian.Attributes.Deployment <= 0) character.Civilian.Attributes.Idle = 100;
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
