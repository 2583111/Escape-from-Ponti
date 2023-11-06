using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public InventoryRemake Inventory;
    public GameObject inventoryUIObject; // Reference to the Inventory UI GameObject.

    private void Start()
    {
        Inventory.ItemAdded += InventoryScript_ItemAdded;
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        if (inventoryUIObject != null)
        {
            Transform inventoryPanel = inventoryUIObject.transform;

            foreach (Transform slot in inventoryPanel)
            {
                Image image = slot.GetChild(0).GetChild(0).GetComponent<Image>();

                if (!image.enabled)
                {
                    image.enabled = true;
                    image.sprite = e.Item.Image;
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Inventory UI GameObject reference is missing.");
        }
    }
}
