 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            SpawnDeathSplatterPrefab();
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
}
