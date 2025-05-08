using System;
using UnityEngine;

public class InputHandlerAI : MonoBehaviour
{
    public static InputHandlerAI instance;
    private BlockInputHandler handler; // Reference to the block input handler
    private SocketClient socketClient; // Reference to the SocketClient script
    private string lastMove = ""; // Variable to store the last move made by the AI
    private bool firstTurn = true; // Flag to check if it's the first turn

    private void Awake()
    {
        instance = this;
        socketClient = FindFirstObjectByType<SocketClient>(); // Find the SocketClient instance in the scene
    }

    public void SetBlockInputHandler(BlockInputHandler newHandler)
    {
        handler = newHandler;
    }

    public void PerformAction(string actionStr)
    {
        if (!int.TryParse(actionStr, out int action) && !firstTurn)
        {
            Debug.LogWarning("[UNITY] AI-Action invalid: " + actionStr);
            return;
        }

        if (handler == null)
        {
            Debug.LogError("[UNITY] Handler is null in PerformAction.");
            return;
        }

        // 0 = left, 1 = right, 2 = flip, 3 = placeDown
        switch(action)
        {
            case 0:
                handler.TryMove(Vector3.left); 
                lastMove = "Move Left"; // Save the last action
                break;
            case 1:
                handler.TryMove(Vector3.right);
                lastMove = "Move Right";
                break;
            case 2:
                handler.TryRotate(Quaternion.Euler(0, 0, -90));
                lastMove = "Rotate";
                break;
            case 3:
                handler.DropBlock();
                lastMove = "Drop";
                break;
            case 4:
                lastMove = "Do nothing";
                break;
        }

        if (firstTurn)  firstTurn = false;
        //Debug.Log("[UNITY] Last Move: " + lastMove);
    }

    public string GetLastMove()
    {
        return lastMove;
    }
}
