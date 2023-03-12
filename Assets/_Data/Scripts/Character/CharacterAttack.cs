using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttackType
{
    None = 0,
    Melee = 5,
    Ranged = 10,
}

public class CharacterAttack : MonoBehaviour
{
    public AttackType attack_type = AttackType.Melee;
    public int attack_damage = 5;       //Basic damage without equipment
    public float attack_range = 1.2f;   //How far can you attack (melee)
    public float attack_cooldown = 1f;  //Seconds of waiting in between each attack
    public float attack_windup = 0.7f;  //Timing (in secs) between the start of the attack and the hit
    public float attack_windout = 0.4f; //Timing (in secs) between the hit and the end of the attack
                                        //public float attack_energy = 1f;    //Energy cost to attack (not implemented yet)
    public GameObject default_projectile; //Default projectile prefab (ranged attack only)

    public UnityAction<Destructible> onAttack;
    public UnityAction<Destructible> onAttackHit;

    private Character character;
    private Destructible destruct;

    private Coroutine attack_routine = null;
    private float attack_timer = 0f;
    private bool is_attacking = false;

    void Awake()
    {
        character = GetComponent<Character>();
        destruct = GetComponent<Destructible>();
    }

    void Update()
    {
        if (GameMgr.Instance.IsPaused())
            return;

        if (/*IsDead() || */character.IsWaiting())
            return;

        //Attack when target is in range
        float mult = GameMgr.Instance.GetSpeedMultiplier();
        attack_timer += Time.deltaTime * mult;
    }

    public void AttackStrike(Destructible target)
    {
        if (!character.IsWaiting() && CanAttack())
        {
            attack_timer = -10f; //Willbe set to 0f after the strike
            attack_routine = StartCoroutine(AttackRun(target));
        }
    }

    public bool CanAttack()
    {
        return attack_type != AttackType.None;
    }

    //Melee or ranged targeting one target
    private IEnumerator AttackRun(Destructible target)
    {
        character.Wait();
        is_attacking = true;

        //Start animation
        if (onAttack != null)
            onAttack.Invoke(target);

        //Face target
        character.FaceToward(target.transform.position);

        //Wait for windup
        float windup = attack_windup;
        yield return new WaitForSeconds(windup);

        DoAttackStrike(target);

        //Reset timer
        attack_timer = 0f;

        //Wait for the end of the attack before character can move again
        float windout = attack_windout;
        yield return new WaitForSeconds(windout);

        character.StopWait();
        is_attacking = false;
    }

    private void DoAttackStrike(Destructible target)
    {
        if (target == null)
            return;

        //Ranged attack
        // bool is_ranged = attack_type == AttackType.Ranged //|| HasRangedWeapon();
        // ItemEquipData equipped = GetBestWeapon();
        // ItemProjData projectile = GetRangedProjectile(equipped);
        // GameObject proj_default = GetDefaultProjectile();
        // if (is_ranged && (projectile != null || proj_default != null))
        // {
        //     int damage = GetAttackDamage(target);
        //     if (projectile != null)
        //         damage += projectile.damage_bonus;

        //     Vector3 pos = GetProjectileSpawnPos();
        //     Vector3 dir = target.GetCenter() - pos;
        //     GameObject prefab = projectile != null ? projectile.projectile_prefab : proj_default;
        //     GameObject proj = Instantiate(prefab, pos, Quaternion.LookRotation(dir.normalized, Vector3.up));
        //     Projectile project = proj.GetComponent<Projectile>();
        //     project.target = target;
        //     project.shooter = select;
        //     project.shooter_character = character;
        //     project.dir = dir.normalized;
        //     project.damage = damage;

        //     //Remove projectile
        //     if (projectile != null && character.Equip != null)
        //         character.Equip.EquipData.AddItem(projectile.id, -1);
        // }

        //Melee attack
        if (IsAttackTargetInRange(target.Interactable))
        {
            target.TakeDamage(character, GetAttackDamage(target));

            if (onAttackHit != null)
                onAttackHit.Invoke(target);
        }
    }

    public void CancelAttack()
    {
        if (is_attacking)
        {
            is_attacking = false;
            attack_timer = 0f;
            character.StopMove();
            if (attack_routine != null)
                StopCoroutine(attack_routine);
        }
    }

    public bool IsAttacking()
    {
        return is_attacking;
    }

    //Is the character in fighting mode?
    public bool IsFighting()
    {
        return is_attacking || character.GetCurrentAction() is ActionAttack;
    }

    public bool IsAttackTargetInRange(Interactable target)
    {
        if (target != null)
        {
            float dist = (target.transform.position - transform.position).magnitude;
            return dist < GetTargetAttackRange(target);
        }
        return false;
    }

    public float GetTargetAttackRange(Interactable target)
    {
        return GetAttackRange() + target.use_range;
    }

    public float GetAttackRange()
    {
        return attack_range;
    }

    public int GetAttackDamage(Destructible target)
    {
        return attack_damage;
    }

    public float GetAttackSpeedMultiplier()
    {
        float att_speed = 1f ;//+ character.GetBonusValue(BonusType.AttackSpeed);
        return att_speed * GameMgr.Instance.GetSpeedMultiplier();
    }

    public float GetAttackCooldown()
    {
        return attack_cooldown / GetAttackSpeedMultiplier();
    }

    public bool IsCooldownReady()
    {
        return attack_timer > GetAttackCooldown();
    }

    public bool CanAttack(Destructible target)
    {
        return target != null && target.CanBeAttacked()
            && target.target_team != GetAttackGroup()
            && (target.target_group == null || target.target_group != GetTeamGroup());
    }

    public AttackTeam GetAttackGroup()
    {
        if (destruct != null)
            return destruct.target_team;
        return AttackTeam.CantBeAttacked;
    }

    public string GetTeamGroup()
    {
        if (destruct != null)
            return destruct.target_group;
        return null;
    }
}
