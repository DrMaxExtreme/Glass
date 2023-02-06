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
    [SerializeField] private TMP_Text _textBestCountExperienceGameOver;

    private SelecterCubes _currentSelecter;

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
        UpdateUI();
        _backgroundMusic.Play();
        _bestCurrentCubesDestroyed = PlayerPrefs.GetInt("RecordCubesDestroyed", 0);
        _bestCurrentMoney = PlayerPrefs.GetInt("RecordMoney", 0);
        _bestCurrentExperience = PlayerPrefs.GetInt("RecordExperience", 0);
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        _countExperience = PlayerPrefs.GetInt("CurrentExperience", 0);
    }

    private void LateUpdate()
    {
        UpdateUI();

        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit, _layerMask))
        {
            SelecterCubes selectable = _hit.collider.gameObject.GetComponent<SelecterCubes>();

            if (selectable && Input.GetMouseButton(0))
            {
                if (_currentSelecter && _currentSelecter != selectable)
                    _currentSelecter.Deselect();

                _currentSelecter = selectable;
                selectable.Select(true);
                selectable.SelectIdentityColorCubes(false);
                _textCountSelected.text = Convert.ToString(selectable.CountSelected);
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

    public void Restart()
    {
        _currentCubesDestroyed = 0;
        _currentMoney = 0;
        _currentExperience = 0;
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
            PlayerPrefs.SetInt("CurrentLevel", _currentLevel);
            _currentMultiplier += _valueUpMultiplier;
            _levelUpSound.Play();
        }
    }

    private void UpdateUI()
    {
        string countSpawnedText = "+" + _spawner.CurrentSpawnedCubes;
        string defaultCountSelectedText = "0";
        string currentLevelText = "x" + Math.Round(_currentMultiplier, 1);

        _textCubesDestroyed.text = Convert.ToString(_currentCubesDestroyed);

        _spawned4UpFill.fillAmount = Convert.ToSingle(_currentCubesDestroyed) / Convert.ToSingle(_target4CubesDestroyed);
        _spawned5UpFill.fillAmount = (Convert.ToSingle(_currentCubesDestroyed) - Convert.ToSingle(_target4CubesDestroyed)) / (Convert.ToSingle(_target5CubesDestroyed) - Convert.ToSingle(_target4CubesDestroyed));
        _spawned6UpFill.fillAmount = (Convert.ToSingle(_currentCubesDestroyed) - Convert.ToSingle(_target5CubesDestroyed)) / (Convert.ToSingle(_target6CubesDestroyed) - Convert.ToSingle(_target5CubesDestroyed));

        _textCountSelected.text = defaultCountSelectedText;
        _countCubesSpawned.text = countSpawnedText;

        _textMoney.text = Convert.ToString(_currentMoney);
        _textCurrentMultiplier.text = currentLevelText;

        _levelFill.fillAmount = Convert.ToSingle(_countExperience) / Convert.ToSingle(_targetExperience);
        _textCurrentLevel.text = Convert.ToString(_currentLevel);
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
            _canvas.GetComponent<GameOverPanel>().ActivateGameOverPanel();
            _textCubesDestroyedGameOver.text = Convert.ToString(_currentCubesDestroyed);
            _textBestCubesDestroyedGameOver.text = Convert.ToString(_bestCurrentCubesDestroyed);
            _textCountMoneyGameOver.text = Convert.ToString(_currentMoney);
            _textBestCountMoneyGameOver.text = Convert.ToString(_bestCurrentMoney);
            _textCountExperienceGameOver.text = Convert.ToString(_currentExperience);
            _textBestCountExperienceGameOver.text = Convert.ToString(_bestCurrentExperience);
        }
    }

    private void GetReward(SelecterCubes selectable, Action onComplite)
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
}
