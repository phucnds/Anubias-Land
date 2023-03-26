﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// To be put on the icon representing an inventory item. Allows the slot to
/// update the icon and number.
/// </summary>
[RequireComponent(typeof(Image))]
public class InventoryItemIcon : MonoBehaviour
{
    // CONFIG DATA
    [SerializeField] GameObject textContainer = null;
    [SerializeField] TextMeshProUGUI itemNumber = null;

    // PUBLIC

    public void SetItem(InventoryItem item)
    {
        SetItem(item, 0);
    }

    public void SetItem(InventoryItem item, int number)
    {
        var iconImage = GetComponent<Image>();
        if (item == null)
        {
            iconImage.enabled = false;
            Debug.Log("item is null");
        }
        else
        {
            iconImage.enabled = true;
            iconImage.sprite = item.GetIcon();
            Debug.Log("item is not null");
        }

        if (itemNumber)
        {
            if (number <= 1)
            {
                textContainer.SetActive(false);
            }
            else
            {
                textContainer.SetActive(true);
                itemNumber.text = number.ToString();
            }
        }
    }
}