using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject movePlatform;
    public float elevatorSpeed = 1.0f; // Adjust this value to control the elevator speed.

    private void OnTriggerStay(Collider other)
    {
        // Move the platform by multiplying the up vector with the elevator speed and Time.deltaTime.
        movePlatform.transform.position += movePlatform.transform.up * elevatorSpeed * Time.deltaTime;
    }
}
