using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    private Animator fenceAnimator;

    private void Start()
    {
        fenceAnimator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Set the "Collided" parameter to trigger the transition
            fenceAnimator.SetBool("Collided", true);
        }
    }
}
