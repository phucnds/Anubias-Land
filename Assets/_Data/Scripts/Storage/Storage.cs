using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Storage : MonoBehaviour
{
    public static event UnityAction<Storage> OnAnyStorageCreated;
    public static event UnityAction<Storage> OnAnyStorageDestroyed;

    private Interactable interact;

    public int inventoryItem = 0;

    private void Awake()
    {
        interact = GetComponent<Interactable>();
    }

    private void Start()
    {
        OnAnyStorageCreated?.Invoke(this);
    }

    private void OnDestroy()
    {
        OnAnyStorageDestroyed?.Invoke(this);
    }

    public static Storage GetNearestActive(Vector3 pos, float range = 999f)
    {
        float min_dist = range;
        Storage nearest = null;
        foreach (Storage storage in GameMgr.Instance.StorageManager.GetListStorage())
        {
            float dist = (pos - storage.transform.position).magnitude;
            if (dist < min_dist) //&& storage.IsActive()
            {
                min_dist = dist;
                nearest = storage;
            }
        }
        return nearest;
    }

    public Interactable Interactable { get { return interact; } }
}
