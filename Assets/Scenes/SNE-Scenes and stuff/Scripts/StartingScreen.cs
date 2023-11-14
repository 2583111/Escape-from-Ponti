using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartingScreen : MonoBehaviour
{
    //private GameObject player;
    public GameObject startingScreen;
    public float startingScreenTime = 3f;

    public AudioClip startingScreenClip;
    public AudioClip gamecreditsClip;

    private AudioSource audioSource;
    public Canvas gameCredits;

    public float waitTime;

    
    private bool inGamecredit = false;
    void Start()
    {
        startingScreen.SetActive(true);
        //player = GameObject.FindWithTag("Player");
        //player.GetComponent<FirstPersonController>().enabled = false;
        gameCredits.enabled = false;
       audioSource = GetComponent<AudioSource>();

        audioSource.clip = startingScreenClip;
        audioSource.Play();


        StartCoroutine(Starting());
    }

    IEnumerator Starting()
    {
        yield return new WaitForSeconds(startingScreenTime);
        //inGamecredit = true;
        ShowGameCredits();
        startingScreen.SetActive(false);
        //  player.GetComponent<FirstPersonController>().enabled = true;
        audioSource.clip = gamecreditsClip;
        audioSource.loop = true;
        
    }
    void ShowGameCredits()
    {
        gameCredits.enabled = true;
    }

    private void Update()
    {
        if (inGamecredit)
        {
            Debug.Log("This worked");
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                audioSource.Stop();
                SceneManager.LoadScene("Next Scene");

            }
        }
    }



}
