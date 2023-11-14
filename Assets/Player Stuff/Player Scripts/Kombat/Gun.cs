using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] GunData gunData;
    public Transform Muzzle;
    public ParticleSystem muzzleFlash; // Assign the muzzle flash particle system in the Inspector
    public AudioSource shootingAudioSource; // Assign the audio source in the Inspector
    float timeSinceLastShot;
    public TextMeshProUGUI ammoCount;

    private bool canShoot = true; // Flag to control shooting

    private bool CanShoot()
    {
        GameObject inventoryPanel = GameObject.Find("inventoryPanel");
        if (inventoryPanel != null && inventoryPanel.activeSelf)
        {
            return false;
        }
        return gunData.currentAmmo > 0 && !gunData.Reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);
    }

    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        UpdateAmmoText();
        if (muzzleFlash != null)
        {
            muzzleFlash.gameObject.SetActive(false); // Disable the muzzle flash at the start
        }
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            if (gunData.currentAmmo > 0)
            {
                if (Physics.Raycast(Muzzle.position, Muzzle.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamageble damageble = hitInfo.transform.GetComponent<IDamageble>();
                    damageble?.TakeDamage(gunData.damage);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                UpdateAmmoText();

                if (gunData.currentAmmo == 0)
                {
                    canShoot = false; // Disable shooting when ammo is zero
                }

                // Display muzzle flash
                if (muzzleFlash != null)
                {
                    muzzleFlash.gameObject.SetActive(true);
                    muzzleFlash.Play();
                    StartCoroutine(DisableMuzzleFlash());
                }

                // Play shooting sound effect
                if (shootingAudioSource != null)
                {
                    shootingAudioSource.Play();
                }
            }
        }
    }

    private IEnumerator DisableMuzzleFlash()
    {
        yield return new WaitForSeconds(0.1f); // Adjust this duration to match the muzzle flash duration
        muzzleFlash.gameObject.SetActive(false);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShoot()
    {
        // Additional effects or actions on shooting
    }

    private void UpdateAmmoText()
    {
        if (ammoCount != null)
        {
            ammoCount.text = gunData.currentAmmo.ToString();
        }
    }


}
