using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public static event UnityAction<Building> OnAnyBuildingCreated;
    public static event UnityAction<Building> OnAnyBuildingDetroyed;

    protected Interactable interact;

    protected virtual void Awake()
    {
        interact = GetComponent<Interactable>();
    }

    protected virtual void Start()
    {
        OnAnyBuildingCreated?.Invoke(this);
    }

    protected virtual void OnDestroy()
    {
        OnAnyBuildingDetroyed?.Invoke(this);
    }

    public Interactable Interactable { get { return interact; } }

}
