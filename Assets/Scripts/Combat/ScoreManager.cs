using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static Action OnScoreThreshold;

    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _bestScoreText;
    [SerializeField] int _scoreThresholdChanger = 50;

    private int _currentScore = 0;
    private int _bestScore = 0;
    private int _scoreThreshold;

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
        _scoreThreshold = _scoreThresholdChanger;
    }

    private void Update()
    {
        UpdateBestScore();
        UpdateScoreThreshold();
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
        _bestScoreText.text = "Best\n" + _bestScore.ToString("D3");
    }

    private void UpdateScoreThreshold()
    {
        if (_currentScore > _scoreThreshold)
        {
            OnScoreThreshold?.Invoke();
            _scoreThreshold += _scoreThresholdChanger;
        }
    }
}
