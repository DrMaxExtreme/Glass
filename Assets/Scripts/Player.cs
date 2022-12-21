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
    [SerializeField] private TMP_Text _textCountSelected;
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _textScoreGameOver;
    [SerializeField] private TMP_Text _textCurrentLevel;
    [SerializeField] private TMP_Text _textTargetScore;
    [SerializeField] private TMP_Text _countSpawned;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private AudioSource _clickSound;
    [SerializeField] private AudioSource _spawnedCountUpSound;
    [SerializeField] private AudioSource _levelUpSound;
    [SerializeField] private MPImage _scoreFill;

    private SelecterCubes _currentSecectable;
    private float _score = 0;
    private float _currentLevel = 1f;
    private float _currentLevelMultiplier = 0.2f;
    private float _targetScore = 250f;
    private float _oldTargetScore = 0f;
    private float _currentDifficulty = 1f;
    private float _differenceNewTargetScore = 250f;
    private float _difficultyMultiplier = 0.3f;

    private void Start()
    {
        ActivateMusic();
        UpdateTexts();
    }

    private void LateUpdate()
    {
        UpdateTexts();
        TryUpLevel();

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
                SetScore(selectable, StartDeley);
            }
        }
    }

    private void TryUpLevel()
    {
        float dividerLevel = 0.8f;

        if (_score >= _targetScore)
        {
            _oldTargetScore = _targetScore;
            _currentDifficulty += _difficultyMultiplier;
            _targetScore += _differenceNewTargetScore * _currentDifficulty;
            _currentLevel += _currentLevelMultiplier;
            _levelUpSound.Play();

            if (Math.Round(_currentLevel % dividerLevel, 1) == 0)
            {
                _spawner.IncreasedSpawnedCount();
                _spawnedCountUpSound.Play();
            }
        }
    }

    private void UpdateTexts()
    {
        string defaultCountSelectedText = "0";
        string currentLevelText = "x" + Math.Round(_currentLevel, 1);

        _textCountSelected.text = defaultCountSelectedText;
        _textCurrentLevel.text = currentLevelText;
        _textScore.text = Convert.ToString(_score);
        _textTargetScore.text = Convert.ToString(Math.Round(_targetScore));
        _scoreFill.fillAmount = (_score - _oldTargetScore) / (_targetScore - _oldTargetScore);
        _countSpawned.text = Convert.ToString(_spawner.SpawnedCubesCount);
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
            _textScoreGameOver.text = Convert.ToString(_score);
        }
    }

    private void SetScore(SelecterCubes selectable, Action onComplite)
    {
        ActivateAudioOnClickCube();
        Destroy(selectable.gameObject);
        _spawner.SpawnCubes();
        _score += Mathf.Round(selectable.GetScore() * _currentLevel);
        onComplite?.Invoke();
    }

    public void ActivateAudioOnClickCube() { }

    public void ActivateMusic() { }
}
