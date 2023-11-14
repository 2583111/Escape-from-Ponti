using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GAMEMANAGER : MonoBehaviour
{
    public Canvas gameCredits;
    public List<string> levels; // A list of level names
    public GameObject player;
    public GameObject gate;

    private int currentLevelIndex = 0;
    private int totalItemsToCollect = 0;
    private int collectedItems = 0;

    void Start()
    {
        gameCredits.enabled = false;
        LoadCurrentLevel();
        totalItemsToCollect = CountItemsInLevel();
    }

    void LoadCurrentLevel()
    {
        if (currentLevelIndex >= levels.Count)
        {
            // All levels completed, show game credits
            gameCredits.enabled = true;
            player.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(levels[currentLevelIndex]);
        }
    }

    int CountItemsInLevel()
    {
        // Implement your logic to count items in the current level
        // You can use tags, triggers, or any other mechanism to identify collectible items.
        // For example, you might use GameObject.FindGameObjectsWithTag and count them.
        // Replace this with your actual implementation.
        return 10; // Replace with the actual count of items in the current level.
    }

    void Update()
    {
        if (currentLevelIndex < levels.Count)
        {
            if (collectedItems == totalItemsToCollect)
            {
                currentLevelIndex++; // Move to the next level
                LoadCurrentLevel();
            }
        }
        else
        {
            // Handle game completion logic here (e.g., show ending credits, restart the game, etc.)
        }
    }

    public void CollectItem()
    {
        collectedItems++;
    }

    public void UnlockGate()
    {
        gate.SetActive(false); // Example: Deactivate the gate when unlocked.
    }
}


