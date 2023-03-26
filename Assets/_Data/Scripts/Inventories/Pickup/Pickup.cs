using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    InventoryItem item;
    int number;

    Inventory inventory;

    private void Awake()
    {
        inventory = GameMgr.Instance.Inventory;
    }

    public void Setup(InventoryItem item, int number)
    {
        this.item = item;
        if (!item.IsStackable())
        {
            number = 1;
        }
        this.number = number;
    }

    public int GetNumber()
    {
        return number;
    }

    public InventoryItem GetItem()
    {
        return item;
    }

    public void PickupItem()
    {
        bool foundSlot = inventory.AddToFirstEmptySlot(item, number);
        if (foundSlot)
        {
            Destroy(gameObject);
        }
    }

    public bool CanBePickedUp()
    {
        return inventory.HasSpaceFor(item);
    }
}
