using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 3f;

    private float _moveX;
    private bool _canMove = true;

    private Rigidbody2D _rigidBody;
    private Knockback _knockback;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
    }

    private void OnEnable()
    {
        _knockback.OnKnockBackStart += DisableMovement;
        _knockback.OnKnockBackEnd += EnableMovement;
    }

    private void OnDisable()
    {
        _knockback.OnKnockBackStart -= DisableMovement;
        _knockback.OnKnockBackEnd -= EnableMovement;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetCurrentDirection(float currentXDirection)
    {
        _moveX = currentXDirection;
    }

    private void Move()
    {
        if (!_canMove) { return; }
        Vector2 movement = new Vector2(_moveX * _moveSpeed, _rigidBody.velocity.y);
        _rigidBody.velocity = movement;
    }

    private void EnableMovement()
    {
        _canMove = true;
    }

    private void DisableMovement()
    {
        _canMove = false;
    }
}
