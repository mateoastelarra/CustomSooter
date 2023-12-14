using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Grenade : MonoBehaviour
{
    [SerializeField] private GameObject _grenadeVFXPrefab;
    [SerializeField] private float _shootImpulse = 20f;
    [SerializeField] private float _explotionTime = 2f;
    [SerializeField] private float _explotionRadius = 1f;
    [Range(1,3)]
    [SerializeField] private int _amountOfTicks = 3;
    [SerializeField] private float _tickTime = 0.1f;
    [SerializeField] private float _torqueAmount;
    [SerializeField] private int _damageAmount = 3;
    [SerializeField] private float _knockBackThrust = 20f;
    
    private Vector2 _fireDirection;
    private Gun _gun;
    private Rigidbody2D _rigidBody;
    private Light2D _tickingLight;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _tickingLight = GetComponentInChildren<Light2D>();

        Debug.Log(_tickingLight);
    }

    private void Start()
    {
        _rigidBody.AddForce(_fireDirection * _shootImpulse);
        _rigidBody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
        GrenadeTicking();
    }

    public void Init(Gun gun, Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        _gun = gun;
        transform.position = bulletSpawnPos;
        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }

    private void FixedUpdate()
    {
        
        //_rigidBody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.GetComponent<IDamageable>() != null)
        {
            Explode();
        }
    }

    private void GrenadeTicking()
    {
        StartCoroutine(GrenadeRoutine());
    }

    private IEnumerator GrenadeRoutine()
    {
        int ticks = 0;
        while(ticks < _amountOfTicks)
        {
            ticks += 1;
            StartCoroutine(Tick());
            yield return new WaitForSeconds(_explotionTime / _amountOfTicks);            
        }
        Explode();    
    }

    private IEnumerator Tick()
    {
        _tickingLight.enabled = true;
        yield return new WaitForSeconds(_tickTime);
        _tickingLight.enabled = false;
    }

    private void Explode()
    {
        Debug.Log("Kaboooom!!");
        //Instantiate(_grenadeVFXPrefab, transform.position, transform.rotation);

        Collider2D[] inExplotionZoneColliders = Physics2D.OverlapCircleAll(transform.position, _explotionRadius);
        foreach (Collider2D collider in inExplotionZoneColliders)
        {
            IHitable iHitable = collider.gameObject.GetComponent<IHitable>();
            iHitable?.TakeHit();

            IDamageable iDamageable = collider.gameObject.GetComponent<IDamageable>();
            iDamageable?.TakeDamage(_damageAmount, _knockBackThrust);

        }

        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
