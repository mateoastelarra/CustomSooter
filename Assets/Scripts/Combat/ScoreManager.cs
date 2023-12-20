using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _bestScoreText;

    private int _currentScore = 0;
    private int _bestScore = 0;

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
        if (sender.gameObject.CompareTag("Player")) 
        {
            ResetScore();
            return; 
        }
        _currentScore += sender.StartingHealth;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        _scoreText.text = "Score" + _currentScore.ToString("D3");   
    }

    private void ResetScore()
    {
        UpdateBestScore();
        _currentScore = 0;
        UpdateScoreText();
    }

    private void UpdateBestScore()
    {
        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            _bestScoreText.text = "Best" + _currentScore.ToString("D3");
        }
    }
}
