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
    [SerializeField] Transform rightHandTransform = null;
    [SerializeField] Transform leftHandTransform = null;
    [SerializeField] Transform leftHandShieldTransform = null;
    [SerializeField] WeaponConfig defaultWeapon = null;

    [SerializeField] private AttackType attack_type = AttackType.Melee;
    [SerializeField] private float attack_cooldown = 1f;

    public UnityAction<Destructible> onAttack;
    public UnityAction<Destructible> onAttackHit;

    private Character character;
    private Destructible destruct;

    private Coroutine attack_routine = null;
    private float attack_timer = 0f;
    private bool is_attacking = false;
    private float timeWindup = 0.2f;

    WeaponConfig currentWeaponConfig;
    LazyValue<Weapon> currentWeapon;

    void Awake()
    {
        character = GetComponent<Character>();
        destruct = GetComponent<Destructible>();

        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
    }

    private void Start()
    {
        currentWeapon.ForceInit();
    }

    private Weapon SetupDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
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

    private IEnumerator AttackRun(Destructible target)
    {
        character.FaceToward(target.transform.position);
        character.Wait();
        is_attacking = true;

        if (onAttack != null)
            onAttack.Invoke(target);

        yield return new WaitForSeconds(timeWindup);
        DoAttackStrike(target);

        attack_timer = 0f;

        character.StopWait();
        is_attacking = false;
    }

    private void DoAttackStrike(Destructible target)
    {
        if (target == null)
            return;

        //Ranged attack
        if (currentWeaponConfig.HasProjectile())
        {
            currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform, target, character, GetAttackDamage(target));
        }

        //Melee attack
        else if (IsAttackTargetInRange(target.Interactable))
        {
            target.TakeDamage(character, GetAttackDamage(target));
            //Debug.Log(GetAttackDamage(target));

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
            float dist = Vector3.Distance(target.transform.position, transform.position);
            //(target.transform.position - transform.position).magnitude; 
            return dist <= GetTargetAttackRange(target);
        }
        return false;
    }

    public float GetTargetAttackRange(Interactable target)
    {
        return GetAttackRange() + target.use_range;
    }

    public float GetAttackRange()
    {
        return currentWeaponConfig.GetWeaponRange();
    }

    public float GetAttackDamage(Destructible target)
    {
        float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
        BaseStats baseStats = target.GetComponent<BaseStats>();

        if (baseStats != null)
        {
            float defence = baseStats.GetStat(Stat.Defence);
            damage /= 1 + defence / damage;
        }

        return damage;
    }

    public float GetAttackSpeedMultiplier()
    {
        float att_speed = 1f;//+ character.GetBonusValue(BonusType.AttackSpeed);
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

    private Weapon AttachWeapon(WeaponConfig weapon)
    {
        //Debug.Log("AttachWeapon");
        Animator animator = GetComponent<Animator>();
        return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }


}
