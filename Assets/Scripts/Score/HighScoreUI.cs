using TMPro;
using UnityEngine;

public class HighScoreUI : MonoBehaviour
{
    public TextMeshProUGUI highScoreText;

    void Update()
    {
        highScoreText.text = "HighScore: " + ScoreManager.HighScore;
    }
}
