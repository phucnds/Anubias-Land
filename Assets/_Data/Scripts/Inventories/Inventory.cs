using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [Tooltip("Allowed size")]
    [SerializeField] int inventorySize = 20;

    public InventorySlot[] slots;

    public struct InventorySlot
    {
        public InventoryItem item;
        public int number;
    }
    public event UnityAction inventoryUpdated;

    private void Awake()
    {
        slots = new InventorySlot[inventorySize];
    }


    /// <summary>
    /// Could this item fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceFor(InventoryItem item)
    {
        return FindSlot(item) >= 0;
    }

    public bool HasSpaceFor(IEnumerable<InventoryItem> items)
    {
        int freeSlot = FreeSlots();
        List<InventoryItem> stackedItems = new List<InventoryItem>();
        foreach (var item in items)
        {
            if (item.IsStackable())
            {
                if (HasItem(item)) continue;
                if (stackedItems.Contains(item)) continue;
                stackedItems.Add(item);
            }
            if (freeSlot <= 0) return false;
            freeSlot--;
        }

        return true;
    }

    public int FreeSlots()
    {
        int count = 0;
        foreach (InventorySlot slot in slots)
        {
            if (slot.number == 0)
                count++;
        }
        return count;
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetSize()
    {
        return slots.Length;
    }

    /// <summary>
    /// Attempt to add the items to the first available slot.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>Whether or not the item could be added.</returns>
    public bool AddToFirstEmptySlot(InventoryItem item, int number)
    {

        foreach (var store in GetComponents<IItemStore>())
        {
            number -= store.AddItems(item, number);
        }

        if (number <= 0) return true;

        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots[i].item = item;
        slots[i].number += number;

        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }

        return true;
    }

    /// <summary>
    /// Is there an instance of the item in the inventory?
    /// </summary>
    public bool HasItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return true;
            }
        }
        return false;
    }

    public bool HasItem(InventoryItem item, int number)
    {
        int count = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                count++;
            }
        }
        return count >= number;
    }

    /// <summary>
    /// Return the item type in the given slot.
    /// </summary>
    public InventoryItem GetItemInSlot(int slot)
    {
        return slots[slot].item;
    }

    public int GetNumberInSlot(int slot)
    {
        return slots[slot].number;
    }

    public int GetItemSlot(InventoryItem item, int number)
    {
        // Loop through all of the inventory slots. 
        for (int i = 0; i < slots.Length; i++)
        {
            // Check if the current iteration's item equals to the parameter item.
            // Check if the current iteration's item number equals to or greater than the parameter number.
            if (object.ReferenceEquals(slots[i].item, item) && GetNumberInSlot(i) >= number)
            {
                // If true, return the slot number
                return i;
            }
        }
        // Return -1 by default.
        // When using this method to check if a slot is found, we simply check if the returned value equals to or greater than 0.
        return -1;
    }

    /// <summary>
    /// Remove the item from the given slot.
    /// </summary>
    public void RemoveFromSlot(int slot, int number)
    {
        slots[slot].number -= number;
        if (slots[slot].number <= 0)
        {
            slots[slot].number = 0;
            slots[slot].item = null;
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }

    /// <summary>
    /// Will add an item to the given slot if possible. If there is already
    /// a stack of this type, it will add to the existing stack. Otherwise,
    /// it will be added to the first empty slot.
    /// </summary>
    /// <param name="slot">The slot to attempt to add to.</param>
    /// <param name="item">The item type to add.</param>
    /// <returns>True if the item was added anywhere in the inventory.</returns>
    public bool AddItemToSlot(int slot, InventoryItem item, int number)
    {
        if (slots[slot].item != null)
        {
            return AddToFirstEmptySlot(item, number); ;
        }

        var i = FindStack(item);
        if (i >= 0)
        {
            slot = i;
        }

        slots[slot].item = item;
        slots[slot].number += number;
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
        return true;
    }

    // PRIVATE



    /// <summary>
    /// Find a slot that can accomodate the given item.
    /// </summary>
    /// <returns>-1 if no slot is found.</returns>
    private int FindSlot(InventoryItem item)
    {
        int i = FindStack(item);
        if (i < 0)
        {
            i = FindEmptySlot();
        }
        return i;
    }

    /// <summary>
    /// Find an empty slot.
    /// </summary>
    /// <returns>-1 if all slots are full.</returns>
    private int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return i;
            }
        }
        return -1;
    }

    private int FindStack(InventoryItem item)
    {
        if (!item.IsStackable())
        {
            return -1;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i].item, item))
            {
                return i;
            }
        }
        return -1;
    }

    [System.Serializable]
    private struct InventorySlotRecord
    {
        public string itemID;
        public int number;
    }
}
