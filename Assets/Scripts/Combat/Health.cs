using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    public GameObject SplatterPrefab => _splatterPrefab;
    public GameObject DeathVFXPrefab => _deathVFXPrefab;

    public static Action<Health> OnDeath;

    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathVFXPrefab;
    [SerializeField] private int _startingHealth = 3;

    private Knockback _knockback;
    private Flash _flash;
    private Health _health;
    private bool _isInmune;
    private int _currentHealth;

    private void Awake()
    {
        _knockback = GetComponent<Knockback>();
        _flash = GetComponent<Flash>();
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        if (_knockback)
        {
            _knockback.OnKnockBackStart += BeInmune;
            _knockback.OnKnockBackEnd += StopBeingInmune;
        }    
    }

    private void OnDisable()
    {
        if(_knockback)
        {
            _knockback.OnKnockBackStart -= BeInmune;
            _knockback.OnKnockBackEnd -= StopBeingInmune;
        }  
    }

    private void Start() {
        ResetHealth();
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        if (_isInmune) { return; }
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(Vector2 damageSourceDirection, int damageAmount, float knockbackTrhust)
    {
        _health.TakeDamage(damageAmount);
        _knockback?.GetKnockedBack(damageSourceDirection, knockbackTrhust);
    }

    public void TakeHit()
    {
        _flash.StartFlash();
    }

    public void BeInmune()
    {
        if (gameObject.CompareTag("Player"))//Only player gets damage cool down by being invincible
            _isInmune = true;
    }

    public void StopBeingInmune()
    {
        _isInmune = false;
    }
}
