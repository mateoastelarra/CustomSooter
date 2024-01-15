using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    [SerializeField] private Texture2D _cursorTexture;

    private void Awake()
    {
        Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
