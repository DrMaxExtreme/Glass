using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _spawnedCubesCount;
    [SerializeField] private int _startCubesCount;
    [SerializeField] private LayerMask _layerMask;

    private List<int> _pointsIndex = new List<int>();

    public int SpawnedCubesCount => _spawnedCubesCount;

    private void Start()
    {
        StartSpawnCubes();
    }

    public bool IsExceededLimitCubesInColumn()
    {
        float rayDisnatce = 20f;
        int maxCubesCount = 10;
        float startRayYPosition = 1;

        foreach (var spawnPoint in _spawnPoints)
        {
            Vector3 startRayPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + startRayYPosition, spawnPoint.position.z);

            Ray ray = new Ray(startRayPosition, -transform.up);
            Debug.DrawRay(startRayPosition, -transform.up, Color.magenta);

            if (Physics.RaycastAll(ray, rayDisnatce, _layerMask).Count() > maxCubesCount)
                return true;
        }

        return false;
    }

    public void SpawnCubes()
    {
        int pointIndex;

        while(_pointsIndex.Count < _spawnedCubesCount)
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

    public void IncreasedSpawnedCount()
    {
        int maxCountSpawnrdCubes = 6;

        if(_spawnedCubesCount < maxCountSpawnrdCubes)
            _spawnedCubesCount++;
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
