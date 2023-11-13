using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightEnabler : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private InventoryView inventoryViewScript;
    [SerializeField] private GameObject flashlightObject;

    private bool canPickUp = true;

    private void Start()
    {
        
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(false);
        }
    }

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
                // Pick up the item and enable the flashlightObject
                EventBus.Instance.PickUpItem(itemData);
                EnableFlashlight();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        canPickUp = true;
    }

    private void EnableFlashlight()
    {
        // Enable the flashlightObject if it's not already enabled
        if (flashlightObject != null && !flashlightObject.activeSelf)
        {
            flashlightObject.SetActive(true);
        }
    }
}
