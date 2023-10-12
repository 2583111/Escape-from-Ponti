using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolder : MonoBehaviour
{
    public Transform cameraPosition; // The target camera's position to follow

    // FixedUpdate is called at a fixed interval (physics update)
    void FixedUpdate()
    {
        // Ensure that this object's position follows the camera's position
        // This keeps the object in sync with the camera's movement
        transform.position = cameraPosition.position;
    }
}
