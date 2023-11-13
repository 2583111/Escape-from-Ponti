using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostMeter : MonoBehaviour
{
    public Slider BoostBar;
    public float decreaseRate = 1f;
    public Walking walkSpeed;
    public float boostSPEED = 10f;

    private void Start()
    {
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
    }

    private void DecreaseSlider(float amount)
    {
        BoostBar.value -= amount;
        BoostBar.value = Mathf.Max(BoostBar.value, 0);
    }
}
