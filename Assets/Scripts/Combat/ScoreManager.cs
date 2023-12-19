using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;

    private int _currentScore = 0;

    private void OnEnable()
    {
        Health.OnDeath += AddScore;
    }

    private void OnDisable()
    {
        Health.OnDeath -= AddScore;
    }

    private void AddScore(Health sender)
    {
        if (sender.gameObject.CompareTag("Player")) { return; }
        _currentScore += sender.StartingHealth;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = _currentScore.ToString("D3");   
    }
}
