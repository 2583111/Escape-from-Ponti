using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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
    public bool isInventoryOpen = false;
    private bool isBoostActive = false;
    private bool isBoostEnabled = false;

    public PlayerCamera playerCamera;

    public GameObject closeButton;
    private ItemSlots _currentSlot;
    public GameObject BoostUI;
    public GunData gunData;
    public TextMeshProUGUI ammoCountText;

    public int chappies = 20;
    public int medkit = 50;
    public int biscuits = 30;
    public int sanityPills = 5;

    private int smallMedkitHealingAmount = 20;
    private int medkitHealingAmount = 50;
    private int largeMedkitHealingAmount = 30;
    private int sanityPillsAmount = 5;

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

        BoostUI.SetActive(false);
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
            case ItemData.SpecificItem.Ammo:
                itemDescriptionText.SetText("Ammo can only be used when gun ammo is empty");
                break;
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

        // Perform specific checks based on the item type
        switch (itemData.specificItem)
        {
            case ItemData.SpecificItem.SmallMedkit:
                if (CanUseSmallMedkit())
                {
                    UseMedkit(chappies, smallMedkitHealingAmount);
                    _currentSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Cannot use Small Medkit. Player is already at max health.");
                }
                break;

            case ItemData.SpecificItem.LargeMedkit:
                if (CanUseLargeMedkit())
                {
                    UseMedkit(medkit, largeMedkitHealingAmount);
                    _currentSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Cannot use Large Medkit. Player is already at max health.");
                }
                break;

            case ItemData.SpecificItem.Medkit:
                if (CanUseMedkit())
                {
                    UseMedkit(biscuits, medkitHealingAmount);
                    _currentSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Cannot use Medkit. Player is already at max health.");
                }
                break;

            case ItemData.SpecificItem.SanityPills:
                if (CanUseSanityPills())
                {
                    UseSanityPills(sanityPills, sanityPillsAmount);
                    _currentSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Cannot use Sanity Pills. Player's sanity is already at max.");
                }
                break;

            case ItemData.SpecificItem.Boosts:
                if (CanUseBoost())
                {
                    UseBoost();
                    _currentSlot.ClearSlot();
                }
                else
                {
                    Debug.LogWarning("Cannot use Boost. Boost is already active. Wait for the current boost to expire.");
                }
                break;

            case ItemData.SpecificItem.Ammo:
                if (gunData.currentAmmo == 0)
                {
                    ReloadGun();
                    _currentSlot.ClearSlot();
                    UpdateAmmoText(gunData.currentAmmo); // Update ammo count UI
                    Debug.Log("Gun reloaded using Ammo item!");
                }
                else
                {
                    Debug.Log("Ammo item can only be used when gun ammo is empty.");
                }
                break;

            // Add more cases as needed

            default:
                Debug.LogError("Unhandled item type: " + itemData.specificItem);
                return;
        }

        Debug.Log($"Consumable item '{itemData.itemName}' used successfully.");
    }

    private bool CanUseSmallMedkit()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        return playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth;
    }

    private bool CanUseLargeMedkit()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        return playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth;
    }

    private bool CanUseMedkit()
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        return playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth;
    }

    private bool CanUseSanityPills()
    {
        Sanity playerSanity = FindObjectOfType<Sanity>();
        return playerSanity != null && playerSanity.currentSanity < 90;
    }

    private bool CanUseBoost()
    {
        
        if (BoostUI != null)
        {
            // Ensure the BoostMeter script is attached to the BoostUI GameObject
            BoostMeter boostMeter = BoostUI.GetComponent<BoostMeter>();
            if (boostMeter != null && !isBoostActive)
            {
                return boostMeter.GetCurrentBoostValue() > 0;
            }
            else
            {
                Debug.LogError("BoostMeter script not found on BoostUI GameObject.");
                return false;
            }
        }
        else
        {
            Debug.LogError("BoostUI GameObject not set in the Unity Inspector.");
            return false;
        }
    }

    private void UseMedkit(int amount, int healingAmount)
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth script not found.");
            return;
        }

        if (playerHealth.currentHealth < playerHealth.maxHealth)
        {
            // Use Medkit only if the player is not already at max health
            playerHealth.Heal(healingAmount);
        }
        else
        {
            Debug.Log("Player is already at max health.");
        }
    }

    private void UseSanityPills(int amount, int sanityAmount)
    {
        Sanity playerSanity = FindObjectOfType<Sanity>();
        if (playerSanity == null)
        {
            Debug.LogError("Sanity script not found.");
            return;
        }

        if (playerSanity.currentSanity < playerSanity.maxSanity)
        {
            // Use Sanity Pills only if the player's sanity is not already at max
            playerSanity.IncreaseSanity(sanityAmount);
        }
        else
        {
            Debug.Log("Player's sanity is already at max.");
        }
    }

    private void UseBoost()
    {
        // Ensure the BoostUI GameObject is set in the Unity Inspector
        if (BoostUI != null)
        {
            // Ensure the BoostMeter script is attached to the BoostUI GameObject
            BoostMeter boostMeter = BoostUI.GetComponent<BoostMeter>();
            if (boostMeter != null)
            {
                // Check if a boost is already active
                if (!isBoostActive)
                {
                    // Enable the BoostUI GameObject and BoostMeter script
                    BoostUI.SetActive(true);
                    boostMeter.EnableBoostEffect();
                    isBoostActive = true;
                }
                else
                {
                    Debug.Log("Boost is already active. Wait for the current boost to expire.");
                }
            }
            else
            {
                Debug.LogError("BoostMeter script not found on BoostUI GameObject.");
            }
        }
        else
        {
            Debug.LogError("BoostUI GameObject not set in the Unity Inspector.");
        }
    }

    private void ReloadGun()
    {
        if (gunData.currentAmmo <= 0)
        {
            gunData.currentAmmo = gunData.magSize;
            UpdateAmmoText(gunData.currentAmmo); // Provide ammoCount to UpdateAmmoText
        }
        else
        {
            Debug.Log("Gun is not empty. Cannot reload.");
        }
    }

    public void UpdateAmmoText(int ammoCount)
    {
        // Update the UI text with the provided ammo count
        if (ammoCountText != null)
        {
            ammoCountText.text = ammoCount.ToString();
        }
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


    private void EnableBoost()
    {
        // Ensure the BoostUI GameObject is set in the Unity Inspector
        if (BoostUI != null)
        {
            // Enable the BoostUI GameObject
            BoostUI.SetActive(true);

            // Ensure the BoostMeter script is attached to the BoostUI GameObject
            BoostMeter boostMeter = BoostUI.GetComponent<BoostMeter>();
            if (boostMeter != null)
            {
                // Enable the Boost functionality in the BoostMeter script
                boostMeter.EnableBoostEffect();
                isBoostActive = true;
            }
            else
            {
                Debug.LogError("BoostMeter script not found on BoostUI GameObject.");
            }
        }
        else
        {
            Debug.LogError("BoostUI GameObject not set in the Unity Inspector.");
        }
    }

    public void DisableBoost()
    {
        // Ensure the BoostUI GameObject is set in the Unity Inspector
        if (BoostUI != null)
        {
            // Disable the BoostUI GameObject
            BoostUI.SetActive(false);

            // Ensure the BoostMeter script is attached to the BoostUI GameObject
            BoostMeter boostMeter = BoostUI.GetComponent<BoostMeter>();
            if (boostMeter != null)
            {
                // No need for DisableBoost in BoostMeter
                isBoostActive = false;
            }
            else
            {
                Debug.LogError("BoostMeter script not found on BoostUI GameObject.");
            }
        }
        else
        {
            Debug.LogError("BoostUI GameObject not set in the Unity Inspector.");
        }
    }



}



