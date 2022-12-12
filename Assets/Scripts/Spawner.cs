using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _cubePrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    private float _secondsBetweenSpawn = 1;
    private float _elapcedTime = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        _elapcedTime += Time.deltaTime;

        if(_elapcedTime >= _secondsBetweenSpawn)
        {
            _elapcedTime = 0;

            int spawnPointIndex = Random.Range(0, _spawnPoints.Length);
            int cubePrefabIndex = Random.Range(0, _spawnPoints.Length - 1);
            Instantiate(_cubePrefabs[cubePrefabIndex], _spawnPoints[spawnPointIndex]);
        }
    }
}
