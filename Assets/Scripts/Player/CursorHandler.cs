using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;

    private void Awake()
    {
        Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
