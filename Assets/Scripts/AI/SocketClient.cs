using System.Net.Sockets;
using System.Text;
using NUnit.Framework.Internal;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class SocketClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    public bool connected = false; // Flag to check if connected to the server

    public void ConnectToPy()
    {
        if (connected) return;

        try
        {
            client = new TcpClient("127.0.0.1", 65432);
            stream = client.GetStream();
            Debug.Log("[UNITY] Connected to Pyhton Server."); // Log the connection status
            connected = true;
        }
        catch
        {
            Debug.LogError("[UNITY] Connection to Python Server failed.");
        }
    }

    public void Disconnect()
    {
        if (!connected) return;
        if (client != null && client.Connected)
        {
            client.Close();
            connected = false;
            Debug.Log("[UNITY] Disconnected from Python server.");
        }
    }

    public void SendGameState(string currentGameState, string lastMove, string newState, int linesCleared, bool moveBlocked)
    {
        if (client == null || !client.Connected) return;

        string gameState = $"gridBefore:{currentGameState};gridAfter:{newState};lastMove:{lastMove};linesCleared:{linesCleared};moveBlocked:{moveBlocked}|"; // Create the game state string
        GameStateToByte(gameState); // Convert the game state to byte array and send it to the server
        GetAIAnswer(); // Get the AI action from the server
    }

    public void GameStateToByte(string gameState)
    {
        byte[] data = Encoding.UTF8.GetBytes(gameState); // Convert the game state to bytes
        stream.Write(data, 0, data.Length); // Send the game state to the server
    }

    private void GetAIAnswer()
    {
        byte[] response = new byte[1024]; // Buffer for reading the response
        int bytes = stream.Read(response, 0, response.Length); // Read the response from the server
        string actionStr = Encoding.UTF8.GetString(response, 0, bytes); // Convert the response to a string

        if (string.IsNullOrEmpty(actionStr)) // Check if the response is empty or null
        {
            Debug.LogWarning("[UNITY] Empty AI Action received from Python Server.");
        }

        InputHandlerAI.instance?.PerformAction(actionStr);
    }

    public string GridToString()
    {
        string state = "";
        for (int y = 0; y < GridManager.height; y++)
        {
            for (int x = 0; x < GridManager.width; x++)
            {
                state += GridManager.GetAt(x, y) != null ? "1" : "0";
            }
        }
        return state;
    }

    public void SendGameOver()
    {
        if (client == null || !client.Connected) return;

        string gameOverMessage = "gameOver:true|"; // Send a message to indicate game over
        byte[] data = Encoding.UTF8.GetBytes(gameOverMessage);
        stream.Write(data, 0, data.Length);
        //Debug.Log("[UNITY] Sent GameOver status to Python.");
    }

    private void OnApplicationQuit() 
    {
        stream?.Close();
        client?.Close();
        Debug.Log("[UNITY] Connection to Python Server closed."); // Log the disconnection status
    }
}
