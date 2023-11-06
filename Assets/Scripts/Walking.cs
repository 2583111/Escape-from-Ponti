using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Transform playerOrientation;
    public float gravity;

    private Rigidbody body;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }

    private void Update()
    {
        HandleMovementInput();
        ApplyCustomGravity();
        LimitVelocity();
    }

    private void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveInput = playerOrientation.forward * verticalInput + playerOrientation.right * horizontalInput;
        moveInput.Normalize();

        body.velocity = moveInput * walkSpeed;
    }

    private void ApplyCustomGravity()
    {
        Vector3 customGravityVector = Vector3.down * gravity;
        body.AddForce(customGravityVector, ForceMode.Acceleration);
    }

    private void LimitVelocity()
    {
        Vector3 velocity = body.velocity;
        velocity.y = 0; // Ensure the vertical velocity is not affected by limiting speed

        if (velocity.magnitude > walkSpeed)
        {
            // Clamp the horizontal velocity to the specified walkSpeed
            Vector3 horizontalVelocity = Vector3.ClampMagnitude(velocity, walkSpeed);
            body.velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
        }
    }
}
