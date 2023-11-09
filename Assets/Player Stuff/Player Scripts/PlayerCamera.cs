using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivityX;
    public float sensitivityY;
    public Transform playerOrientation;

    private float rotationX;
    private float rotationY;

    public GameObject inventory; // Reference to the Inventory game object

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Find the Inventory game object by its tag
        inventory = GameObject.FindGameObjectWithTag("Inventory");

        if (inventory == null)
        {
            Debug.LogWarning("Inventory game object not found!");
        }
    }

    private void Update()
    {
        // Check if the Inventory game object is active (enabled)
        bool isInventoryActive = inventory != null && inventory.activeSelf;

        // Adjust sensitivity based on the Inventory state
        if (isInventoryActive)
        {
            sensitivityX = 0f; // Set to 0 when Inventory is active
            sensitivityY = 0f; // Set to 0 when Inventory is active
        }
        else
        {
            sensitivityX = 2f; // Set to the normal value when Inventory is not active
            sensitivityY = 2f; // Set to the normal value when Inventory is not active
        }

        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        rotationY += mouseX;

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        playerOrientation.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}

