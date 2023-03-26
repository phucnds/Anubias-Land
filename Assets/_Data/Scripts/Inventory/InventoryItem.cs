using System.Collections;
using System.Collections.Generic;
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

public abstract class InventoryItem : ScriptableObject
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
}
