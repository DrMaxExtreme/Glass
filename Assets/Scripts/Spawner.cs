using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private int _startSpawnedCubesCount;
    [SerializeField] private int _maxSpawnedCubesCount;
    [SerializeField] private int _startCubesCount;
    [SerializeField] private LayerMask _layerMask;

    private List<int> _pointsIndex = new List<int>();

    private int _currentSpawnedCubes; 

    public int CurrentSpawnedCubes => _currentSpawnedCubes;
    public int MaxSpawnedCubesCount => _maxSpawnedCubesCount;

    private void Start()
    {
        StartSpawnCubes();
        _currentSpawnedCubes = _startSpawnedCubesCount;
    }

    public bool IsExceededLimitCubesInColumn()
    {
        float rayDisnatce = 20f;
        float startRayYPosition = 1f;
        int maxCubesCount = 10;
        int countEmptyColumn = 0;

        foreach (var spawnPoint in _spawnPoints)
        {
            Vector3 startRayPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y + startRayYPosition, spawnPoint.position.z);

            Ray ray = new Ray(startRayPosition, -transform.up);

            if (Physics.RaycastAll(ray, rayDisnatce, _layerMask).Count() > maxCubesCount)
                return true;

            if (Physics.RaycastAll(ray, rayDisnatce, _layerMask).Count() == 0)
                countEmptyColumn++;
        }

        if (countEmptyColumn == _spawnPoints.Count())
            SpawnCubes();

        return false;
    }

    public void SpawnCubes()
    {
        int pointIndex;

        while(_pointsIndex.Count < _currentSpawnedCubes)
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

    public void IncreaseSpawnedCount()
    {
        _currentSpawnedCubes ++;
    }

    public void Restart()
    {
        foreach (var spawnPoint in _spawnPoints)
        {
            float rayDisnatce = 100f;
            float offsetPositionY = 10f;

            Vector3 rayPosition = spawnPoint.transform.position;
            rayPosition.y += offsetPositionY;

            Ray ray = new Ray(rayPosition, -transform.up);
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDisnatce, _layerMask);

            foreach (var hit in hits)
                Destroy(hit.collider.gameObject);
        }

        _currentSpawnedCubes = _startSpawnedCubesCount;
        StartSpawnCubes();
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
