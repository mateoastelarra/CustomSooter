 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public Action OnDeath;

    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFXPrefab;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    private void OnEnable()
    {
        OnDeath += SpawnDeathSplatterPrefab;
        OnDeath += SpawnDeathVFXPrefab;
    }

    private void OnDisable()
    {
        OnDeath -= SpawnDeathSplatterPrefab;
        OnDeath -= SpawnDeathVFXPrefab;
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    public void SpawnDeathSplatterPrefab()
    {
        GameObject newSplatterPrefab = Instantiate(_splatterPrefab, transform.position, transform.rotation);
        SpriteRenderer splatterSR = newSplatterPrefab.GetComponent<SpriteRenderer>();
        ColorChanger enemyColorChanger = GetComponent<ColorChanger>();
        splatterSR.color = enemyColorChanger.DefaultColor;
    }

    public void SpawnDeathVFXPrefab()
    {
        GameObject newDeathVFX = Instantiate(_deathVFXPrefab, transform.position, transform.rotation);
        ParticleSystem.MainModule psMainModule = newDeathVFX.GetComponent<ParticleSystem>().main;
        ColorChanger enemyColorChanger = GetComponent<ColorChanger>();
        psMainModule.startColor = enemyColorChanger.DefaultColor;
    }
}
