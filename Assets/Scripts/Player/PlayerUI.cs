using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _grenadesText;

    private Health _playerHealth;
    private Gun _playerGun;
    private int _currentHealth;
    private int _currentGrenades;

    private void Start()
    {
        GameObject player = PlayerController.Instance.gameObject;
        _playerHealth = player.GetComponent<Health>();
        _playerGun = player.GetComponentInChildren<Gun>();
        _currentHealth = _playerHealth.StartingHealth;
        _currentGrenades = _playerGun.GrenadesAtStart;
        UpdateHealthText();
        UpdateGrenadesText();
    }

    private void OnEnable()
    {
        PlayerHit.OnPlayerHit += UpdateCurrentHealth;
        PlayerHit.OnPlayerHit += UpdateHealthText;
        Gun.OnLaunchGrenade += UpdateCurrentGreenades;
        Gun.OnLaunchGrenade += UpdateGrenadesText;
    }

    private void OnDisable()
    {
        PlayerHit.OnPlayerHit -= UpdateCurrentHealth;
        PlayerHit.OnPlayerHit -= UpdateHealthText;
        Gun.OnLaunchGrenade -= UpdateCurrentGreenades;
        Gun.OnLaunchGrenade -= UpdateGrenadesText;
    }

    private void UpdateCurrentHealth()
    {
        _currentHealth--;
    }

    private void UpdateHealthText()
    {
        if (_currentHealth < 0) { return; }
        _healthText.text = _currentHealth.ToString();
    }

    private void UpdateGrenadesText()
    {
        _grenadesText.text = _currentGrenades.ToString();    
    }

    private void UpdateCurrentGreenades()
    {
        _currentGrenades--;
    }
}
