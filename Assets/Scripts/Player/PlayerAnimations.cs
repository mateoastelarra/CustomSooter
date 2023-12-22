using UnityEngine;
using Cinemachine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustVFX;
    [SerializeField] private ParticleSystem _JumpVFX;
    [SerializeField] private ParticleSystem _powerUpPickUpVFX;
    [SerializeField] private float _tiltAngle = 20f;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private float _hatTiltModifier = 8f;
    [SerializeField] Transform _characterSpriteTransform;
    [SerializeField] Transform _characterHatTransform;
    [SerializeField] float _yLandVelocityCheck = -20f;
    [SerializeField] CinemachineImpulseSource _hitGroundImpulseSource;

    private Vector2 _velocityBeforePyhsicsUpdate;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        DetectMoveDust();
        ApplyTilt();
    }

    private void OnEnable()
    {
        PlayerController.OnJump += PlayJumpEffect;
        IPowerUp.OnPowerUpPickUp += PlayPowerUpPickUpVFX;
    }

    private void OnDisable()
    {
        PlayerController.OnJump -= PlayJumpEffect;
        IPowerUp.OnPowerUpPickUp -= PlayPowerUpPickUpVFX;
    }

    private void FixedUpdate()
    {
        _velocityBeforePyhsicsUpdate = _rigidBody.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_velocityBeforePyhsicsUpdate.y < _yLandVelocityCheck)
        {
            PlayJumpEffect();
            _hitGroundImpulseSource.GenerateImpulse();

        }
    }

    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckGrounded())
        {
            if (!_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Play();
            }
        }
        else
        {
            if (_moveDustVFX.isPlaying)
            {
                _moveDustVFX.Stop();
            }
        }
    }

    private void PlayJumpEffect()
    {
        _JumpVFX.Play();
    }

    private void PlayPowerUpPickUpVFX()
    {
        _powerUpPickUpVFX.Play();
    }

    private void ApplyTilt()
    {
        float targetAngle;

        if (PlayerController.Instance.MoveInput.x < 0f)
        {
            targetAngle = _tiltAngle;
        }
        else if (PlayerController.Instance.MoveInput.x > 0f)
        {
            targetAngle = -_tiltAngle;
        }
        else
        {
            targetAngle = 0;
        }

        //Player Sprite
        Quaternion currentCharacterRotation = _characterSpriteTransform.rotation;
        Quaternion targetCharacterRotation = Quaternion.Euler(currentCharacterRotation.eulerAngles.x,
                                                              currentCharacterRotation.eulerAngles.y,
                                                              targetAngle);


        _characterSpriteTransform.rotation = Quaternion.Lerp(currentCharacterRotation, targetCharacterRotation, Time.deltaTime * _tiltSpeed);

        //Hat Sprite
        Quaternion currentHatRotation = _characterHatTransform.rotation;
        Quaternion targetHatRotation = Quaternion.Euler(currentCharacterRotation.eulerAngles.x,
                                                              currentCharacterRotation.eulerAngles.y,
                                                              -targetAngle / _hatTiltModifier);

        _characterHatTransform.rotation = Quaternion.Lerp(currentHatRotation, 
                                                          targetHatRotation, 
                                                          Time.deltaTime * _hatTiltModifier * _tiltSpeed);
    }
}
