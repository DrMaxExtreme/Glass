using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanel;
    [SerializeField] private GameObject _seceltablePanel;
    [SerializeField] private GameObject _gameOverPanel;

    public void ActivebleGameOverPanel()
    {
        _scorePanel.SetActive(false);
        _seceltablePanel.SetActive(false);
        _gameOverPanel.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
}
