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

    #region InventoryEditor Additions

#if UNITY_EDITOR
    
    protected void Dirty()
    {
        EditorUtility.SetDirty(this);
    }

    protected void SetUndo(string message)
    {
        Undo.RecordObject(this, message);
    }

    protected bool FloatEquals(float value1, float value2)
    {
        return Math.Abs(value1 - value2) < .001f;
    }


    void SetDisplayName(string newDisplayName)
    {
        if (newDisplayName == displayName) return;
        SetUndo("Change Display Name");
        displayName = newDisplayName;
        Dirty();
    }

    void SetDescription(string newDescription)
    {
        if (newDescription == description) return;
        SetUndo("Change Description");
        description = newDescription;
        Dirty();
    }

    void SetPrice(float newPrice)
    {
        if (FloatEquals(price, newPrice)) return;
        SetUndo("Set Price");
        price = newPrice;
        Dirty();
    }

    void SetIcon(Sprite newIcon)
    {
        if (icon == newIcon) return;
        SetUndo("Change Icon");
        icon = newIcon;
        Dirty();
    }

    void SetPickup(GameObject potentialnewPickup)
    {
        if (!potentialnewPickup)
        {
            SetUndo("Set No Pickup");
            pickup = null;
            Dirty();
            return;
        }
        if (!potentialnewPickup.TryGetComponent(out Pickup newPickup)) return;
        if (pickup == newPickup) return;
        SetUndo("Change Pickup");
        pickup = newPickup;
        Dirty();
    }

    void SetItemID(string newItemID)
    {
        if (itemID == newItemID) return;
        SetUndo("Change ItemID");
        itemID = newItemID;
        Dirty();
    }

    void SetStackable(bool newStackable)
    {
        if (stackable == newStackable) return;
        SetUndo(stackable ? "Set Not Stackable" : "Set Stackable");
        stackable = newStackable;
        Dirty();
    }

    bool drawInventoryItem = true;
    [NonSerialized] protected GUIStyle foldoutStyle;
    [NonSerialized] protected GUIStyle contentStyle;
    public virtual void DrawCustomInspector()
    {
        contentStyle = new GUIStyle { padding = new RectOffset(15, 15, 0, 0) };
        GUIStyle expandedAreaStyle = new GUIStyle(EditorStyles.textArea) { wordWrap = true };

        foldoutStyle = new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold };
        drawInventoryItem = EditorGUILayout.Foldout(drawInventoryItem, "InventoryItem Data", foldoutStyle);
        if (!drawInventoryItem) return;
        EditorGUILayout.BeginVertical(contentStyle);
        SetItemID(EditorGUILayout.TextField("ItemID (clear to reset", itemID));
        SetDisplayName(EditorGUILayout.TextField("Display name", displayName));
        SetPrice(EditorGUILayout.Slider("Price", price, 0, 99999));
        EditorGUILayout.LabelField("Description");
        SetDescription(EditorGUILayout.TextArea(description, expandedAreaStyle));
        SetIcon((Sprite)EditorGUILayout.ObjectField("Icon", icon, typeof(Sprite), false));
        GameObject potentialPickup = pickup ? pickup.gameObject : null;
        SetPickup((GameObject)EditorGUILayout.ObjectField("Pickup", potentialPickup, typeof(GameObject), false));
        SetStackable(EditorGUILayout.Toggle("Stackable", stackable));
        EditorGUILayout.EndVertical();
    }

#endif
    #endregion

}
