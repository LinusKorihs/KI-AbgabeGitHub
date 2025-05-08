using System.Collections.Generic;
using UnityEngine;

public class BlockSetter
{
    private readonly BlockMovement block;
    public static int linesCleared = 0;

    public BlockSetter(BlockMovement block)
    {
        this.block = block;
    }

    public void SetBlock()
    {
        foreach (Transform child in block.transform)
        {
            GridManager.AddToGrid(child);
        }

        linesCleared = LineClearManager.ClearFullLines();
        if (linesCleared > 0)
        {
            Debug.Log($"Lines cleared: {linesCleared}"); // Log the number of lines cleared
            ScoreManager.AddScore(linesCleared); // Add score for cleared lines
        }

        CheckParentDestroy(); // Call the blockDeleteList method to check for empty blocks

        NewSpawn(); // Spawn a new block
    }

    public void CheckParentDestroy()
    {
        // All GameObjects with the tag "BlockParent"
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("BlockParent");
        //Debug.Log("Block added: " + block.name + "Number of blocks: " + allBlocks.Length);

        foreach (GameObject block in allBlocks)
        {
            // Does the block have children?
            if (block.transform.childCount == 0)
            {
                // If no children are present, then delete
                //Debug.Log("Block without children found: " + block.name);
                Object.Destroy(block); // Delete the block
            }
        }
    }

    public void NewSpawn()
    {
        block.blockSpawner.SpawnBlock();
        block.enabled = false;
    }
}

