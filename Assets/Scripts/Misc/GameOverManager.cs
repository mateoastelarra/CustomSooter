using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameOverManager: MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeTime = 1.5f;
    [SerializeField] private float _deactivateSpawnPointTime = 3f;

    private SpriteRenderer _respawnPointSR;
    private Spawner _spawner;
    private Transform _respawnPoint;

    private void Awake()
    {
        _image.gameObject.SetActive(true);
        _spawner = GetComponent<Spawner>();   
    }

    private void Start()
    {
        GetReSpawnPoint();
        PlayerController.Instance.transform.position = _respawnPoint.transform.position;
        _respawnPointSR = _respawnPoint.GetComponentInChildren<SpriteRenderer>();
        FadeIn();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private void FadeIn()
    {
        ActivateRespawnPoint();
        StartCoroutine(FadeRoutine(0f));
        StartCoroutine(DeactivateRespawnPoint());   
    }

    private IEnumerator FadeOutRoutine()
    {
        yield return StartCoroutine(FadeRoutine(1f));
        ReloadScene();
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

    private void GetReSpawnPoint()
    {
        Transform[] spawnPoints = _spawner.SpawnPoints;
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].childCount != 0)
            {
                _respawnPoint = spawnPoints[i];
                return;
            }
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
