using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    public float use_range = 0f;        //Distance from which characters can interact with this object
    public int use_max = 6;           //Maximum number of characters who can interact with this object

    [Header("Actions")]
    public ActionBasic[] actions;       //Default actions when interacting with this Interactable (right clicking on it)

    [Header("Interaction Point")]
    public Transform interact_root;
    public Transform[] interact_points;

    public UnityAction<Character> onTarget;
    public UnityAction<Character> onInteract;

    private Transform transf;
    private Task task;
    private Destructible destruct;

    private void Awake()
    {

        task = GetComponent<Task>();
        destruct = GetComponent<Destructible>();

        transf = transform;
        UpdateInteractPoints();
    }

    private void UpdateInteractPoints()
    {
        if (interact_root == null) return;
        List<Transform> lst = new List<Transform>();

        foreach (Transform trans in interact_root)
        {
            lst.Add(trans);
        }

        interact_points = lst.ToArray();
    }

    public void Interact(Character character)
    {
        onInteract?.Invoke(character);
    }

    public Vector3 GetInteractCenter()
    {
        if (interact_points.Length > 0)
            return interact_points[0].position;
        return transf.position;
    }

    public Vector3 GetInteractCenter(int index)
    {
        if (interact_points.Length > 0 && index >= 0)
        {
            int interact_point = index % interact_points.Length;
            return interact_points[interact_point].position;
        }
        return transf.position;
    }

    public Vector3 GetInteractPosition(int index)
    {
        Vector3 center = GetInteractCenter(index);
        float angle = (index * 360f / use_max) * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        Vector3 pos = center + dir * use_range;
        return pos;
    }

    public List<Vector3> GetInteractPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < use_max; i++)
        {
            Vector3 pos = GetInteractPosition(i);
            positions.Add(pos);
        }
        return positions;
    }

    public int GetInteractPositionIndex(Character character)
    {
        List<Character> icharacters = Character.GetAllTargeting(this);
        List<Vector3> positions = GetInteractPositions();
        HashSet<int> interact_index = new HashSet<int>();
        int nearest = 0;
        float min_dist = 99999f;
        foreach (Character acharacter in icharacters)
        {
            if (acharacter != character)
                interact_index.Add(acharacter.GetTargetPosIndex());
        }
        for (int i = 0; i < positions.Count; i++)
        {
            if (!interact_index.Contains(i))
            {
                float dist = (character.transform.position - positions[i]).magnitude;
                if (dist < min_dist)
                {
                    min_dist = dist;
                    nearest = i;
                }
            }
        }
        return nearest;
    }

    public bool IsInteractFull()
    {
        int nb = Character.CountTargetingTarget(this);
        return nb >= use_max;
    }

    public Task Task { get { return task; } }
    public Transform Transform { get { return transf; } }
    public Destructible Destructible { get { return destruct; } }
}
