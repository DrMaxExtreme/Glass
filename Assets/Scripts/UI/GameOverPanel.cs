using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Agava.YandexGames;
using Agava.YandexMetrica;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _glass;
    [SerializeField] private AudioSource _gameOverSound;
    [SerializeField] private AudioSource _continueSound;
    [SerializeField] private Player _player;
    [SerializeField] private Button _continueButton;
    [SerializeField] private DestroyerLine[] _destroyerLines;

    private float _normalTimeScale;
    
    private void Start()
    {
        _normalTimeScale = Time.timeScale;
    }

    public void Activate()
    {
        SwichGameOver(true, false);
        _gameOverSound.Play();
    }

    private void Reward()
    {
        SwichGameOver(false, true);
        _continueSound.Play();
        _continueButton.gameObject.SetActive(false);

        foreach (var destroyerLine in _destroyerLines)
        {
            destroyerLine.Destroy();
        }
    }

    public void RestartScene()
    {
        SwichGameOver(false, true);
        _continueButton.gameObject.SetActive(true);
        _player.Restart();
        InterstitialAd.Show();
        YandexMetrica.Send("pageOpen");
    }

    public void ShowAd()
    {
        VideoAd.Show(onOpenCallback:Pause, onRewardedCallback:Reward, onCloseCallback:Continue);
    }
    
    private void SwichGameOver(bool isActiveGameOverPanel, bool isActiveGlass)
    {
        _gameOverPanel.SetActive(isActiveGameOverPanel);
        _glass.SetActive(isActiveGlass);
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Continue()
    {
        Time.timeScale = _normalTimeScale;
    }
}
