using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void BacktoMainMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }
}