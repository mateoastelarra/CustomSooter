using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorSpotlight : MonoBehaviour
{
    [SerializeField] private GameObject _spotLightHead;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _maxRotation = 45f;

    private float _currentRotation;
    private bool _shouldRotate = false;

    private Light2D _light2D;

    private void Awake()
    {
        _light2D = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        RandomStartingRotation();
        _light2D.enabled = false;
    }

    private void OnEnable()
    {
        DiscoballManager.OnStartParty += StartRotating;
        DiscoballManager.OnFinishParty += StopRotating;
        DiscoballManager.OnStartParty += TurnOnTheLights;
        DiscoballManager.OnFinishParty += TurnOffTheLights;
    }

    private void OnDisable()
    {
        DiscoballManager.OnFinishParty -= StartRotating;
        DiscoballManager.OnFinishParty -= StopRotating;
        DiscoballManager.OnStartParty -= TurnOnTheLights;
        DiscoballManager.OnFinishParty -= TurnOffTheLights;
    }

    private void Update()
    {
        if (_shouldRotate) { RotateHead(); }   
    }

    private void StartRotating()
    {
        _shouldRotate = true;   
    }

    private void StopRotating()
    {
        _shouldRotate = false;
    }

    private void TurnOnTheLights()
    {
        _light2D.enabled = true;
    }

    private void TurnOffTheLights()
    {
        _light2D.enabled = false;
    }

    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * _rotationSpeed;
        float z = Mathf.PingPong(_currentRotation, _maxRotation);
        _spotLightHead.transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }

    private void RandomStartingRotation()
    {
        float randomStartingRotationZ = Random.Range(-_maxRotation, _maxRotation);
        _spotLightHead.transform.localRotation = Quaternion.Euler(0f, 0f, randomStartingRotationZ);
        _currentRotation = randomStartingRotationZ + _maxRotation;
    }
}
