using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityX = 2f; // Sensitivity for horizontal mouse movement
    public float sensitivityY = 2f; // Sensitivity for vertical mouse movement

    public Transform playerOrientation; // Reference to the player's orientation

    private float rotationX; // Stores the rotation around the X-axis
    private float rotationY; // Stores the rotation around the Y-axis

    // Start is called before the first frame update
    void Start()
    {
        // Lock and hide the cursor at the beginning of the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX; 
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Update the rotation values
        rotationX -= mouseY; // Invert the mouseY input to match the usual camera controls
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Clamp the vertical rotation to avoid looking upside down

        rotationY += mouseX; // Horizontal rotation

        // Rotate the camera and player orientation
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        playerOrientation.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}
