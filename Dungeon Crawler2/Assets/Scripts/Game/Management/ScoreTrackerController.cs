using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTrackerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    private int depthAchieved = 0;

    public void IncreaseDepth()
    {
        depthAchieved++;
        UpdateScore();
    }
    public void UpdateScore()
    {
        scoreText.text = "Depth: " + depthAchieved;
    }
    public void ResetScore()
    {
        depthAchieved = 0;
        UpdateScore();
    }
}
