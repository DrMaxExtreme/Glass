using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _stepCubesCount;
    [SerializeField] private int _startCubesCount;

    private float _secondsBetweenSpawn = 1;
    private float _elapcedTime = 0;
    private List<int> _pointsIndex = new List<int>();

    private void Start()
    {
        StartSpawnCubes();
    }

    private void Update()
    {
        _elapcedTime += Time.deltaTime;

        if(_elapcedTime >= _secondsBetweenSpawn)
        {
            _elapcedTime = 0;
            SpawnCubes();
        }
    }

    private void SpawnCubes()
    {
        int pointIndex;

        while(_pointsIndex.Count < _stepCubesCount)
        {
            pointIndex = Random.Range(0, _spawnPoints.Length);

            var result = _pointsIndex.Where(index => index == pointIndex);
            
            if(result.Count() == 0)
                _pointsIndex.Add(pointIndex);
        }

        foreach (var index in _pointsIndex)
        {
            Instantiate(_cubePrefab, _spawnPoints[index].transform);
        }

        _pointsIndex.Clear();
    }

    private void StartSpawnCubes()
    {
        int index = 0;
        int upPositionY = 0;

        for (int i = 0; i < _startCubesCount; i++)
        {
            if (index == _spawnPoints.Length)
            {
                index = 0;
                upPositionY++;
            }

            Vector3 spawnPosition = _spawnPoints[index].position;
            spawnPosition.y += upPositionY;

            Instantiate(_cubePrefab, spawnPosition, Quaternion.identity);

            index++;
        }
    }
}
