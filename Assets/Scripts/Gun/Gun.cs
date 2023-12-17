using System;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;
using System.Collections;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public static Action OnLaunchGrenade;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private Grenade _grenadePrefab;
    [SerializeField] private float _gunFireCD = 0.5f;
    [SerializeField] private float _gunGreenadeCD = 2f;
    [SerializeField] private float _muzzleFlashTime = .05f;

    private Coroutine _muzzleFlashCoroutine;
    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;
    private float _lastGrenadeTime = 0f;

    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;
    private PlayerInput _playerInput;
    private FrameInput _frameInput;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _playerInput = GetComponentInParent<PlayerInput>();
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        Shoot();
        RotateGun();
        GatherInput();
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += UpdateLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
        OnLaunchGrenade += LaunchGrenade;
        OnLaunchGrenade += UpdateLastGrenadeFireTime;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= UpdateLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
        OnLaunchGrenade -= LaunchGrenade;
        OnLaunchGrenade -= UpdateLastGrenadeFireTime;
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() => {
            return Instantiate(_bulletPrefab);
        }, bullet => {
            bullet.gameObject.SetActive(true);
        }, bullet => {
            bullet.gameObject.SetActive(false);
        }, bullet => {
            Destroy(bullet);
        }, true, 20, 40);
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        if (bullet.isActiveAndEnabled) { _bulletPool.Release(bullet); } 
    }

    private void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    private void Shoot()
    {
        if (_frameInput.FireGrenade && Time.time >= _lastGrenadeTime) 
        {
            OnLaunchGrenade?.Invoke();    
        }
        else if (_frameInput.FireGun && Time.time >= _lastFireTime)
        {
            OnShoot?.Invoke();
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void LaunchGrenade()
    {
        Grenade newGrenade = Instantiate(_grenadePrefab, transform.position, Quaternion.identity);
        newGrenade.Init(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void UpdateLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;
    }

    private void UpdateLastGrenadeFireTime()
    {
        _lastGrenadeTime = Time.time + _gunGreenadeCD;
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void MuzzleFlash()
    {
        if (_muzzleFlashCoroutine != null)
        {
            StopCoroutine(_muzzleFlashCoroutine);
        }

        _muzzleFlashCoroutine = StartCoroutine(MuzzleFlashRoutine());
    }

    private IEnumerator MuzzleFlashRoutine()
    {
        _muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}
