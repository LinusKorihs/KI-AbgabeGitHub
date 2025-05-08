using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public static bool gameOver = false; // Flag to check if the game is over
    public static GameOverManager instance;
    public bool restartOnGameOver = true; // Flag to check if the game should restart on game over
 
    private void Awake()
    {
        instance = this;
        gameOverUI.SetActive(false); // UI is initially hidden
    }

    public static void TriggerGameOver()
    {
        gameOver = true; // Set the game over flag to true
        Debug.Log("Game Over!");

        Time.timeScale = 0f; // pause the game
        ScoreManager.CheckHighScore(); // Check if the current score is a high score
        instance.gameOverUI.SetActive(true); // Show the Game Over UI

        var socketClient = FindFirstObjectByType<SocketClient>();
        if (socketClient != null && socketClient.connected) socketClient.SendGameOver();
        socketClient?.Disconnect();
    }

    public static void RestartGame()
    {
        gameOver = false; // Reset the game over flag
        Time.timeScale = 1f;
        ScoreManager.ResetScore(); // Reset the score when restarting
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
