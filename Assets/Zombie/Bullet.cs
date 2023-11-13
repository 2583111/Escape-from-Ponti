using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 20;

    // OnTriggerEnter is called when the Collider other enters the trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has a Zombie component.
        Zombie zombie = other.GetComponent<Zombie>();

        // If it's a zombie, apply damage.
        if (zombie != null)
        {
            zombie.TakeDamage(damageAmount);
        }

        // Destroy the bullet when it hits something.
        Destroy(gameObject);
    }
}