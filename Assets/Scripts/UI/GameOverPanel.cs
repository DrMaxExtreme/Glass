using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _glass;
    [SerializeField] private AudioSource _gameoverSound;
    [SerializeField] private AudioSource _continueSound;

    public void ActivateGameOverPanel()
    {
        ChangePanelsActive(true, false);
        _gameoverSound.Play();
    }

    public void WatchAdsFor—ontinueGame()
    {
        ChangePanelsActive(false, true);
        _continueSound.Play();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }

    private void ChangePanelsActive(bool isActiveGameOverPanel, bool isActiveGlass)
    {
        _gameOverPanel.SetActive(isActiveGameOverPanel);
        _glass.SetActive(isActiveGlass);
    }
}
