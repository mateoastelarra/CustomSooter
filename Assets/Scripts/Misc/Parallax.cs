using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float _parallaxOffset = -0.1f;

    private Camera _mainCamera;
    private Vector2 _startPos;

    private Vector2 _travel => (Vector2) _mainCamera.transform.position - _startPos;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _startPos = transform.position;
    }

    private void FixedUpdate()
    {
        float newPositionX = _startPos.x + _travel.x * _parallaxOffset;
        transform.position = new Vector2(newPositionX, transform.position.y); 
    }
}
