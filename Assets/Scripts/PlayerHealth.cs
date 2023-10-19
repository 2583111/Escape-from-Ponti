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
    public AudioSource heavyBreathing;

    private bool isPlayingNormalHeartbeat = false;

    void Start()
    {
        currentHealth = maxHealth;
        HealthBar.SetMaxHealth(maxHealth);
        HealthBar.SetHealthBar(currentHealth);

        // Play the normal heartbeat sound
        normalHeartbeatSound.Play();

        // Start the audio check coroutine 
        StartCoroutine(CheckAudio(1f));
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
                heavyBreathing.Stop();
            }
            else if (currentHealth < 20 && isPlayingNormalHeartbeat)
            {
                normalHeartbeatSound.Stop();
                isPlayingNormalHeartbeat = false;
            }

            if (currentHealth >= 1 && currentHealth <= 19)
            {
                fasterHeartbeatSound.Play();
                heavyBreathing.Play();
            }
            else
            {
                fasterHeartbeatSound.Stop();
                heavyBreathing.Stop();
            }

            yield return new WaitForSeconds(interval); 
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

