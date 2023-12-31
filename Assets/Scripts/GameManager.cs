using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GMInstance;
    public So_Scorer so_Scorer;
    private float score;
    private float highScore;
    private float lastScore;
    public float multiplier = 1;

    private void Awake()
    {
        GMInstance = this;
        
    }


    public void SetScore(float score )
    {
        so_Scorer.currentScore += score * multiplier;
        if (so_Scorer.HighScore < so_Scorer.currentScore)
        {
            SetHighScore();
        }
    }

    public void SetHighScore()
    {
        so_Scorer.HighScore = so_Scorer.currentScore;
    }

}
