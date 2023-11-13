using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject movePlatform;
    public float elevatorSpeed = 1.0f; // Adjust this value to control the elevator speed.
    public float delayTime = 2.0f; // Adjust this value to set the delay before the elevator starts moving.

    private bool canMove = false;

    private void OnTriggerStay(Collider other)
    {
        if (!canMove)
        {
            StartCoroutine(StartElevatorAfterDelay());
        }
        else
        {
            // Move the platform by multiplying the up vector with the elevator speed and Time.deltaTime.
            movePlatform.transform.position += movePlatform.transform.up * elevatorSpeed * Time.deltaTime;
        }
    }

    IEnumerator StartElevatorAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);
        canMove = true;
    }
}
