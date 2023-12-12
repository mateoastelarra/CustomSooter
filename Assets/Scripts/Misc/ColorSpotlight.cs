using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSpotlight : MonoBehaviour
{
    [SerializeField] private GameObject _spotLightHead;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _maxRotation = 45f;

    private float _currentRotation;

    private void Start()
    {
        RandomStartingRotation();
    }

    private void Update()
    {
        RotateHead();
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
