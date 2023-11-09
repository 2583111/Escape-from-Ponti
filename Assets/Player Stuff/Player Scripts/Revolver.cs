using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolver : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootForce = 10f;
    public Transform playerTransform;
    public Transform playerCam;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 startPosition = playerTransform.transform.position + playerTransform.transform.forward;

        GameObject newProjectile = Instantiate(projectilePrefab, startPosition, playerTransform.transform.rotation);

        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = playerCam.transform.forward * shootForce;
        }
    }
}
