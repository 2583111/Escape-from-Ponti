using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Flashlight : MonoBehaviour
{
    public GameObject flashLight;
    public AudioSource turnOn;
    public AudioSource turnOff;
    //public TextMeshProUGUI text;
    //public TextMeshProUGUI batteryText;
    public float batterylife = 100;

    private bool isOn;

    public KeyCode flashlightKey = KeyCode.F; 

    private void Start()
    {
        isOn = false;
        flashLight.SetActive(false);
    }

    private void Update()
    {
        //text.text = batterylife.ToString("0") + "%";
        
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
