using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashLight;
    public AudioSource turnOn;
    public AudioSource turnOff;

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
    }

    private void ToggleFlashlight()
    {
        isOn = !isOn;
        flashLight.SetActive(isOn);

        if (isOn)
        {
            turnOn.Play();
        }
        else
        {
            turnOff.Play();
        }
    }



}
