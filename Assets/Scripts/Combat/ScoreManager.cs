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

    private void Start()
    {
        _bestScore = PlayerPrefs.GetInt("BestScore");
        SetBestScore(_bestScore);
    }

    private void Update()
    {
        UpdateBestScore();
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
            SetBestScore(_currentScore);
        }
    }

    private void SetBestScore(int score)
    {
        _bestScore = score;
        PlayerPrefs.SetInt("BestScore", _bestScore);
        _bestScoreText.text = "Best" + _bestScore.ToString("D3");
    }
}
