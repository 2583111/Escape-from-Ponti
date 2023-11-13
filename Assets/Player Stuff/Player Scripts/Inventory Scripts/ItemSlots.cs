using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class ItemSlots : MonoBehaviour, ISelectHandler
{
     public ItemData itemData;

    private InventoryView viewController;
    private Image displaySprite;

    public void OnSelect(BaseEventData eventData)
    {
        viewController.OnSlotSelected(this);
    }

    private void OnEnable()
    {
        viewController = FindObjectOfType<InventoryView>();

        if (itemData != null)
        {
            DisplayItemSprite();
        }
    }

    private void OnDisable()
    {
        ClearDisplaySprite();
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }

    public void UseItem()
    {
        if (!IsEmpty())
        {
            // Implement the logic for using the item here

            // Clear the item from the slot immediately after use
            ClearSlot();
        }
        else
        {
            Debug.LogWarning("Selected item slot is empty.");
        }
    }

    private void DisplayItemSprite()
    {
        if (displaySprite == null && itemData.itemSprite != null)
        {
            displaySprite = Instantiate(itemData.itemSprite, transform.position, Quaternion.identity, transform);
        }
    }

    private void ClearDisplaySprite()
    {
        if (displaySprite != null)
        {
            Destroy(displaySprite.gameObject);
            displaySprite = null;
        }
    }

    public void ClearSlot()
    {
        ClearDisplaySprite();
        itemData = null;
    }
}
