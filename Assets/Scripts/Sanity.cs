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
    public float audioThreshold = 0.5f; // Updated threshold
    public float vignetteStartIntensity = 0.65f;
    public float grainStartIntensity = 1.25f;
    public float ambientStartIntensity = 1.75f;
    public AudioSource sanityDecrease;

    private Vignette vignette;
    private Grain grain;
    private AmbientOcclusion ambient;

    private bool effectsEnabled = false;
    private bool audioHasPlayed = false; // Flag to track audio playback

    private void Start()
    {
        InitializeSanity();
        InitializePostProcessingEffects();
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

            // Check for audio playback when sanity drops below 50%
            if (currentSanityPercent <= 0.5f)
            {
                if (!audioHasPlayed)
                {
                    PlayAudio();
                    audioHasPlayed = true; // Set the flag to indicate that the audio has played
                }
            }
            else
            {
                audioHasPlayed = false; // Reset the flag when sanity goes above 50%
            }

            if (currentSanityPercent <= 0.25f)
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

    private void PlayAudio()
    {
        sanityDecrease.Play();
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
}
