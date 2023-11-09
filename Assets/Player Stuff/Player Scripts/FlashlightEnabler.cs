using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightEnabler : MonoBehaviour
{
    public GameObject flashlightObject;

    private bool isEnabled = false;

    private void Start()
    {
        // Ensure the flashlightObject is initially disabled
        if (flashlightObject != null)
        {
            flashlightObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            // Enable the flashlightObject if it's not already enabled
            if (flashlightObject != null && !isEnabled)
            {
                flashlightObject.SetActive(true);
                isEnabled = true;
            }

            
            Destroy(gameObject);
        }
    }
}
