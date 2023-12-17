using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour, IDamageable
{
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _spawnTimer = 3f;

    private ColorChanger _colorChanger;
    private Health _health;
    private Flash _flash;

    private void Awake()
    {
        _colorChanger = GetComponent<ColorChanger>();
        _health = GetComponent<Health>();
        _flash = GetComponent<Flash>();
    }

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            _colorChanger.SetRandomColor();
            Enemy enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
            enemy.Init(_colorChanger.DefaultColor);
            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    public void TakeDamage(int damageAmount, float knockbackTrhust)
    {
        _health.TakeDamage(damageAmount);
    }

    public void TakeHit()
    {
        _flash.StartFlash();
    }
}
