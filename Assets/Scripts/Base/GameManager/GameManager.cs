using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGame()
    {
        // Initialize game state, load resources, etc.
        Debug.Log("Game Started");
    }

    public void EndGame()
    {
        // Handle game over logic, save state, etc.
        Debug.Log("Game Over");
    }

    public void RestartGame()
    {
        // Reset game state, reload scene, etc.
        Debug.Log("Game Restarted");
    }

}