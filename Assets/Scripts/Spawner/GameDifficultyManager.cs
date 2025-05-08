using UnityEngine;

public class GameDifficultyManager : MonoBehaviour
{
    public static GameDifficultyManager instance;

    public float fallTime = 1f;
    public float cooldown = 5f; // Time in seconds between difficulty increases
    public float fallSpeedIncrease = 0.1f; // Amount to increase fall time 
    public float maxFallTime = 5f; // Maximum fall time
    private float timer;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown && fallTime + fallSpeedIncrease < maxFallTime)
        {
            Debug.Log("Increasing difficulty from: " + fallTime + " to: " + (fallTime + fallSpeedIncrease));
            IncreaseDifficulty();
        }
        else if (fallTime + fallSpeedIncrease >= maxFallTime)
        {
            Debug.Log("Current fall time can not be increased: " + fallTime);
        }
    }

    public void IncreaseDifficulty()
    {
        timer = 0f;
        fallTime += fallSpeedIncrease;

        //Debug.Log("FallTime adjusted to: " + fallTime);
    }
}
