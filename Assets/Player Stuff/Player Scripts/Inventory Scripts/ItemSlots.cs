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


    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        viewController.OnSlotSelected(this);
    }


    private void OnEnable()
    {
        viewController = FindObjectOfType<InventoryView>();

        if (itemData == null)
        {
            return;
        }

        displaySprite = Instantiate<Image>(itemData.itemSprite, transform.position, Quaternion.identity, transform);
    }

    private void OnDisable()
    {
        if (displaySprite != null)
        {
            Destroy(displaySprite);
        }
    }


    public bool IsEmpty()
    {
        return itemData == null;
    }

}
