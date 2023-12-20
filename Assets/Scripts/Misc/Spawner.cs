using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawnable; 
    [SerializeField] private GameObject[] _spawnPoints;
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
            if (randomNum < 1 / (_spawnquantity * 1.0f) && !_spawnPoints[spawnPosition].activeInHierarchy)
            {
                //Instantiate(_spawnable, _spawnPoints[spawnPosition].position, Quaternion.identity, transform);
                _spawnPoints[spawnPosition].SetActive(true);
                spawnedObjects++;
                Debug.Log(spawnPosition);
            }
            spawnPosition = (spawnPosition + 1) % _spawnPoints.Length;
        }
    }



}
