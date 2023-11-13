using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private InventoryView inventoryViewScript;

    private bool canPickUp = true;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") || !canPickUp) return;

        if (InventoryView.warningPanelActive)
        {
            // Panel is active, prevent pickup and show the warning panel
            Debug.Log("Slot is full. Cannot pick up the item.");
            inventoryViewScript.ShowWarningPanel();
        }
        else if (Input.GetKeyDown(KeyCode.E) && !inventoryViewScript.isInventoryOpen)
        {
            if (inventoryViewScript != null)
            {
                EventBus.Instance.PickUpItem(itemData);
                gameObject.SetActive(false); // Disable the GameObject
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        canPickUp = true;
    }
}
