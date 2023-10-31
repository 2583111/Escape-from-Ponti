using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    Light testLight;
    public float minWaitTime;
    public float maxWaitTime;

    AudioSource source;
    public AudioClip flickerSounds;

    private void Start()
    {
        testLight = GetComponent<Light>();
        StartCoroutine(Flashing());
        source = GetComponent<AudioSource>();

    }
    IEnumerator Flashing()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            testLight.enabled =!testLight.enabled;

            if(source != null && flickerSounds != null )
            {
                source.PlayOneShot(flickerSounds);
            }
        }
    }
}
