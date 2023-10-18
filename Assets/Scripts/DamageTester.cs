using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTester : MonoBehaviour
{
    public int damageAmount = 10; // Amount of damage the enemy deals
    private bool canDamage = true; // To prevent continuous damage in case of collision

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a player object
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            canDamage = false; // Prevent continuous damage for a brief moment

            // Get the player's Health component and apply damage
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }

            // Start a coroutine to reset canDamage after a delay
            StartCoroutine(ResetDamage());
        }
    }

    IEnumerator ResetDamage()
    {
        // Wait for a specified delay before allowing damage again
        yield return new WaitForSeconds(1.0f); // Adjust the delay time as needed
        canDamage = true; // Allow damage again
    }
}
