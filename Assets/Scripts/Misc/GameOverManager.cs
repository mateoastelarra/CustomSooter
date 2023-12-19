using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class GameOverManager: MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeTime = 1.5f;
    [SerializeField] private float _deactivateSpawnPointTime = 3f;

    private SpriteRenderer _respawnPointSR;

    private void Awake()
    {
        _respawnPointSR = _respawnPoint.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(DeactivateRespawnPoint());
    }

    public void FadeInAndOut()
    {
        StartCoroutine(FadeOut());
    }

    private void FadeIn()
    {
        StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeOut()
    {
        yield return StartCoroutine(FadeRoutine(1f));
        ActivateRespawnPoint();
        RespawnPlayer();
        FadeIn();
        StartCoroutine(DeactivateRespawnPoint());
    }

    private IEnumerator FadeRoutine(float targetAlpha, Sprite sprite = null)
    {
        float elapsedTime = 0;
        float startValue = _image.color.a;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetAlpha, elapsedTime / _fadeTime);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);
            yield return null;
        }
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, targetAlpha);
    }

    private void RespawnPlayer()
    {
        GameObject player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity);
        _virtualCamera.Follow = player.transform;
    }

    private IEnumerator DeactivateRespawnPoint()
    {
        float elapsedTime = 0;
        float startValue = _respawnPointSR.color.a;

        while (elapsedTime < _deactivateSpawnPointTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, 0, elapsedTime / _fadeTime);
            _respawnPointSR.color = new Color(_respawnPointSR.color.r,
                                              _respawnPointSR.color.g,
                                              _respawnPointSR.color.b,
                                              newAlpha);
            yield return null;
        }
        _respawnPointSR.color = new Color(_respawnPointSR.color.r,
                                              _respawnPointSR.color.g,
                                              _respawnPointSR.color.b, 0);
        _respawnPoint.gameObject.SetActive(false);
    }

    private void ActivateRespawnPoint()
    {
        _respawnPointSR.color = new Color(_respawnPointSR.color.r,
                                          _respawnPointSR.color.g,
                                          _respawnPointSR.color.b, 1);
        _respawnPoint.gameObject.SetActive(true);
    }

}
