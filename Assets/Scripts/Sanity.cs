using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;



public class Sanity : MonoBehaviour
{
    public Slider sanityBar;
    public int maxSanity;
    public PostProcessProfile profile;
    public float decreaseRate = 2f;
    public float audioThreshold = 0.5f;
    public float vignetteStartIntensity = 0.65f;
    public float grainStartIntensity = 1.25f;
    public float ambientStartIntensity = 1.75f;
    public AudioSource sanityDecrease;
    public AudioSource whispers;

    private Vignette vignette;
    private Grain grain;
    private AmbientOcclusion ambient;

    private bool effectsEnabled = false;
    private bool audioHasPlayed = false;
    private bool whispersAudioPlayed = false;
    private float whispersVolume = 0.0f;

    private void Start()
    {
        InitializeSanity();
        InitializePostProcessingEffects();
        whispers.volume = 0.0f;
        StartCoroutine(ContinuousSanityUpdate());
    }

    private void InitializeSanity()
    {
        sanityBar.maxValue = maxSanity;
        sanityBar.value = maxSanity;
    }

    private void InitializePostProcessingEffects()
    {
        profile.TryGetSettings(out vignette);
        profile.TryGetSettings(out grain);
        profile.TryGetSettings(out ambient);
        SetPostProcessingEffects(vignetteStartIntensity, grainStartIntensity, ambientStartIntensity);
    }

    private IEnumerator ContinuousSanityUpdate()
    {
        while (true)
        {
            DecreaseSanity(decreaseRate);
            float currentSanityPercent = sanityBar.value / maxSanity;

            if (currentSanityPercent <= 0.25f) //Adjust for when sanity break audio plays
            {
                if (!audioHasPlayed)
                {
                    PlayAudio(sanityDecrease);
                    audioHasPlayed = true;
                }
            }
            else
            {
                audioHasPlayed = false;
            }

            if (currentSanityPercent <= 0.20f) //Adjust for when whispers audio plays
            {
                if (!whispersAudioPlayed)
                {
                    whispersAudioPlayed = true;
                    StartCoroutine(PlayAndIncreaseVolume(whispers, 0.05f)); //The final volume
                }
            }
            else if (currentSanityPercent > 0.25f)
            {
                whispersAudioPlayed = false;
                StartCoroutine(DecreaseWhispersVolume(whispers, 0f)); //The starting volume
            }

            if (currentSanityPercent <= 0.25f) //Adjust for when post processing effects happen
            {
                effectsEnabled = true;
            }

            UpdatePostProcessingEffects(currentSanityPercent);

            yield return null;
        }
    }

    private void DecreaseSanity(float amount)
    {
        sanityBar.value -= amount;
        sanityBar.value = Mathf.Max(sanityBar.value, 0);
    }

    private void PlayAudio(AudioSource audioSource)
    {
        audioSource.Play();
    }

    private void UpdatePostProcessingEffects(float currentSanityPercent)
    {
        float vignetteIntensity = Mathf.Lerp(vignetteStartIntensity, 0f, currentSanityPercent * 4);
        float grainIntensity = Mathf.Lerp(grainStartIntensity, 0f, currentSanityPercent * 4);
        float ambientIntensity = Mathf.Lerp(ambientStartIntensity, 0f, currentSanityPercent * 4);

        SetPostProcessingEffects(vignetteIntensity, grainIntensity, ambientIntensity);
    }

    private void SetPostProcessingEffects(float vignetteIntensity, float grainIntensity, float ambientIntensity)
    {
        vignette.intensity.value = vignetteIntensity;
        grain.intensity.value = grainIntensity;
        ambient.intensity.value = ambientIntensity;
    }

    private IEnumerator PlayAndIncreaseVolume(AudioSource audioSource, float targetVolume)
    {
        audioSource.Play();

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += 0.01f; // Adjust the rate at which the volume increases
            yield return new WaitForSeconds(0.05f); // Adjust the interval between volume increases
        }
    }

    private IEnumerator DecreaseWhispersVolume(AudioSource audioSource, float targetVolume)
    {
        while (audioSource.volume > targetVolume)
        {
            audioSource.volume -= 0.01f; // Adjust the rate at which the volume decreases
            yield return new WaitForSeconds(0.1f); // Adjust the interval between volume decreases
        }
    }
}
