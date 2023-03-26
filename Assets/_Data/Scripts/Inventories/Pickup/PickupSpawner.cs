using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] InventoryItem item = null;
    [SerializeField] int number = 1;

    private void Awake()
    {
        // Spawn in Awake so can be destroyed by save system after.
        SpawnPickup();
    }

    public Pickup GetPickup()
    {
        return GetComponentInChildren<Pickup>();
    }

    /// <summary>
    /// True if the pickup was collected.
    /// </summary>
    public bool isCollected()
    {
        return GetPickup() == null;
    }

    private void SpawnPickup()
    {
        var spawnedPickup = item.SpawnPickup(transform.position, number);
        spawnedPickup.transform.SetParent(transform);
    }

    private void DestroyPickup()
    {
        if (GetPickup())
        {
            Destroy(GetPickup().gameObject);
        }
    }
}
