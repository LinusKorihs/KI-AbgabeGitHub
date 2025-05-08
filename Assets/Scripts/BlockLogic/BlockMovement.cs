using UnityEngine;
public class BlockMovement : MonoBehaviour
{
    private BlockInputHandler inputHandler;
    private BlockSetter setter;
    public BlockSpawner blockSpawner;

    void Start()
    {
        inputHandler = new BlockInputHandler(this);
        setter = new BlockSetter(this);
        blockSpawner = FindFirstObjectByType<BlockSpawner>();
    }

    void Update()
    {
        inputHandler?.HandleInput(); // Handle input for movement, rotation, and falling
    }

    public void FinalizeBlock()
    {
        if (setter == null) setter = new BlockSetter(this); // Initialize setter if null
        setter.SetBlock();
    }
}
