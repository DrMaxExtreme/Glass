using MPUIKIT;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Canvas _canvas;

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

    public void Invoke(float currentMultiplier, int currentCubesDestroyed, int target4CubesDestroyed, int target5CubesDestroyed, int target6CubesDestroyed, int currentMoney, int countExperience, int targetExperience, int currentLevel)
    {
        string countSpawnedText = "+" + _spawner.CurrentSpawnedCubes;
        string defaultCountSelectedText = "0";
        string currentLevelText = "x" + Math.Round(currentMultiplier, 1);

        _textCubesDestroyed.text = Convert.ToString(currentCubesDestroyed);

        _spawned4UpFill.fillAmount = Convert.ToSingle(currentCubesDestroyed) / Convert.ToSingle(target4CubesDestroyed);
        _spawned5UpFill.fillAmount = (Convert.ToSingle(currentCubesDestroyed) - Convert.ToSingle(target4CubesDestroyed)) / (Convert.ToSingle(target5CubesDestroyed) - Convert.ToSingle(target4CubesDestroyed));
        _spawned6UpFill.fillAmount = (Convert.ToSingle(currentCubesDestroyed) - Convert.ToSingle(target5CubesDestroyed)) / (Convert.ToSingle(target6CubesDestroyed) - Convert.ToSingle(target5CubesDestroyed));

        _textCountSelected.text = defaultCountSelectedText;
        _countCubesSpawned.text = countSpawnedText;

        _textMoney.text = Convert.ToString(currentMoney);
        _textCurrentMultiplier.text = currentLevelText;

        _levelFill.fillAmount = Convert.ToSingle(countExperience) / Convert.ToSingle(targetExperience);
        _textCurrentLevel.text = Convert.ToString(currentLevel);
    }

    public void InvokeForGameOverPanel(int currentCubesDestroyed, int bestCurrentCubesDestroyed, int currentMoney, int bestCurrentMoney, int currentExperience, int bestCurrentExperience)
    {
        _canvas.GetComponent<GameOverPanel>().Activate();
        _textCubesDestroyedGameOver.text = Convert.ToString(currentCubesDestroyed);
        _textBestCubesDestroyedGameOver.text = Convert.ToString(bestCurrentCubesDestroyed);
        _textCountMoneyGameOver.text = Convert.ToString(currentMoney);
        _textBestCountMoneyGameOver.text = Convert.ToString(bestCurrentMoney);
        _textCountExperienceGameOver.text = Convert.ToString(currentExperience);
        _textBestCountExperienceGameOver.text = Convert.ToString(bestCurrentExperience);
    }

    public void Restart()
    {
        _spawned4UpFill.fillAmount = 0;
        _spawned5UpFill.fillAmount = 0;
        _spawned6UpFill.fillAmount = 0;
    }

    public void CountSelected(int CountSelected)
    {
        _textCountSelected.text = Convert.ToString(CountSelected);
    }
}
