using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostMeter : MonoBehaviour
{
    private float originalWalkSpeed;

    public Slider BoostBar;
    public float decreaseRate = 1f;
    public Walking walkSpeed;
    public float boostSPEED = 10f;
    public InventoryView inventoryView;

    private void Start()
    {
        // Set initial BoostBar value
        BoostBar.value = 100f;
    }

    public void EnableBoostEffect()
    {
        // Store the original walk speed before applying the boost
        originalWalkSpeed = walkSpeed.walkSpeed;

        StartCoroutine(ContinuousDecrease());
    }

    public IEnumerator ContinuousDecrease()
    {
        while (BoostBar.value > 0)
        {
            DecreaseSlider(decreaseRate);

            if (walkSpeed != null)
            {
                // Increase move speed while the slider is decreasing
                walkSpeed.UpdateWalkSpeed(boostSPEED);
            }

            yield return null;
        }

        // BoostBar value is 0, disable BoostUI and BoostMeter
        if (inventoryView != null)
        {
            // Reset walk speed to the original value
            walkSpeed.UpdateWalkSpeed(originalWalkSpeed);

            // Reset the BoostBar value to 100
            BoostBar.value = 100f;

            // Call the DisableBoost method from InventoryView
            inventoryView.DisableBoost();
        }
    }

    private void DecreaseSlider(float amount)
    {
        BoostBar.value -= amount;
        BoostBar.value = Mathf.Max(BoostBar.value, 0);
    }

    public float GetCurrentBoostValue()
    {
        return BoostBar.value;
    }
}