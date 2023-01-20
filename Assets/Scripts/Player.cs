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
    [SerializeField] private Canvas _canvas;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _spawnedCountUpSound;
    [SerializeField] private AudioSource _levelUpSound;
    [SerializeField] private AudioSource _backgroundMusic;

    [SerializeField] private MPImage _spawned4UpFill;
    [SerializeField] private MPImage _spawned5UpFill;
    [SerializeField] private MPImage _spawned6UpFill;
    [SerializeField] private TMP_Text _countCubesSpawned;
    [SerializeField] private TMP_Text _textCountSelected;
    [SerializeField] private TMP_Text _textCubesDestroyed;

    [SerializeField] private TMP_Text _textMoney;
    [SerializeField] private TMP_Text _textCurrentMultiplier;

    [SerializeField] private MPImage _levelFill;
    [SerializeField] private TMP_Text _textCurrentLevel;

    [SerializeField] private TMP_Text _textCubesDestroyedGameOver;
    [SerializeField] private TMP_Text _textBestCubesDestroyedGameOver;
    [SerializeField] private TMP_Text _textCountMoneyGameOver;
    [SerializeField] private TMP_Text _textBestCountMoneyGameOver;
    [SerializeField] private TMP_Text _textCountExperienceGameOver;
    [SerializeField] private TMP_Text _textBestCounttExperienceGameOver;

    private SelecterCubes _currentSecectable;

    private float _target4CubesDestroyed = 50f;
    private float _target5CubesDestroyed = 150f;
    private float _target6CubesDestroyed = 300f;

    private int _currentCubesDestroyed = 0;
    private float _bestCurrentCubesDestroyed = 0f;

    private float _currentMoney = 0f;
    private float _bestCurrentMoney = 0f;
    private float _currentMultiplier = 1f;
    private float _valueUpMultiplier = 0.1f;

    private float _countExperience = 0f;
    private float _targetExperience = 1000f;
    private float _valueUpTargetExperience = 100f;
    private float _currentLevel = 0f;
    private float _currentExperience = 0f;
    private float _bestCurrentExperience = 0f;

    private void Start()
    {
        UpdateUI();
    }

    private void LateUpdate()
    {
        UpdateUI();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _layerMask))
        {
            SelecterCubes selectable = hit.collider.gameObject.GetComponent<SelecterCubes>();

            if (selectable)
            {
                if (_currentSecectable && _currentSecectable != selectable)
                    _currentSecectable.Deselect();

                _currentSecectable = selectable;
                selectable.Select(true);
                selectable.SelectIdentityColorCubes(false);
                _textCountSelected.text = Convert.ToString(selectable.CountSelected);
            }

            if (selectable && Input.GetMouseButtonDown(0))
            {
                PlaySountClick();
                selectable.SelectIdentityColorCubes(true);
                GetReward(selectable, StartDeley);
                TryUpLevel();
            }
        }
    }

    public void Restart()
    {
        _currentCubesDestroyed = 0;
        _currentMoney = 0f;
        _currentExperience = 0f;
        _spawned4UpFill.fillAmount = 0;
        _spawned5UpFill.fillAmount = 0;
        _spawned6UpFill.fillAmount = 0;
        _spawner.Restart();
    }

    private void TryUpLevel()
    {
        if (_countExperience >= _targetExperience)
        {
            _countExperience -= _targetExperience;
            _targetExperience += _valueUpTargetExperience;
            _currentLevel++;

            _currentMultiplier += _valueUpMultiplier;

            _levelUpSound.Play();
        }
    }

    private void UpdateUI()
    {
        string countSpawnesText = "+" + _spawner.CurrentSpawnedCubes;
        string defaultCountSelectedText = "0";
        string currentLevelText = "x" + Math.Round(_currentMultiplier, 1);

        _textCubesDestroyed.text = Convert.ToString(_currentCubesDestroyed);

        _spawned4UpFill.fillAmount = _currentCubesDestroyed / _target4CubesDestroyed;
        _spawned5UpFill.fillAmount = (_currentCubesDestroyed - _target4CubesDestroyed) / (_target5CubesDestroyed - _target4CubesDestroyed);
        _spawned6UpFill.fillAmount = (_currentCubesDestroyed - _target5CubesDestroyed) / (_target6CubesDestroyed - _target5CubesDestroyed);

        _textCountSelected.text = defaultCountSelectedText;
        _countCubesSpawned.text = countSpawnesText;

        _textMoney.text = Convert.ToString(_currentMoney);
        _textCurrentMultiplier.text = currentLevelText;

        _levelFill.fillAmount = _countExperience / _targetExperience;
        _textCurrentLevel.text = Convert.ToString(_currentLevel);
    }

    private void PlaySountClick()
    {
        _clickSound.Play();
    }

    private void StartDeley()
    {
        StartCoroutine(CheckOverflow());
    }

    private IEnumerator CheckOverflow()
    {
        float delaySeconds = 0.01f;

        yield return new WaitForSeconds(delaySeconds);

        if (_spawner.IsExceededLimitCubesInColumn())
        {
            _canvas.GetComponent<GameOverPanel>().ActivateGameOverPanel();
            _textCubesDestroyedGameOver.text = Convert.ToString(_currentCubesDestroyed);
            _textBestCubesDestroyedGameOver.text = Convert.ToString(_bestCurrentCubesDestroyed);
            _textCountMoneyGameOver.text = Convert.ToString(_currentMoney);
            _textBestCountMoneyGameOver.text = Convert.ToString(_bestCurrentMoney);
            _textCountExperienceGameOver.text = Convert.ToString(_currentExperience);
            _textBestCounttExperienceGameOver.text = Convert.ToString(_bestCurrentExperience);
        }
    }

    private void GetReward(SelecterCubes selectable, Action onComplite)
    {
        float myltiplayerExperience = _spawner.CurrentSpawnedCubes - 2;
        int currentSpawnedcubes3 = 3;
        int currentSpawnedcubes4 = 3;
        int currentSpawnedcubes5 = 3;

        _currentCubesDestroyed += selectable.CountSelected;

        for (int i = 1; i <= selectable.CountSelected; i++)
        {
            _countExperience += i * myltiplayerExperience * myltiplayerExperience;
            _currentExperience += i * _spawner.CurrentSpawnedCubes;
        }

        if (_currentCubesDestroyed >= _target4CubesDestroyed && _spawner.CurrentSpawnedCubes == currentSpawnedcubes3)
            IncreaseSpawnedCount();

        if (_currentCubesDestroyed >= _target5CubesDestroyed && _spawner.CurrentSpawnedCubes == currentSpawnedcubes4)
            IncreaseSpawnedCount();

        if (_currentCubesDestroyed >= _target6CubesDestroyed && _spawner.CurrentSpawnedCubes == currentSpawnedcubes5)
            IncreaseSpawnedCount();

        Destroy(selectable.gameObject);
        _spawner.SpawnCubes();
        _currentMoney += Mathf.Round(selectable.GetScore() * _currentMultiplier);
        onComplite?.Invoke();

        if (_bestCurrentCubesDestroyed < _currentCubesDestroyed)
            _bestCurrentCubesDestroyed = _currentCubesDestroyed;

        if (_bestCurrentMoney < _currentMoney)
            _bestCurrentMoney = _currentMoney;

        if(_bestCurrentExperience < _currentExperience)
            _bestCurrentExperience = _currentExperience;
    }

    private void IncreaseSpawnedCount()
    {
        _spawner.IncreaseSpawnedCount();
        _spawnedCountUpSound.Play();
    }
}
