using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _glass;
    [SerializeField] private TMP_Text _textCountSelected;
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _textScoreGameOver;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private AudioSource _clickSound;

    private SelecterCubes _currentSecectable;
    private float _score = 0;

    private void Start()
    {
        ActivateMusic();
    }

    private void LateUpdate()
    {
        string defaultCountSelectedText = "0";
        _textCountSelected.text = defaultCountSelectedText;
        _textScore.text = Convert.ToString(_score);
        _textScoreGameOver.text = Convert.ToString(_score);

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

    private void PlaySountClick()
    {
        float minPitch = 0.9f;
        float maxPitch = 1.1f;

        _clickSound.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
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
            _glass.SetActive(false);
            _canvas.GetComponent<GameOverPanel>().ActivebleGameOverPanel();
        }
    }

    private void SetScore(SelecterCubes selectable, Action onComplite)
    {
        ActivateAudioOnClickCube();
        Destroy(selectable.gameObject);
        _spawner.SpawnCubes();
        _score += selectable.GetScore();
        onComplite?.Invoke();
    }

    public void ActivateAudioOnClickCube() { }

    public void ActivateMusic() { }
}
