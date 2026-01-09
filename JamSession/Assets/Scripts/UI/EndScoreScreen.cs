using System;
using TMPro;
using UnityEngine;

public class EndScoreScreen : MonoBehaviour
{
    [SerializeField] Scoremanager _scoremanager;
    [SerializeField] TextMeshProUGUI _scoreText;
    
    private void Awake()
    {
        GameEvents.GameOver += ShowScoreScreen;
    }

    private void OnDestroy()
    {
        GameEvents.GameOver -= ShowScoreScreen;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void ShowScoreScreen()
    {
        _scoreText.text = "Score: " + _scoremanager.GetScore();
        gameObject.SetActive(true);
    }
}
