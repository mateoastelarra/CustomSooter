using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Transform _virtualMousePosition;
    [SerializeField] private bool _playWithController;

    PlayerInput _playerInput;
    FrameInput _frameInput;
    Vector3 _currentMousePosition;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        MakeCursorVisible(false);
        _currentMousePosition = Vector3.zero;
    }

    private void Update()
    {
        _frameInput = _playerInput.FrameInput;
    }

    public Vector3 GetCursorPosition()
    {
 
        if (_frameInput.AimWithJoystick == Vector2.zero && _currentMousePosition != Input.mousePosition)
        {
            _virtualMousePosition.position = Input.mousePosition;
            _currentMousePosition = Input.mousePosition;
        }
        return _virtualMousePosition.position;
    }

    public void MakeCursorVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
    }
}
