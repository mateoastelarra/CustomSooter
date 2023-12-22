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
    private int _currentGrenades;

    private void Start()
    {
        _playerHealth = GetComponent<Health>();
        _playerGun = GetComponentInChildren<Gun>();
        _currentGrenades = _playerGun.GrenadesAtStart;
        UpdateHealthText();
        UpdateGrenadesText();
    }

    private void OnEnable()
    {
        PlayerHit.OnPlayerHit += UpdateHealthText;
        Gun.OnLaunchGrenade += UpdateCurrentGreenades;
        Gun.OnLaunchGrenade += UpdateGrenadesText;
    }

    private void OnDisable()
    {
        PlayerHit.OnPlayerHit -= UpdateHealthText;
        Gun.OnLaunchGrenade -= UpdateCurrentGreenades;
        Gun.OnLaunchGrenade -= UpdateGrenadesText;
    }

    public void UpdateHealthText()
    {
        if (_playerHealth.CurrentHealth < 0) { return; }
        _healthText.text = _playerHealth.CurrentHealth.ToString();
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
