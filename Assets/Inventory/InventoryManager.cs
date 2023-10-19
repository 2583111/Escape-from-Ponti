using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<InventorySlot> inventorySlotsList = new List<InventorySlot>();

    public GameObject inventoryItemPrefab;
    public GameObject inventorySlotPrefab;

    public int maxStack = 5;

    public int balance = 1000;
    public TextMeshProUGUI balanceText;

    public static int listCount;


    public bool AddItem(Items item)
    {

        listCount = inventorySlotsList.Count;
        if (balance >= item.cost)
        {
            balanceText.text = balance.ToString();

            for (int i = 0; i < listCount; i++)
            {
                InventorySlot slot = inventorySlotsList[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot != null && itemInSlot.item == item && itemInSlot.Count < maxStack && item.stackable)
                {
                    itemInSlot.Count++;
                    itemInSlot.RefreshCount();
                    balance -= item.cost;
                    balanceText.text = balance.ToString();

                    return true;
                }
            }

            for (int i = 0; i < listCount; i++)
            {
                InventorySlot slot = inventorySlotsList[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    balance -= item.cost;
                    balanceText.text = balance.ToString();

                    return true;
                }
            }
        }
        return false;


    }

    public void SpawnNewItem(Items item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);
    }
}
