using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    public bool IsImmune => _isImmune;

    public static Action OnPlayerHit;

    [SerializeField] private float _immunityTime = 1f;

    private bool _isImmune;
    private Flash _flash;

    private void Awake()
    {
        _flash = GetComponent<Flash>();
    }

    private void Start()
    {
        StartCoroutine(HitImmunityRoutine());
        StartCoroutine(BlinkingRoutine());
    }

    private void OnEnable()
    {
        OnPlayerHit += HitImmunity;
        OnPlayerHit += Blinking;
    }

    private void OnDisable()
    {
        OnPlayerHit -= HitImmunity;
        OnPlayerHit -= Blinking;
    }

    private void OnDestroy()
    {
        GameOverManager gameOverManager = FindFirstObjectByType<GameOverManager>();
        gameOverManager?.FadeOut();
    }

    void HitImmunity()
    {
        StartCoroutine(HitImmunityRoutine());
    }

    IEnumerator HitImmunityRoutine()
    {
        _isImmune = true;
        yield return new WaitForSeconds(_immunityTime);
        _isImmune = false;
    }

    void Blinking()
    {
        StartCoroutine(BlinkingRoutine());
    }

    IEnumerator BlinkingRoutine()
    {
        float flashTime = _flash.FlashTime;
        float blinkTime = flashTime * 2; //flash time and normal sprite time are the same

        while (blinkTime < _immunityTime)
        {
            yield return new WaitForSeconds(flashTime * 2);
            _flash.StartFlash();
            blinkTime += flashTime * 2;
        }
        
    }
}
