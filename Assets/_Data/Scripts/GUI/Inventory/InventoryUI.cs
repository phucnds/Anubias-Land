﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be placed on the root of the inventory UI. Handles spawning all the
/// inventory slot prefabs.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    // CONFIG DATA
    [SerializeField] InventorySlotUI InventoryItemPrefab = null;

    // CACHE
    Inventory playerInventory;
    private void Awake()
    {
        playerInventory = GameMgr.Instance.Inventory;
        playerInventory.inventoryUpdated += Redraw;
    }

    private void Start()
    {
        Redraw();
    }

    private void Redraw()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < playerInventory.GetSize(); i++)
        {
            var itemUI = Instantiate(InventoryItemPrefab, transform);
            itemUI.Setup(playerInventory, i);
        }

    }
}