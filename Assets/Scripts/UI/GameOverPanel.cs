using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _glass;
    [SerializeField] private AudioSource _gameoverSound;
    [SerializeField] private AudioSource _continueSound;
    [SerializeField] private Player _player;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Button _continueButton;

    public void ActivateGameOverPanel()
    {
        ChangePanelsActive(true, false);
        _gameoverSound.Play();
    }

    public void WatchAdsFor—ontinueGame()
    {
        ChangePanelsActive(false, true);
        _continueSound.Play();
        _continueButton.gameObject.SetActive(false);
    }

    public void RestartScene()
    {
        ChangePanelsActive(false, true);
        _continueButton.gameObject.SetActive(true);
        _player.Restart();
    }

    private void ChangePanelsActive(bool isActiveGameOverPanel, bool isActiveGlass)
    {
        _gameOverPanel.SetActive(isActiveGameOverPanel);
        _glass.SetActive(isActiveGlass);
    }
}
