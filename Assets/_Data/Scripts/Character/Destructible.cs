using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttackTeam
{
    Neutral = 0, //Can be attacked by anyone, but won't be attacked automatically, use for resources
    Ally = 10,   //Will be attacked automatically by enemies, cant be attacked by colonists
    Enemy = 20,  //Will be attacked automatically by colonists
    CantBeAttacked = 50, //Cannot be attacked
}

[Serializable]
public class TakeDamageEvent : UnityEvent<float> { }

public class Destructible : MonoBehaviour
{
    [Header("Stats")]
    public float HP = 50000;

    [Header("Targeting")]
    public AttackTeam target_team;
    public String target_group = "Hero";

    [SerializeField] TakeDamageEvent takeDamage;


    public static event UnityAction<Destructible> OnAnyDestructibleCreated;
    public static event UnityAction<Destructible> OnAnyDestructibleDestroyed;

    




    private Interactable interact;

    private void Awake()
    {
        interact = GetComponent<Interactable>();
        OnAnyDestructibleCreated?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnAnyDestructibleDestroyed?.Invoke(this);
    }

    public void TakeDamage(Character attacker, float damage)
    {
        // if (attacker.Colonist != null)
        // {
        //     attacker.Colonist.Attributes.AddXP(BonusType.AttackValue, damage, interact);
        //     attacker.Colonist.Attributes.AddXP(BonusType.AttackPercent, damage, interact);
        // }

        TakeDamage(damage);
        takeDamage?.Invoke(damage);
    }

    // //Take damage from source
    // public void TakeDamage(Selectable attacker, int damage)
    // {
    //     TakeDamage(damage);
    //     if (!dead && attacker != null)
    //         onDamagedBy?.Invoke(attacker);
    // }

    //Take damage from no sources
    public void TakeDamage(float damage)
    {
        HP -= damage;
        HP = Mathf.Clamp(HP, 0f, 50000f);
    }

    public bool IsDead()
    {
        return HP <= 0;
    }

    public bool CanBeAttacked()
    {
        return target_team != AttackTeam.CantBeAttacked && !IsDead();
    }

    public static Destructible GetNearest(AttackTeam team, Vector3 pos, float range = 999f)
    {
        return GetNearest(team, null, pos, range);
    }

    public static Destructible GetNearest(AttackTeam team, Destructible skip, Vector3 pos, float range = 999f)
    {
        Destructible nearest = null;
        float min_dist = range;
        foreach (Destructible destruct in GameMgr.Instance.DestructibleManager.GetListDestruct())
        {
            if (destruct != null && destruct != skip && !destruct.IsDead())
            {
                float dist = (destruct.transform.position - pos).magnitude;
                if (dist < min_dist && destruct.target_team == team)
                {
                    min_dist = dist;
                    nearest = destruct;
                }
            }
        }
        return nearest;
    }

    public Interactable Interactable { get { return interact; } }
}
