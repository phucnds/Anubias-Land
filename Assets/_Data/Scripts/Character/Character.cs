using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Character : MonoBehaviour
{

    [SerializeField] private BasicAction currentAction = null;


    public bool isIdle = false;
    public bool is_moving = false;


    private AILerp agent;
    private Animator animator;
    private AIDestinationSetter destinationSetter;
    
    
    private void Awake() {
        agent= GetComponent<AILerp>();
        animator = GetComponent<Animator>();
        

    }

    void Update()
    {
        UpdateAnimator();
        is_moving = IsMoving();
        isIdle = IsIdle();
    }

    private void UpdateAnimator() {

        Vector3 localVelocity = GetLocalVelocity();
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }


    private bool IsMoving()
    {
        Vector3 localVelocity = GetLocalVelocity();
        return localVelocity.z > 0;
    }

    private Vector3 GetLocalVelocity()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        return localVelocity;
    }

    private bool IsIdle()
    {
        return !IsMoving() && currentAction == null;
    }

    private bool ReachedDestination()
    {
        return agent.reachedDestination;
    }
}
