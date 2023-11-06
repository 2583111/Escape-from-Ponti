using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InventoryView : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI dropItemText;
    public TextMeshProUGUI useItemText;
    public GameObject inventoryViewGO;
    public GameObject descriptionPanel;
    public GameObject warningPanel; // Renamed to 'warningPanel'
    Walking walking;

    public KeyCode openInventoryKey = KeyCode.I; // Key to open/close inventory
    private KeyCode closeInventoryKey = KeyCode.Escape; // Key to close inventory

    public List<ItemSlots> slotData;

    public static bool warningPanelActive = false;
    private bool showingWarningPanel = false;

    private void Start()
    {
        inventoryViewGO.SetActive(false);
        walking = GetComponent<Walking>();

        // Disable the descriptionPanel and clear text when the game starts
        descriptionPanel.SetActive(false);
        itemNameText.ClearMesh();
        itemDescriptionText.ClearMesh();
        dropItemText.ClearMesh();
        useItemText.ClearMesh();
        warningPanel.SetActive(false); // Renamed to 'warningPanel'
    }

    private void OnEnable()
    {
        EventBus.Instance.onPickUpItem += OnItemPickedUp;
    }

    public void OnDisable()
    {
        EventBus.Instance.onPickUpItem -= OnItemPickedUp;
    }

    public void OnItemPickedUp(ItemData itemData)
    {
        int filledSlots = 0;

        foreach (var slot in slotData)
        {
            if (!slot.IsEmpty())
            {
                filledSlots++;
            }
            if (slot.IsEmpty())
            {
                slot.itemData = itemData;
                break;
            }
        }

        if (filledSlots >= 11)
        {
            warningPanelActive = true;
            ShowWarningPanel();
        }
        else
        {
            warningPanelActive = false; // Reset the flag if the panel is no longer active
        }
    }

    public void ShowWarningPanel() // Renamed the method to 'ShowWarningPanel'
    {
        warningPanel.SetActive(true);
        showingWarningPanel = true;
        StartCoroutine(DisableWarningPanel());
    }

    public void HideWarningPanel() // Added method to hide the warning panel
    {
        warningPanel.SetActive(false);
        showingWarningPanel = false;
    }

    private IEnumerator DisableWarningPanel() // Renamed the method to 'DisableWarningPanel'
    {
        yield return new WaitForSeconds(5.0f); // Wait for 2 seconds
        warningPanel.SetActive(false);
    }

    public void OnSlotSelected(ItemSlots selectedSlots)
    {
        if (selectedSlots.itemData == null)
        {
            itemNameText.ClearMesh();
            itemDescriptionText.ClearMesh();
            dropItemText.ClearMesh();
            useItemText.ClearMesh();
            descriptionPanel.SetActive(false);
            return;
        }

        itemNameText.SetText(selectedSlots.itemData.itemName);

        switch (selectedSlots.itemData.itemCategory)
        {
            case ItemData.ItemCategory.Consumable:
                itemDescriptionText.SetText(selectedSlots.itemData.itemCategory.ToString());
                useItemText.SetText("E to Use");
                break;
            case ItemData.ItemCategory.KeyItem:
                itemDescriptionText.SetText(selectedSlots.itemData.itemCategory.ToString());
                useItemText.SetText("This can't be used here");
                break;
            case ItemData.ItemCategory.Flashlight:
                itemDescriptionText.SetText("KeyItem");
                useItemText.SetText("F to Use");
                break;
        }

        dropItemText.SetText("Press G to Drop");
        descriptionPanel.SetActive(true); // Enable descriptionPanel when the slot is not empty
    }

    private void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
        {
            if (inventoryViewGO.activeSelf)
            {
                inventoryViewGO.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                walking.enabled = true;
            }
            else
            {
                inventoryViewGO.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                walking.enabled = false;
            }
        }

        if (Input.GetKeyDown(closeInventoryKey) && inventoryViewGO.activeSelf)
        {
            inventoryViewGO.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            walking.enabled = true;
        }

        if (showingWarningPanel && Input.GetKeyDown(KeyCode.E))
        {
            // If the "E" key is pressed while the warningPanel is shown, hide it.
            HideWarningPanel();
        }
    }
}





