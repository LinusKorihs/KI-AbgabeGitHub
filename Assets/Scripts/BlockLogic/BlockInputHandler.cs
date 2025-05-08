using UnityEngine;

public class BlockInputHandler
{
    private readonly BlockMovement block;
    private float previousTime;

    public BlockInputHandler(BlockMovement block)
    {
        this.block = block;
    }

    public void HandleInput()
    {
        HandleMovement();
        HandleRotation();
        HandleFall();
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            TryMove(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            TryMove(Vector3.right);
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            TryRotate(Quaternion.Euler(0, 0, -90));
        }
    }

    private void HandleFall()
    {
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            DropBlock();
        }

        float fallTime = GameDifficultyManager.instance.fallTime;
        if (Time.time - previousTime > 1 / fallTime)
        {
            if (!TryMove(Vector3.down))
            {
                block.FinalizeBlock();
            }
            else
            {
                previousTime = Time.time;
            }
        }
    }

    public void DropBlock()
    {
        while (TryMove(Vector3.down)) { } // Move the block down until it can't anymore
        block.FinalizeBlock(); // Finalize the block's position
        return;
    }

    public bool TryMove(Vector3 direction)
    {
        block.transform.position += direction;

        if (!BlockValidation.IsValidPosition(block.transform))
        {
            block.transform.position -= direction;
            return false;
        }

        return true;
    }

    public bool TryRotate(Quaternion rotation)
    {
        block.transform.rotation *= rotation;

        if (!BlockValidation.IsValidPosition(block.transform))
        {
            block.transform.rotation *= Quaternion.Inverse(rotation);
            return false;
        }

        return true;
    }
}
