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


    [Header("Targeting")]
    public AttackTeam target_team;
    public String target_group = "Hero";

    [SerializeField] TakeDamageEvent takeDamage;
    [SerializeField] float regenerationPercentage = 70;

    public static event UnityAction<Destructible> OnAnyDestructibleCreated;
    public static event UnityAction<Destructible> OnAnyDestructibleDestroyed;

    public UnityEvent onDie;

    LazyValue<float> healthPoints;
    private Interactable interact;
    private Character character;

    private DissolvingController dissolving;

    bool wasDeadLastFrame = false;

    protected virtual void Awake()
    {
        healthPoints = new LazyValue<float>(GetInitialHealth);
        interact = GetComponent<Interactable>();
        character = GetComponent<Character>();
        if (TryGetComponent<DissolvingController>(out DissolvingController d)) dissolving = d;
        OnAnyDestructibleCreated?.Invoke(this);


    }

    protected virtual void Start()
    {
        healthPoints.ForceInit();
    }

    protected virtual void Update()
    {
        if (healthPoints.value < MaxHP())
        {
            healthPoints.value += GetHPRegenRate() * Time.deltaTime;
            if (healthPoints.value > MaxHP())
            {
                healthPoints.value = MaxHP();
            }
        }
    }

    public float CurrentHP()
    {
        return healthPoints.value;
    }

    public float MaxHP()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    private float GetInitialHealth()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    public float GetHPRegenRate()
    {
        return GetComponent<BaseStats>().GetStat(Stat.HPRegenRate);
    }


    protected virtual void OnEnable()
    {
        GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
    }

    protected virtual void OnDisable()
    {
        GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
    }

    protected virtual void OnDestroy()
    {
        OnAnyDestructibleDestroyed?.Invoke(this);
    }

    public void TakeDamage(Character instigator, float damage)
    {
        healthPoints.value -= damage;
        healthPoints.value = Mathf.Clamp(healthPoints.value, 0f, 50000f);

        if (IsDead())
        {
            onDie.Invoke();
            AwardExperience(instigator);
        }
        else
        {
            takeDamage?.Invoke(damage);
        }
        UpdateState();
    }

    private void AwardExperience(Character instigator)
    {
        Debug.Log(instigator);
    }

    public bool IsDead()
    {
        return healthPoints.value <= 0;
    }

    private void UpdateState()
    {
        Animator animator = GetComponent<Animator>();
        if (!wasDeadLastFrame && IsDead())
        {
            animator.SetTrigger("die");
            character.Stop();

        }

        if (wasDeadLastFrame && !IsDead())
        {
            animator.Rebind();
        }

        wasDeadLastFrame = IsDead();
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

    private void RegenerateHealth()
    {
        float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * regenerationPercentage / 100;
        healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
    }

    private void OnDie()
    {
        Debug.Log("die");
        if (dissolving == null) return;
        StartCoroutine(dissolving.DissolveCo());
        Destroy(gameObject,3);
    }

    public Interactable Interactable { get { return interact; } }
    public Character Character { get { return character; } }
}
