using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Events;
using MPUIKIT;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private UIUpdater _UIUpdater;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _spawnedCountUpSound;
    [SerializeField] private AudioSource _levelUpSound;
    [SerializeField] private AudioSource _backgroundMusic;

    private SelectorCubes _currentSelecter;

    private int _target4CubesDestroyed = 50;
    private int _target5CubesDestroyed = 150;
    private int _target6CubesDestroyed = 300;

    private int _currentCubesDestroyed = 0;
    private int _bestCurrentCubesDestroyed = 0;

    private int _currentMoney = 0;
    private int _bestCurrentMoney = 0;
    private float _currentMultiplier = 1f;
    private float _valueUpMultiplier = 0.1f;

    private int _countExperience = 0;
    private int _targetExperience = 1000;
    private int _valueUpTargetExperience = 100;
    private int _currentLevel = 0;
    private int _currentExperience = 0;
    private int _bestCurrentExperience = 0;

    private Ray _ray;
    private RaycastHit _hit;

    private void Start()
    {
        _UIUpdater.Invoke(_currentMultiplier, _currentCubesDestroyed, _target4CubesDestroyed, _target5CubesDestroyed, _target6CubesDestroyed, _currentMoney, _countExperience, _targetExperience, _currentLevel);
        _backgroundMusic.Play();
        GetPrefs();
    }

    private void LateUpdate()
    {
        _UIUpdater.Invoke(_currentMultiplier, _currentCubesDestroyed, _target4CubesDestroyed, _target5CubesDestroyed, _target6CubesDestroyed, _currentMoney, _countExperience, _targetExperience, _currentLevel);
        SetRay();
    }

    public void Restart()
    {
        _currentCubesDestroyed = 0;
        _currentMoney = 0;
        _currentExperience = 0;
        _UIUpdater.Restart();
        _spawner.Restart();
    }

    private void SetRay()
    {
        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit, _layerMask))
        {
            SelectorCubes selectable = _hit.collider.gameObject.GetComponent<SelectorCubes>();

            if (selectable && Input.GetMouseButton(0))
            {
                if (_currentSelecter && _currentSelecter != selectable)
                    _currentSelecter.Deselect();

                _currentSelecter = selectable;
                selectable.Select(true);
                selectable.SelectIdentityColorCubes(false);
                _UIUpdater.CountSelected(selectable.CountSelected);
            }

            if (selectable && Input.GetMouseButtonUp(0))
            {
                PlaySoundClick();
                selectable.SelectIdentityColorCubes(true);
                GetReward(selectable, StartDelay);
                TryUpLevel();
            }
        }
    }

    private void TryUpLevel()
    {
        if (_countExperience >= _targetExperience)
        {
            _countExperience -= _targetExperience;
            _targetExperience += _valueUpTargetExperience;
            _currentLevel++;
            PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
            _currentMultiplier += _valueUpMultiplier;
            PlayerPrefs.SetFloat("CurrentMultiplier", _currentMultiplier);
            _levelUpSound.Play();
        }
    }

    private void PlaySoundClick()
    {
        _clickSound.Play();
    }

    private void StartDelay()
    {
        StartCoroutine(CheckOverflow());
    }

    private IEnumerator CheckOverflow()
    {
        float delaySeconds = 0.01f;

        yield return new WaitForSeconds(delaySeconds);

        if (_spawner.IsExceededLimitCubesInColumn())
        {
            _UIUpdater.InvokeForGameOverPanel(_currentCubesDestroyed, _bestCurrentCubesDestroyed, _currentMoney, _bestCurrentMoney, _currentExperience, _bestCurrentExperience);
        }
    }

    private void GetReward(SelectorCubes selectable, Action onComplite)
    {
        float myltiplayerExperience = _spawner.CurrentSpawnedCubes - 2;
        int currentSpawnedcubes3 = 3;
        int currentSpawnedcubes4 = 4;
        int currentSpawnedcubes5 = 5;

        _currentCubesDestroyed += selectable.CountSelected;

        for (int i = 1; i <= selectable.CountSelected; i++)
        {
            _countExperience += Convert.ToInt32(Mathf.Round(i * myltiplayerExperience * myltiplayerExperience));
            _currentExperience += i * _spawner.CurrentSpawnedCubes;
            PlayerPrefs.SetInt("CurrentExperience", _countExperience);
        }

        if (_currentCubesDestroyed >= _target4CubesDestroyed && _spawner.CurrentSpawnedCubes == currentSpawnedcubes3)
            IncreaseSpawnedCount();

        if (_currentCubesDestroyed >= _target5CubesDestroyed && _spawner.CurrentSpawnedCubes == currentSpawnedcubes4)
            IncreaseSpawnedCount();

        if (_currentCubesDestroyed >= _target6CubesDestroyed && _spawner.CurrentSpawnedCubes == currentSpawnedcubes5)
            IncreaseSpawnedCount();

        Destroy(selectable.gameObject);
        _spawner.SpawnCubes();
        _currentMoney += Convert.ToInt32(Mathf.Round(selectable.GetScore() * _currentMultiplier));
        onComplite?.Invoke();

        if (_bestCurrentCubesDestroyed < _currentCubesDestroyed)
        {
            _bestCurrentCubesDestroyed = _currentCubesDestroyed;
            PlayerPrefs.SetInt("RecordCubesDestroyed", _currentCubesDestroyed);
        }

        if (_bestCurrentMoney < _currentMoney)
        {
            _bestCurrentMoney = _currentMoney;
            PlayerPrefs.SetInt("RecordMoney", _currentMoney);
        }

        if (_bestCurrentExperience < _currentExperience)
        {
            _bestCurrentExperience = _currentExperience;
            PlayerPrefs.SetInt("RecordExperience", _currentExperience);
        }
    }

    private void IncreaseSpawnedCount()
    {
        _spawner.IncreaseSpawnedCount();
        _spawnedCountUpSound.Play();
    }

    private void GetPrefs()
    {
        _bestCurrentCubesDestroyed = PlayerPrefs.GetInt("RecordCubesDestroyed", 0);
        _bestCurrentMoney = PlayerPrefs.GetInt("RecordMoney", 0);
        _bestCurrentExperience = PlayerPrefs.GetInt("RecordExperience", 0);
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        _countExperience = PlayerPrefs.GetInt("CurrentExperience", 0);
        _currentMultiplier = PlayerPrefs.GetFloat("CurrentMultiplier", _currentMultiplier);
    }
}
