using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 MoveInput => _frameInput.Move;

    public static Action OnJump;
    public static Action OnJetpack;

    public static PlayerController Instance;

    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private TrailRenderer _jetPackTrailRenderer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = .2f;
    [SerializeField] private float _coyoteTime = 0.5f;
    [SerializeField] private float _jetpackTime = 0.6f;
    [SerializeField] private float _jetPackStrength = 11f;

    private float _timeInAir, _coyoteTimer;
    private bool _doubleJumpAvailable;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private Movement _movement;
    private Coroutine _jetPackCoroutine;

    private Rigidbody2D _rigidBody;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
        OnJetpack += StartJetpack;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
        OnJetpack -= StartJetpack;
    }

    private void Update()
    {
        GatherInput();
        Movement();
        CoyoteTimer();
        HandleJump();
        HandleSpriteFlip();
        GravityDelay();
        Jetpack();
    }

    private void FixedUpdate()
    {
        ExtraGravity();
    }

    public bool CheckGrounded()
    {
        Collider2D isGrounded = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0, _groundLayer);
        return isGrounded;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);
    }

    private void GravityDelay()
    {
        if (!CheckGrounded())
        {
            _timeInAir += Time.deltaTime;
        }
        else
        {
            _timeInAir = 0;
        }      
    }

    private void ExtraGravity()
    {
        if (_timeInAir > _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0, -_extraGravity));
        }
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    private void Movement()
    {
        _movement.SetCurrentDirection(_frameInput.Move.x);
    }

    private void HandleJump()
    {
        if (!_frameInput.Jump) {
            if (_timeInAir > 0 && CheckGrounded())
            {
                _doubleJumpAvailable = false;
            }
            return; 
        }

        if (_doubleJumpAvailable)
        {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }
        else if (_coyoteTimer > 0)
        {
            _doubleJumpAvailable = true;
            OnJump?.Invoke();
        }
        else if (CheckGrounded())
        {
            _doubleJumpAvailable = true;
            OnJump?.Invoke();
        }

    }

    private void CoyoteTimer()
    {
        if (CheckGrounded())
        {
            _coyoteTimer = _coyoteTime;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private void ApplyJumpForce()
    {
        _rigidBody.velocity = new Vector2 (_rigidBody.velocity.x, 0);
        _timeInAir = 0f;
        _coyoteTimer = 0f;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void Jetpack()
    {
        if (!_frameInput.Jetpack || _jetPackCoroutine != null) { return; }
    
        OnJetpack?.Invoke();
    }

    private void StartJetpack()
    {
        _jetPackCoroutine = StartCoroutine(JetpackRoutine());
    }

    private IEnumerator JetpackRoutine()
    {

        float jetTime = 0;
        while (jetTime < _jetpackTime)
        {
            jetTime += Time.deltaTime;
            _rigidBody.velocity = Vector2.up * _jetPackStrength;
            yield return null;
        }

        

        _jetPackCoroutine = null;
        
    }
}
