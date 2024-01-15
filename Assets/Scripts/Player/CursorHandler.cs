using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.LowLevel;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Transform _virtualMouseCursor;
    [SerializeField] private bool _playWithController;
    [SerializeField] private float padding = 20f;
    [SerializeField] private VirtualMouseInput _virtualMouseInput;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private Vector3 _currentMousePosition;
    private Vector3 _virtualMousePosition;
    

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        Cursor.visible = false;
        _currentMousePosition = Vector3.zero;
    }

    private void Update()
    {
        _frameInput = _playerInput.FrameInput;
    }

    public Vector3 GetCursorPosition()
    {
        Debug.Log(_virtualMouseInput.virtualMouse.position.value);
        if (_frameInput.AimWithJoystick == Vector2.zero && _currentMousePosition != Input.mousePosition)
        {
            _virtualMouseCursor.position = Input.mousePosition;
            _currentMousePosition = Input.mousePosition;
        }
        _virtualMousePosition.x = Mathf.Clamp(_virtualMouseCursor.position.x, padding, Screen.width - padding);
        _virtualMousePosition.y = Mathf.Clamp(_virtualMouseCursor.position.y, padding, Screen.height - padding);
        _virtualMouseCursor.position = _virtualMousePosition;
        InputState.Change(_virtualMouseInput.virtualMouse.position, _virtualMousePosition);
        return _virtualMousePosition;
    }
}
