using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Health HealthBar;
    public float healthDecreaseSpeed = 5.0f;

    public AudioSource normalHeartbeatSound;
    public AudioSource fasterHeartbeatSound;

    private bool isPlayingNormalHeartbeat = false;

    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.SetMaxHealth(maxHealth);
        HealthBar.SetHealthBar(currentHealth);

        // Play the normal heartbeat sound on awake
        normalHeartbeatSound.Play();

        // Start the audio check coroutine with adjustable intervals
        StartCoroutine(CheckAudio(1.0f)); // Adjust the interval as needed
    }

    IEnumerator CheckAudio(float interval)
    {
        while (true)
        {
            if (currentHealth >= 20 && !isPlayingNormalHeartbeat)
            {
                normalHeartbeatSound.Play();
                isPlayingNormalHeartbeat = true;
                fasterHeartbeatSound.Stop();
            }
            else if (currentHealth < 20 && isPlayingNormalHeartbeat)
            {
                normalHeartbeatSound.Stop();
                isPlayingNormalHeartbeat = false;
            }

            if (currentHealth >= 1 && currentHealth <= 19)
            {
                fasterHeartbeatSound.Play();
            }
            else
            {
                fasterHeartbeatSound.Stop();
            }

            yield return new WaitForSeconds(interval); // Adjust the interval as needed
        }
    }

    public void TakeDamage(int damage)
    {
        int targetHealth = Mathf.Max(currentHealth - damage, 0);

        if (targetHealth < currentHealth)
        {
            StartCoroutine(DecreaseHealthSmoothly(targetHealth));
        }
    }

    IEnumerator DecreaseHealthSmoothly(int targetHealth)
    {
        while (currentHealth > targetHealth)
        {
            currentHealth = (int)Mathf.MoveTowards(currentHealth, targetHealth, healthDecreaseSpeed * Time.deltaTime);
            HealthBar.SetHealthBar(currentHealth);
            yield return null;
        }
    }
}

