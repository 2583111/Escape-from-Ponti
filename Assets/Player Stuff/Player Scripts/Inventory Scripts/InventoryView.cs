using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI destroyItemText;
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

    public PlayerCamera playerCamera;

    public GameObject closeButton;
    private ItemSlots _currentSlot;

    public int chappies = 20;
    public int medkit = 50;
    public int biscuts = 30;



    private void Start()
    {
        inventoryViewGO.SetActive(false);
        walking = GetComponent<Walking>();

        descriptionPanel.SetActive(false);
        itemNameText.ClearMesh();
        itemDescriptionText.ClearMesh();
        destroyItemText.ClearMesh();
        useItemText.ClearMesh();
        warningPanel.SetActive(false);

        // Find the PlayerCamera script in the scene
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        EventSystem.current.SetSelectedGameObject(closeButton);
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;

        inventoryViewGO.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        walking.enabled = true;
        ClearDescriptionPanel();
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
        _currentSlot = selectedSlots;

        if (_currentSlot.IsEmpty())
        {
            ClearDescriptionPanel();
            return;
        }

        itemNameText.SetText(_currentSlot.itemData.itemName);

        switch (_currentSlot.itemData.itemCategory)
        {
            case ItemData.ItemCategory.Consumable:
                UpdateConsumableDescription(_currentSlot.itemData);
                break;
            case ItemData.ItemCategory.KeyItem:
                UpdateKeyItemDescription(_currentSlot.itemData);
                break;
            case ItemData.ItemCategory.Flashlight:
                UpdateFlashlightDescription(_currentSlot.itemData);
                break;
            default:
                ClearDescriptionPanel();
                break;
        }

        destroyItemText.SetText("Press X to Destroy");
        descriptionPanel.SetActive(true);
    }

    private void UpdateConsumableDescription(ItemData itemData)
    {
        // Additional logic specific to Consumable items if needed
        useItemText.SetText("E to Use");

        // Update the description text based on the specific item
        switch (itemData.specificItem)
        {
            case ItemData.SpecificItem.SmallMedkit:
                itemDescriptionText.SetText("Heals the player a little bit");
                break;
            case ItemData.SpecificItem.Medkit:
                itemDescriptionText.SetText("Heals the player");
                break;
            case ItemData.SpecificItem.LargeMedkit:
                itemDescriptionText.SetText("Heals the player a lot");
                break;
            case ItemData.SpecificItem.SanityPills:
                itemDescriptionText.SetText("Restores sanity");
                break;
            case ItemData.SpecificItem.Battery:
                itemDescriptionText.SetText("Recharges flashlight");
                break;
            case ItemData.SpecificItem.Boosts:
                itemDescriptionText.SetText("Provides a temporary boost");
                break;
            // Add more cases as needed
            default:
                itemDescriptionText.SetText("Unknown Consumable");
                break;
        }
    }

    private void UpdateKeyItemDescription(ItemData itemData)
    {
        // Additional logic specific to KeyItem items if needed
        useItemText.SetText("This can't be used here");

        // Update the description text based on the specific item
        switch (itemData.specificItem)
        {
            // Add cases for specific key items if needed
            default:
                itemDescriptionText.SetText("Unknown KeyItem");
                break;
        }
    }

    private void UpdateFlashlightDescription(ItemData itemData)
    {
        // Additional logic specific to Flashlight items if needed
        useItemText.SetText("F to Use");
        itemDescriptionText.SetText("KeyItem");
    }

    private void ClearDescriptionPanel()
    {
        itemNameText.ClearMesh();
        itemDescriptionText.ClearMesh();
        destroyItemText.ClearMesh();
        useItemText.ClearMesh();
        descriptionPanel.SetActive(false);
    }

    public void UseItem()
    {
        if (_currentSlot == null)
        {
            Debug.LogWarning("No item slot selected.");
            return;
        }

        if (_currentSlot.IsEmpty())
        {
            Debug.LogWarning("Selected item slot is empty.");
            return;
        }

        ItemData itemData = _currentSlot.itemData;

        if (itemData == null)
        {
            Debug.LogError("Item data is null.");
            return;
        }

        if (itemData.itemCategory != ItemData.ItemCategory.Consumable)
        {
            Debug.LogWarning($"Cannot use item {itemData.itemName}. It's not a consumable.");
            return;
        }

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found.");
            return;
        }

        if (playerHealth.currentHealth >= playerHealth.maxHealth)
        {
            Debug.Log("Player is already at max health.");
            return;
        }

        int healingAmount = 0;

        switch (itemData.specificItem)
        {
            case ItemData.SpecificItem.SmallMedkit:
                healingAmount = chappies;
                break;

            case ItemData.SpecificItem.LargeMedkit:
                healingAmount = medkit;
                break;

            case ItemData.SpecificItem.Medkit:
                healingAmount = biscuts;
                break;

            case ItemData.SpecificItem.SanityPills:
                Sanity playerSanity = FindObjectOfType<Sanity>();

                if (playerSanity == null)
                {
                    Debug.LogError("Sanity script not found.");
                    return;
                }

                // Check if sanity is not already at maximum
                if (playerSanity.sanityBar.value < playerSanity.maxSanity)
                {
                    // Use Sanity Boost item
                    playerSanity.IncreaseSanity(5); // Adjust the sanity boost amount as needed
                }
                else
                {
                    // Optionally, you can provide feedback that the sanity is already at maximum.
                    Debug.Log("Sanity is already at maximum.");
                }
                break;

            case ItemData.SpecificItem.Battery:
                // Add logic for using Battery
                break;

            case ItemData.SpecificItem.Boosts:
                // Call the UseBoosts method in the BoostScript script
                break;

            case ItemData.SpecificItem.Ammo:
                // Add logic for using Ammo
                break;

            // Add more cases as needed

            default:
                Debug.LogError("Unhandled item type: " + itemData.specificItem);
                return;
        }

        // Use Medkit
        playerHealth.Heal(healingAmount); // Adjust the healing amount as needed

        // Update UI and clear/destroy the item data from the current slot
        _currentSlot.ClearSlot();

        Debug.Log($"Consumable item '{itemData.itemName}' used successfully.");
    }


    private bool IsItemInInventory(ItemData itemData)
    {
        // Iterate through the inventory slots to check if the item is present
        foreach (var slot in slotData)
        {
            if (!slot.IsEmpty() && slot.itemData == itemData)
            {
                return true;
            }
        }

        return false;
    }

    public void OnItemClicked(ItemSlots selectedSlot)
    {
        _currentSlot = selectedSlot;
        selectedSlot.OnSelect(null);

        // Other code for handling item clicks in the inventory
    }

    public void UseItemFromInventory()
    {
        if (_currentSlot != null && !_currentSlot.IsEmpty())
        {
            UseItem();
        }
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

        // Use the default sensitivity values from PlayerCamera
        if (playerCamera != null)
        {
            if (inventoryViewGO.activeSelf)
            {
                // Inventory is open, set sensitivity to 0
                playerCamera.sensitivityX = 0f;
                playerCamera.sensitivityY = 0f;
            }
            else
            {
                // Inventory is closed, set sensitivity back to normal
                playerCamera.sensitivityX = 15f;
                playerCamera.sensitivityY = 15f;
            }
        }

        if (inventoryViewGO.activeSelf)
        {
            // Check for 'E' key press when an item slot is selected
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_currentSlot != null && !_currentSlot.IsEmpty())
                {
                    UseItem();
                    OnSlotSelected(_currentSlot);
                }

            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (_currentSlot != null && !_currentSlot.IsEmpty())
                {
                    OnSlotSelected(_currentSlot);
                    _currentSlot.ClearSlot();
                    ClearDescriptionPanel();
                }
            }
        }

    }



}



