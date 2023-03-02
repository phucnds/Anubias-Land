using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private AILerp agent;
    private Animator animator;
    private AIDestinationSetter destinationSetter;

    IAstarAI ai;
    Vector3 target;

    private void Awake() {
        agent = GetComponent<AILerp>();
        animator = GetComponent<Animator>();
        ai = GetComponent<IAstarAI>();
        
    }

    void OnEnable()
    {
        ai = GetComponent<IAstarAI>();
        if (ai != null) ai.onSearchPath += Update;
    }

    void OnDisable()
    {
        if (ai != null) ai.onSearchPath -= Update;
    }

    private void Start() {
    }

    void Update()
    {
        UpdateAnimator();

        if (target != null && ai != null) ai.destination = target;
    }

    private void UpdateAnimator()
    {

        Vector3 localVelocity = GetLocalVelocity();
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }

    private Vector3 GetLocalVelocity()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        return localVelocity;
    }

    public void MoveTo(Vector3 pos)
    {
        ai.canMove = true;
        target = pos;
    }

    public void Stop()
    {
        ai.canMove = false;
        
    }

    public bool HasReachedTarget()
    {
        return ai.reachedDestination;
    }
}
