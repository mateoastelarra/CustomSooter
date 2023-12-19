using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Fade : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] Transform _respawnPoint;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] float _fadeTime = 1.5f;
    [SerializeField] float _restartTime = 3;

    private Image _image;
    

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeRoutine(0f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeRoutine(1f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
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

    public void RestartGame()
    {
        StartCoroutine(RestartGameRoutine());
    }

    IEnumerator RestartGameRoutine()
    {
        yield return new WaitForSeconds(_restartTime);
        GameObject player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity);
        _virtualCamera.Follow = player.transform;
        FadeIn();  
    }

}
