using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _seceltablePanel;
    [SerializeField] private GameObject _spawnedPanel;
    [SerializeField] private GameObject _barPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _glass;
    [SerializeField] private AudioSource _gameoverSound;
    [SerializeField] private AudioSource _continueSound;

    public void ActivateGameOverPanel()
    {
        ChangePanelsActive(false, false, false, false, true, false);
        _gameoverSound.Play();
    }

    public void WatchAdsFor—ontinueGame()
    {
        ChangePanelsActive(true, true, true, true, false, true);
        _continueSound.Play();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ChangePanelsActive(bool isActiveScorePanel, bool isActiveSeceltablePanel, bool isActiveSpawnedPanel, bool isActiveBarPanel, bool isActiveGameOverPanel, bool isActiveGlass)
    {
        _scorePanel.SetActive(isActiveScorePanel);
        _seceltablePanel.SetActive(isActiveSeceltablePanel);
        _spawnedPanel.SetActive(isActiveSpawnedPanel);
        _barPanel.SetActive(isActiveBarPanel);
        _gameOverPanel.SetActive(isActiveGameOverPanel);
        _glass.SetActive(isActiveGlass);
    }
}
