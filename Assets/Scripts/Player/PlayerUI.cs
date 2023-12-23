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

    private void Start()
    {
        _playerHealth = GetComponent<Health>();
        _playerGun = GetComponentInChildren<Gun>();
        UpdateHealthText();
        UpdateGrenadesText();
    }

    private void OnEnable()
    {
        PlayerHit.OnPlayerHit += UpdateHealthText;
        Gun.OnLaunchGrenade += UpdateGrenadesText;
        Health.OnDeath += UpdateHealthText;
    }

    private void OnDisable()
    {
        PlayerHit.OnPlayerHit -= UpdateHealthText;
        Gun.OnLaunchGrenade -= UpdateGrenadesText;
        Health.OnDeath -= UpdateHealthText;
    }

    public void UpdateHealthText()
    {
        if (_playerHealth.CurrentHealth <= 0) { return; }
        _healthText.text = _playerHealth.CurrentHealth.ToString();
    }

    public void UpdateHealthText(Health sender)
    {
        if (sender != _playerHealth) { return; }
        if (sender.CurrentHealth <= 0)
        {
            _healthText.text = "0";
        }
    }

    public void UpdateGrenadesText()
    {
        _grenadesText.text = _playerGun.CurrentGrenades.ToString();
        Debug.Log(_playerGun.CurrentGrenades);
    }
}
