using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Text gameOverText;
    //public PlayerHealth playerHealth;

    public Button restartButton;
    public Button mainMenuButton;


    private bool isGameOver = false;

    private void Start()
    {
        //gameOverScreen.SetActive(false);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    /*public void Update()
    {
        if (!isGameOver && playerHealth.currentHealth <= 0)
        {
            GameOver();
        }
    }*/
    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        isGameOver = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game has restarted and button works!");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Debug.Log("MainMenu screen loads");
    }
}
