using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] private Transform _virtualMousePosition;
    [SerializeField] private bool _playWithController;

    private void Awake()
    {
        Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        MakeCursorVisible(false);
    }

    public Vector3 GetCursorPosition()
    {
        if (!_playWithController)
        {
            _virtualMousePosition.position = Input.mousePosition;
        }
        return _virtualMousePosition.position;
    }

    public void MakeCursorVisible(bool isVisible)
    {
        Cursor.visible = isVisible;
    }
}
