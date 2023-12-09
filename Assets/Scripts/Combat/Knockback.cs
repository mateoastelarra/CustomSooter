using UnityEngine;
using System;
using System.Collections;

public class Knockback : MonoBehaviour
{
    public Action OnKnockBackStart;
    public Action OnKnockBackEnd;

    [SerializeField] private float _knockBackTime = 0.2f;

    private Vector3 _hitDirection;
    private float _knockBackThrust;

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        OnKnockBackStart += ApplyKnockBackForce;
        OnKnockBackEnd += StopKnockBack;
    }


    private void OnDisable()
    {
        OnKnockBackStart -= ApplyKnockBackForce;
        OnKnockBackEnd -= StopKnockBack;
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockBackThrust)
    {
        Debug.Log("knock");
        _hitDirection = hitDirection;
        _knockBackThrust = knockBackThrust;

        OnKnockBackStart?.Invoke();
    }

    private void ApplyKnockBackForce()
    {
        Vector3 difference = (transform.position - _hitDirection).normalized ;
        _rigidBody.AddForce(difference * _knockBackThrust * _rigidBody.mass, ForceMode2D.Impulse);
        StartCoroutine(Knock());
    }

    private IEnumerator Knock()
    {
        yield return new WaitForSeconds(_knockBackTime);
        OnKnockBackEnd?.Invoke();
    }

    private void StopKnockBack()
    {
        _rigidBody.velocity = Vector2.zero;
    }
}
