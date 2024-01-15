using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    private bool _playerOnPlatform = false;
    private float _disableColliderTime = 1f;
    private float _controllerThreshold = -0.9f; // for left stick in controllers
    private Collider2D _platformCollider;

    Coroutine _disablePlatformCollider;

    private void Awake()
    {
        _platformCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        DetectPlayerInput();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>())
        {
            _playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            _playerOnPlatform = false;
        }
    }

    private void DetectPlayerInput()
    {
        if (!_playerOnPlatform) { return; }
        if(_disablePlatformCollider != null) { return; }

        if (PlayerController.Instance.MoveInput.y < _controllerThreshold)
        {
            _disablePlatformCollider = StartCoroutine(DisablePlatformCollider());
        }
    }

    private IEnumerator DisablePlatformCollider()
    {
        Collider2D[] playerColliders = PlayerController.Instance.GetComponents<Collider2D>();

        foreach(Collider2D playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _platformCollider, true);
        }

        yield return new WaitForSeconds(_disableColliderTime);

        foreach (Collider2D playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _platformCollider, false);
        }
        _disablePlatformCollider = null;
    }
}
