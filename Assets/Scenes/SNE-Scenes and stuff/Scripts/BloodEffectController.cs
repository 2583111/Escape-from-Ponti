using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodEffectController : MonoBehaviour
{
    [Header("Player Health")]
    public float currentPlayerhealth = 100f;
    [SerializeField] private float maxPlayerhealth = 100f;

    [Header("Add the splatter image here")]
    [SerializeField] private Image redSplatterimage = null;

    [Header("Hurt Image Flash")]
    [SerializeField] private Image hurtImage = null;
    [SerializeField] private float hurtTimer = 0.1f;

    [Header("Audio Name")]
    [SerializeField] private AudioClip hurtAudio = null;
    private AudioSource healthAudioSource;

    private void Start()
    {
        healthAudioSource = GetComponent<AudioSource>();
    }

    void UpdateHealth()
    {
        Color splatterAlpha = redSplatterimage.color;
        splatterAlpha.a = 1 - (currentPlayerhealth / maxPlayerhealth);
        redSplatterimage.color = splatterAlpha;

    }

    IEnumerator HurtFlash()
    {
        hurtImage.enabled = true;
        healthAudioSource.PlayOneShot(hurtAudio);
        yield return new WaitForSeconds(hurtTimer);
        hurtImage.enabled = false;
    }

    public void TakeDamage()
    {
        if (currentPlayerhealth >= 0)
        {
            StartCoroutine(HurtFlash());
            UpdateHealth();
        }
    }
}