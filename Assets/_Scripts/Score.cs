using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;

    private int score;

    public static Score S;

    public void Awake()
    {
        score = 0;
        S = this;
        scoreText.text = "Score: 0";
    }

    public void UpdateScore(int points)
    {
        score = score + points;
        scoreText.text = "Score: " + score.ToString();
    }
}