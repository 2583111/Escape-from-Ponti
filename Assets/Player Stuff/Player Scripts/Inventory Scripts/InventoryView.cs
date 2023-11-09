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
    public GameObject warningPanel;
    Walking walking;

    public KeyCode openInventoryKey = KeyCode.I;
    private KeyCode closeInventoryKey = KeyCode.Escape;

    public List<ItemSlots> slotData;

    public static bool warningPanelActive = false;
    private bool showingWarningPanel = false;

    // Flag to track the inventory state
    public bool isInventoryOpen = false;

    private void Start()
    {
        inventoryViewGO.SetActive(false);
        walking = GetComponent<Walking>();

        descriptionPanel.SetActive(false);
        itemNameText.ClearMesh();
        itemDescriptionText.ClearMesh();
        dropItemText.ClearMesh();
        useItemText.ClearMesh();
        warningPanel.SetActive(false);
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        // Additional code to handle opening the inventory
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        // Additional code to handle closing the inventory
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
            warningPanelActive = false;
        }
    }

    public void ShowWarningPanel()
    {
        warningPanel.SetActive(true);
        showingWarningPanel = true;
        StartCoroutine(DisableWarningPanel());
    }

    public void HideWarningPanel()
    {
        warningPanel.SetActive(false);
        showingWarningPanel = false;
    }

    private IEnumerator DisableWarningPanel()
    {
        yield return new WaitForSeconds(5.0f);
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
        descriptionPanel.SetActive(true);
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
                CloseInventory(); // Call to close the inventory when 'I' is pressed
            }
            else
            {
                inventoryViewGO.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                walking.enabled = false;
                OpenInventory(); // Call to open the inventory when 'I' is pressed
            }
        }

        if (Input.GetKeyDown(closeInventoryKey) && inventoryViewGO.activeSelf)
        {
            inventoryViewGO.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            walking.enabled = true;
            CloseInventory(); // Call to close the inventory when 'Escape' is pressed
        }

        if (showingWarningPanel && Input.GetKeyDown(KeyCode.E))
        {
            HideWarningPanel();
        }
    }
}





