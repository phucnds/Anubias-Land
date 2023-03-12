using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnim : MonoBehaviour
{
    public string move_anim = "move";
    public string attack_anim = "attack";
    public string death_anim = "death";

    public UnityAction<string> onTrigger;

    private Character character;
    private Animator animator;
    private bool was_paused;
    private float prev_speed = 1f;
    private float speed = 1f;
    private string custom_anim;

    private Dictionary<string, UnityAction> trigger_list = new Dictionary<string, UnityAction>();

    void Awake()
    {
        character = GetComponentInParent<Character>();
        animator = GetComponent<Animator>();
        if (animator != null)
            speed = animator.speed;
    }

    private void Start()
    {
        if (character.Attack != null)
            character.Attack.onAttack += OnAttack;
    }

    void Update()
    {
        bool is_paused = GameMgr.Instance.GetSpeedMultiplier() <= 0;
        float mult = GameMgr.Instance.GetSpeedMultiplier();

        if (animator != null)
        {
            if (is_paused != was_paused || !prev_speed.Equals(mult))
                animator.speed = is_paused ? 0f : speed * mult;

            animator.SetBool(move_anim, character.IsReallyMoving());
        }

        was_paused = is_paused;
        prev_speed = mult;
    }

    private void OnAttack(Destructible target)
    {
        string anim = attack_anim;
        animator?.SetTrigger(anim);
    }

    private void OnDeath()
    {
        animator?.SetTrigger(death_anim);
    }

    public void PlayAnim(string anim_id)
    {
        if (animator != null && !string.IsNullOrEmpty(anim_id))
            animator.SetTrigger(anim_id);
    }

    public void Animate(string anim_id, bool active)
    {
        if (animator != null && !string.IsNullOrEmpty(custom_anim))
            animator.SetBool(custom_anim, false);
        if (active)
            custom_anim = anim_id;
        if (animator != null && !string.IsNullOrEmpty(anim_id))
            animator.SetBool(anim_id, active);
    }

    public void StopAnimate()
    {
        if (animator != null && !string.IsNullOrEmpty(custom_anim))
            animator.SetBool(custom_anim, false);
        custom_anim = "";
    }

    public bool IsCustomAnim()
    {
        return !string.IsNullOrEmpty(custom_anim);
    }

    public void SetEvent(string id, UnityAction callback)
    {
        if (!string.IsNullOrEmpty(id))
            trigger_list[id] = callback;
    }

    public void TriggerEvent(string id)
    {
        foreach (KeyValuePair<string, UnityAction> pair in trigger_list)
        {
            if (pair.Key == id)
                pair.Value?.Invoke();
        }
        onTrigger?.Invoke(id);
    }
}