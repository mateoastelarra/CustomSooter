using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] SpawnPoints => _spawnPoints;

    [SerializeField] private GameObject[] _spawnables; 
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] [Range(0, 20)] int _spawnquantity;
    [SerializeField] private bool _shouldSpawnExtra;


    private void Awake()
    {
        SpawnObjectsAtStart();
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreThreshold += SpawnExtraObject; 
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreThreshold -= SpawnExtraObject;
    }

    private void SpawnObjectsAtStart()
    {
        SpawnObjects(_spawnquantity);
    }

    private void SpawnExtraObject()
    {
        if (!_shouldSpawnExtra) { return; }
        SpawnObjects(1);
    }

    private void SpawnObjects(int quantity)
    {
        int spawnedObjects = 0;
        int spawnPosition = 0;

        while (spawnedObjects < quantity)
        {
            float randomNum = Random.Range(0f, 1f);
            if (randomNum < 1 / (_spawnPoints.Length * 1.0f) && _spawnPoints[spawnPosition].childCount == 0)
            {
                int spawnableIndex = Random.Range(0, _spawnables.Length);
                Instantiate(_spawnables[spawnableIndex],
                            _spawnPoints[spawnPosition].position,
                            Quaternion.identity,
                            _spawnPoints[spawnPosition]);
                spawnedObjects++;
            }
            spawnPosition = (spawnPosition + 1) % _spawnPoints.Length;
        }
    }



}
