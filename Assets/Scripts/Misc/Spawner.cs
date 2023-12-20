using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnable; 
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] [Range(0, 20)] int _spawnquantity;


    private void Awake()
    {
        ActivateObjects();
    }

    private void ActivateObjects()
    {
        int spawnedObjects = 0;
        int spawnPosition = 0;

        while (spawnedObjects < _spawnquantity)
        {
            float randomNum = Random.Range(0f, 1f);
            if (randomNum < 1 / (_spawnquantity * 1.0f) && _spawnPoints[spawnPosition].childCount == 0)
            {
                Instantiate(_spawnable, _spawnPoints[spawnPosition].position, Quaternion.identity, _spawnPoints[spawnPosition]);
                //_spawnPoints[spawnPosition].SetActive(true);
                spawnedObjects++;
                Debug.Log(spawnPosition);
            }
            spawnPosition = (spawnPosition + 1) % _spawnPoints.Length;
        }
    }



}
