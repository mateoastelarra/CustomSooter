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
        _currentScore += sender.StartingHealth;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (_currentScore < 10)
        {
            _scoreText.text = "00"+_currentScore.ToString();
        }
        else if (_currentScore < 100)
        {
            _scoreText.text = "0" + _currentScore.ToString();
        }
        else
        {
            _scoreText.text = _currentScore.ToString();
        }   
    }
}
