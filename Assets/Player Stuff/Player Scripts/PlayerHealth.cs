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

    private Coroutine heavyBreathingCoroutine;

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
                StopHeavyBreathing();
            }
            else if (currentHealth < 20 && isPlayingNormalHeartbeat)
            {
                normalHeartbeatSound.Stop();
                isPlayingNormalHeartbeat = false;
            }

            if (currentHealth >= 1 && currentHealth <= 19)
            {
                fasterHeartbeatSound.Play();
                PlayHeavyBreathing();
            }
            else
            {
                fasterHeartbeatSound.Stop();
                StopHeavyBreathing();
            }

            yield return new WaitForSeconds(interval);
        }
    }

    private void PlayHeavyBreathing()
    {
        if (heavyBreathingCoroutine == null)
        {
            heavyBreathing.Play();
            heavyBreathingCoroutine = StartCoroutine(StopHeavyBreathingAfter(20.0f));
        }
    }

    private void StopHeavyBreathing()
    {
        if (heavyBreathingCoroutine != null)
        {
            StopCoroutine(heavyBreathingCoroutine);
            heavyBreathing.Stop();
            heavyBreathingCoroutine = null;
        }
    }

    private IEnumerator StopHeavyBreathingAfter(float duration)
    {
        yield return new WaitForSeconds(duration);
        heavyBreathing.Stop();
        heavyBreathingCoroutine = null;
    }

    public void TakeDamage(int damage)
    {
        int targetHealth = Mathf.Max(currentHealth - damage, 0);

        if (targetHealth < currentHealth)
        {
            StartCoroutine(DecreaseHealthSmoothly(targetHealth));
        }
    }

    public void Heal(int healingAmount)
    {
        if (currentHealth >= maxHealth)
        {
            Debug.Log("Player is already at max health.");
            return;
        }

        currentHealth = Mathf.Min(currentHealth + healingAmount, maxHealth);

        HealthBar.SetHealthBar(currentHealth);

        Debug.Log($"Player healed by: {healingAmount}. Current health: {currentHealth}");
    }



    IEnumerator IncreaseHealthSmoothly(int targetHealth)
    {
        while (currentHealth < targetHealth)
        {
            currentHealth = (int)Mathf.MoveTowards(currentHealth, targetHealth, healthDecreaseSpeed * Time.deltaTime);
            HealthBar.SetHealthBar(currentHealth);
            yield return null;
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

