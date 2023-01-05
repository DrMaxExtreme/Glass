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

    [SerializeField] private TMP_Text _countSpawned;
    [SerializeField] private MPImage _spawnedUpFill;

    [SerializeField] private TMP_Text _textCountSelected;
    [SerializeField] private TMP_Text _textCubesDestroyed;

    [SerializeField] private TMP_Text _textMoney;
    [SerializeField] private TMP_Text _textCurrentMultiplier;

    [SerializeField] private MPImage _levelFill;
    [SerializeField] private TMP_Text _currentLevet;

    [SerializeField] private TMP_Text _textMoneyGameOver;

    private SelecterCubes _currentSecectable;
    private float _money = 0;
    private float _currentMultiplier = 1f;
    private float _currentUpMultiplier = 0.2f;
    private float _targetMoney = 250f;
    private float _currentCubesDestroyed = 0;
    private float _currentDifficulty = 1f;
    private float _differenceNewTargetScore = 250f;
    private float _difficultyMultiplier = 0.3f; //добавить уровень и опыт (опыт = сколько всего кубов уничтожено)

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

    private void TryUpLevel() //переделать под уровень и опыт
    {
        float dividerLevel = 0.8f;

        if (_money >= _targetMoney)
        {
            _currentDifficulty += _difficultyMultiplier;
            _targetMoney += _differenceNewTargetScore * _currentDifficulty;
            _currentMultiplier += _currentUpMultiplier;
            _levelUpSound.Play();

            if (Math.Round(_currentMultiplier % dividerLevel, 1) == 0)
            {
                _spawner.IncreasedSpawnedCount();
                _spawnedCountUpSound.Play();
            }
        }
    }

    private void UpdateTexts()
    {
        string defaultCountSelectedText = "0";
        string currentLevelText = "x" + Math.Round(_currentMultiplier, 1);

        _countSpawned.text = Convert.ToString(_spawner.SpawnedCubesCount);
        //_spawnedUpFill.fillAmount = (_money - _oldTargetMoney) / (_targetMoney - _oldTargetMoney);

        _textCountSelected.text = defaultCountSelectedText;
        _textCubesDestroyed.text = Convert.ToString(_currentCubesDestroyed);

        _textMoney.text = Convert.ToString(_money);
        _textCurrentMultiplier.text = currentLevelText;

        //_levelFill.fillAmount = 
        _currentLevet.text = Convert.ToString(_currentLevet);
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
            _textMoneyGameOver.text = Convert.ToString(_money);
        }
    }

    private void SetScore(SelecterCubes selectable, Action onComplite)
    {
        ActivateAudioOnClickCube();
        Destroy(selectable.gameObject);
        _spawner.SpawnCubes();
        _money += Mathf.Round(selectable.GetScore() * _currentMultiplier);
        onComplite?.Invoke();
    }

    public void ActivateAudioOnClickCube() { }

    public void ActivateMusic() { }
}
