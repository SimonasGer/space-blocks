using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI score;
    public DeleteBlocks deleteBlocks;
    public void UpdateScore(int newScore)
    {
        score.text = $"Score: {newScore}";
    }
}
