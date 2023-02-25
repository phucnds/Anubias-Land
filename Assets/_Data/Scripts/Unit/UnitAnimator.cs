using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += moveAction_OnStartMoving;
            moveAction.OnStopMoving += moveAction_OnStopMoving;
        }
    }

    private void moveAction_OnStartMoving()
    {
        animator.SetBool("IsWalking", true);
    }

    private void moveAction_OnStopMoving()
    {
        animator.SetBool("IsWalking", false);
    }
}
