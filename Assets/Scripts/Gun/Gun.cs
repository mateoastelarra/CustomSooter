using System;
using UnityEngine;
using UnityEngine.Pool;
using Cinemachine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class Gun : MonoBehaviour
{
    public int CurrentGrenades { get => _currentGrenades; set => _currentGrenades = value; }
    
    public static Action OnShoot;
    public static Action OnRegularShoot;
    public static Action OnSpecialShoot;
    public static Action OnLaunchGrenade;

    [SerializeField] private Transform _bulletSpawnPoint;

    [Header("Bullet")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Bullet _specialBulletPrefab;
    [SerializeField] private Light2D _muzzleFlash;
    [SerializeField] private float _gunFireCD = 0.5f;
    [SerializeField] private float _muzzleFlashTime = .05f;

    [Header("Grenade")]
    [SerializeField] [Range(0, 20)] private int _grenadesAtStart = 3;
    [SerializeField] private Grenade _grenadePrefab;
    [SerializeField] private float _gunGreenadeCD = 2f;
    

    private Coroutine _muzzleFlashCoroutine;
    private Coroutine _specialBulletCoroutine;
    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;
    private float _lastGrenadeTime = 0f;
    private int _currentGrenades;
    private bool _specialBulletActive;

    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;
    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private CursorHandler _virtualMouseCursor;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _playerInput = GetComponentInParent<PlayerInput>();
        _virtualMouseCursor = GetComponentInParent<CursorHandler>();
        CurrentGrenades = _grenadesAtStart;
    }

    private void Start()
    {
        GatherInput();
        CreateBulletPool();
    }

    private void Update()
    {
        GatherInput();
        Shoot();
        RotateGun();    
    }

    private void OnEnable()
    {
        OnRegularShoot += ShootProjectile;
        OnSpecialShoot += ShootSpecialProjectile;
        OnShoot += UpdateLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
        OnShoot += MuzzleFlash;
        OnLaunchGrenade += LaunchGrenade;
        OnLaunchGrenade += FireAnimation;
        OnLaunchGrenade += UpdateLastGrenadeFireTime;
    }

    private void OnDisable()
    {
        OnRegularShoot -= ShootProjectile;
        OnSpecialShoot -= ShootSpecialProjectile;
        OnShoot -= UpdateLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
        OnShoot -= MuzzleFlash;
        OnLaunchGrenade -= LaunchGrenade;
        OnLaunchGrenade -= FireAnimation;
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
        if (_frameInput.FireGrenade && Time.time >= _lastGrenadeTime && CurrentGrenades > 0) 
        {
            CurrentGrenades -= 1;
            OnLaunchGrenade?.Invoke();    
        }
        else if (_frameInput.FireGun && Time.time >= _lastFireTime)
        {
            OnShoot?.Invoke();
            if (!_specialBulletActive)
            {
                OnRegularShoot?.Invoke();
            }
            else
            {
                OnSpecialShoot?.Invoke();
            }
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
         
    }

    private void ShootSpecialProjectile()
    {
        Bullet newSpecialBullet = Instantiate(_specialBulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newSpecialBullet.Init(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void LaunchGrenade()
    {
        Grenade newGrenade = Instantiate(_grenadePrefab, _bulletSpawnPoint.position, Quaternion.identity);
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
        _mousePos = Camera.main.ScreenToWorldPoint(_virtualMouseCursor.GetCursorPosition());
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
        _muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.gameObject.SetActive(false);
    }

    private void ChangeMuzzleFlashColor(Color color)
    {
        _muzzleFlash.color = color;
    }

    public void ActivateSpecialBullets(float time)
    {
        if (_specialBulletCoroutine == null)
        {
            _specialBulletCoroutine = StartCoroutine(ActivateSpecialBulletsRoutine(time));
        }
        else
        {
            StopCoroutine(_specialBulletCoroutine);
            _specialBulletCoroutine = StartCoroutine(ActivateSpecialBulletsRoutine(time));
        }   
    }

    private IEnumerator ActivateSpecialBulletsRoutine(float time)
    {
        _specialBulletActive = true;
        ChangeMuzzleFlashColor(Color.red);
        yield return new WaitForSeconds(time);
        _specialBulletActive = false;
        ChangeMuzzleFlashColor(Color.white);
    }
}
