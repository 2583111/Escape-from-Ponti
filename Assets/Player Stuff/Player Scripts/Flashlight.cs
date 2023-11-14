using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Flashlight : MonoBehaviour
{
    public GameObject flashLight;
    public Light outerLight;  
    public Light innerLight;
    public AudioSource turnOn;
    public AudioSource turnOff;
    public float batteryLife = 100;
    public float batteryDrainRate = 1f;
    public float batteryFlickerThreshold = 20f;
    public float batteryCriticalThreshold = 1f;

    private bool isOn;

    public KeyCode flashlightKey = KeyCode.F;

    private void Start()
    {
        isOn = false;
        flashLight.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(flashlightKey))
        {
            ToggleFlashlight();
        }

        if (isOn)
        {
            DrainBattery();
            CheckBatteryStatus();
        }
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;

        if (isOn)
        {
            turnOn.Play();
        }
        else
        {
            turnOff.Play();
        }

        flashLight.SetActive(isOn);
    }

    private void DrainBattery()
    {
        batteryLife -= batteryDrainRate * Time.deltaTime;
        batteryLife = Mathf.Clamp(batteryLife, 0, 100);

        if (batteryLife <= 0)
        {
            isOn = false;
            flashLight.SetActive(false);
        }
    }

    private void CheckBatteryStatus()
    {
        if (batteryLife <= batteryCriticalThreshold)
        {
            float decreaseSpeed = Time.deltaTime * 2; // Adjust the decrease speed if needed
            outerLight.intensity = Mathf.Lerp(outerLight.intensity, 0.01f, decreaseSpeed);
            innerLight.intensity = Mathf.Lerp(innerLight.intensity, 0.01f, decreaseSpeed);
        }
        else if (batteryLife <= 50)
        {
            float targetIntensity = Mathf.Lerp(0.01f, outerLight.intensity, batteryLife / 50f);
            outerLight.intensity = Mathf.Lerp(outerLight.intensity, targetIntensity, Time.deltaTime * 0.09f); // Adjust the decrease rate if needed
            float innerTargetIntensity = Mathf.Lerp(0.01f, innerLight.intensity, batteryLife / 50f);
            innerLight.intensity = Mathf.Lerp(innerLight.intensity, innerTargetIntensity, Time.deltaTime * 0.1f); // Adjust the decrease rate if needed
        }

        if (batteryLife <= batteryFlickerThreshold)
        {
            StartCoroutine(FlickerLight());
        }
    }

    private IEnumerator FlickerLight()
    {
        while (batteryLife <= batteryFlickerThreshold)
        {
            float flickerInterval = Random.Range(0.1f, 0.5f);
            flashLight.SetActive(!flashLight.activeSelf);
            yield return new WaitForSeconds(flickerInterval);
        }
    }



}
