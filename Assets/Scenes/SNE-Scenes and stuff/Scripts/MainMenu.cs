using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void Play()
    {
        SceneManager.LoadScene("Level");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene("Level");
    }
   
    public void WinningScreen()
    {
        SceneManager.LoadScene("WinningScreen");
    }
   
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit the game");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartScreen");
        
    }
    
}
