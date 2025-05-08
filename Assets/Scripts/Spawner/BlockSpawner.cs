using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using NUnit.Framework.Internal;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blockPrefabs;  // All Tetris-Prefabs in Inspector
    public Transform spawnPoint;       // Empty Gameobject over Grid (Spawnpoint)
    public SocketClient socketClient; // Reference to the SocketClient script
    public GameObject newBlock; // Reference to the new block
    public int index = 0; // Index of the current block

    private void Start()
    {
        SpawnBlock();
    }

    public float timer = 0f;
    public float interval = 0.2f; // Cooldown of AI-Action in seconds

    void Update()
    {
        if (GameOverManager.gameOver) return;
        
        CheckForInfos();
    }

    public void CheckForInfos()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            SendInfos(index);
            timer = 0f;
        }
    }

    public void SpawnBlock()
    {
        if (GameOverManager.gameOver) return;
        index = Random.Range(0, blockPrefabs.Length);

        if (!CanSpawnBlock())
        {
            GameOverManager.TriggerGameOver();
            return;
        }
        newBlock = Instantiate(blockPrefabs[index], spawnPoint.position, Quaternion.identity);
        BlockMovement movement = newBlock.GetComponent<BlockMovement>(); // Get the BlockMovement component from the new block
        movement.blockSpawner = this; // Set the block spawner reference in the block movement script

        while (!socketClient.connected)
        {
            socketClient.ConnectToPy();
        }

        AIInput(newBlock); // Set the AI input handler for the new block
    }

    public bool CanSpawnBlock()
    {
        Vector2Int.RoundToInt(spawnPoint.position);

        // Check if the spawn position is inside the grid boundaries
        for (int x = 0; x < GridManager.width; x++)
        {
            if (GridManager.GetAt(x, GridManager.height - 1) != null)
            {
                return false;
            }
        }

        return true;
    }

    private void AIInput(GameObject newBlock)
    {
        BlockMovement movement = newBlock.GetComponent<BlockMovement>(); // Get the BlockMovement component from the new block
        if (movement != null)
        {
            BlockInputHandler handler = new(movement); // Create a new BlockInputHandler instance with the block movement
            InputHandlerAI.instance.SetBlockInputHandler(handler); // Set the block input handler for the AI            
        }
    }

    public void SendInfos(int index)
    {
        string gameState = socketClient.GridToString() + ";" + index; // z. B. "101000111...;2"
        string lastMove = InputHandlerAI.instance?.GetLastMove() ?? "";
        string newState = socketClient.GridToString();
        int clear = 0;
        if (BlockSetter.linesCleared > 0)
        {
            clear = BlockSetter.linesCleared; // Add lines cleared to the game state
            BlockSetter.linesCleared = 0; // Reset lines cleared after sending
        }
        Transform currentBlock = newBlock.transform; // Get the current block transform
        bool moveBlocked = BlockValidation.IsMoveBlocked(currentBlock, Vector2Int.left) || BlockValidation.IsMoveBlocked(currentBlock, Vector2Int.right);
        socketClient.SendGameState(gameState, lastMove, newState, clear, moveBlocked);
    }

    [ContextMenu("Test Lines Cleared")]
    public void TestLinesCleared()
    {
        TestLinesCleared(2); // Test with 2 line cleared
    }
    public void TestLinesCleared(int lines)
    {
        BlockSetter.linesCleared = lines;
        SendInfos(index);
    }
}
