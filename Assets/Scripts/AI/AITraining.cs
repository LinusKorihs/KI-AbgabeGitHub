using UnityEditor.PackageManager;
using UnityEngine;

public class AITraining : MonoBehaviour
{
    public static int maxEpisodes = 1000000; // Maximum number of episodes for training
    public static int currentEpisode = 0;
    public bool isTraining = true;

    private SocketClient socketClient;

    void Start()
    {
        socketClient = FindFirstObjectByType<SocketClient>();
    }

    void Update()
    {
        if (!isTraining || socketClient == null) return;

        if (GameOverManager.gameOver == true)
        {
            currentEpisode++;

            if (currentEpisode >= maxEpisodes)
            {
                Debug.Log("Training completed!");
                isTraining = false;
                return;
            }

            // Reset Game 
            if (GameOverManager.instance.restartOnGameOver == true)
            {
                GameOverManager.RestartGame();
            }
        }
    }
}
