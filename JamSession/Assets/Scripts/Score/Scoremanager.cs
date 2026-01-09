using System;
using TMPro;
using UnityEngine;

public class Scoremanager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int _score = 0;
    
    public int GetScore() => _score;

    public void IncreaseScore()
    {
        _score++;
        scoreText.text = "Score: " + _score;
    }

    private void OnEnable()
    {
        GameEvents.BuzzKillDestroyed += IncreaseScore;
    }

    private void OnDisable()
    {
        GameEvents.BuzzKillDestroyed -= IncreaseScore;
    }
}
