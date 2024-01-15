using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }

    private PlayerInputActions _playerInputActions;
    private InputAction _move, _jump, _jetpack, _fireGun, _fireGrenade, _aimWithJoystick;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();

        _move = _playerInputActions.Player.Move;
        _aimWithJoystick = _playerInputActions.Player.Aim;
        _jump = _playerInputActions.Player.Jump;
        _jetpack = _playerInputActions.Player.Jetpack;
        _fireGun = _playerInputActions.Player.FireGun;
        _fireGrenade = _playerInputActions.Player.FireGrenade;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    private void Update()
    {
       FrameInput = GatherInput();
    }

    private FrameInput GatherInput() 
    {
        return new FrameInput {
            Move = _move.ReadValue<Vector2>(),
            AimWithJoystick = _aimWithJoystick.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame(),
            Jetpack = _jetpack.WasPressedThisFrame(),
            FireGun= _fireGun.IsPressed(),
            FireGrenade = _fireGrenade.WasPerformedThisFrame(),
        };
    }
}

public struct FrameInput
{
    public Vector2 Move;
    public Vector2 AimWithJoystick;
    public bool Jump;
    public bool Jetpack;
    public bool FireGun;
    public bool FireGrenade;
}
