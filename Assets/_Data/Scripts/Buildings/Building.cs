using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : MonoBehaviour
{
    public static event UnityAction<Building> OnAnyBuildingCreated;
    public static event UnityAction<Building> OnAnyBuildingDetroyed;

    protected virtual void  Start() {
        OnAnyBuildingCreated?.Invoke(this);
    }

    protected virtual void OnDestroy() {
        OnAnyBuildingDetroyed?.Invoke(this);
    }

}
