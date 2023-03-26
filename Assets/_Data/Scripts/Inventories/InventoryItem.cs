using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemCategory
{
    None,
    Weapon,
    Shield,
    Accessory,
    Armor,
    Abilities
}

public abstract class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
{
    [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
    [SerializeField] string itemID = null;
    [Tooltip("Item name to be displayed in UI.")]
    [SerializeField] string displayName = null;
    [Tooltip("Item description to be displayed in UI.")]
    [SerializeField][TextArea] string description = null;
    [Tooltip("The UI icon to represent this item in the inventory.")]
    [SerializeField] Sprite icon = null;
    [Tooltip("The prefab that should be spawned when this item is dropped.")]
    [SerializeField] Pickup pickup = null;
    [Tooltip("If true, multiple items of this type can be stacked in the same inventory slot.")]
    [SerializeField] bool stackable = false;
    [SerializeField] float price;
    [SerializeField] ItemCategory category = ItemCategory.None;

    static Dictionary<string, InventoryItem> itemLookupCache;

    public static InventoryItem GetFromID(string itemID)
    {
        if (itemLookupCache == null)
        {
            itemLookupCache = new Dictionary<string, InventoryItem>();
            var itemList = Resources.LoadAll<InventoryItem>("");
            foreach (var item in itemList)
            {
                if (item.itemID == null)
                {
                    Debug.LogError($"{item.name} does not have a valied itemID!");
                    continue;
                }

                if (itemLookupCache.ContainsKey(item.itemID))
                {
                    Debug.LogError(string.Format("Looks like there's a duplicate InventorySystem ID for objects: {0} and {1}", itemLookupCache[item.itemID], item));
                    continue;
                }

                itemLookupCache[item.itemID] = item;
            }
        }

        if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
        return itemLookupCache[itemID];
    }

    public Pickup SpawnPickup(Vector3 position, int number)
    {
        var pickup = Instantiate(this.pickup);
        pickup.transform.position = position;
        pickup.Setup(this, number);
        return pickup;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetItemID()
    {
        return itemID;
    }

    public bool IsStackable()
    {
        return stackable;
    }

    public string GetDisplayName()
    {
        return displayName;
    }

    public float GetPrice()
    {
        return price;
    }

    public ItemCategory GetCatelogy()
    {
        return category;
    }

    public virtual string GetDescription()
    {
        return description;
    }

    public string GetRawDescription()
    {
        return description;
    }

    public Pickup GetPickup()
    {
        return pickup;
    }

    public void OnBeforeSerialize()
    {
        if (string.IsNullOrWhiteSpace(itemID))
        {
            itemID = System.Guid.NewGuid().ToString();
        }
    }

    public void OnAfterDeserialize()
    {

    }


}
