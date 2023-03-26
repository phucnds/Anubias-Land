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

    private static Inventory Instance;

    private void Awake() {
        Instance = this;
    }

    public static Inventory GetPlayerInventory()
    {
        return Instance;
    }
}
