using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _glass;
    [SerializeField] private TMP_Text _textCountSelected;
    [SerializeField] private TMP_Text _textScore;
    [SerializeField] private TMP_Text _textScoreGameOver;
    [SerializeField] private Canvas _canvas;

    private SelecterCubes _currentSecectable;
    private float _score = 0;

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
                selectable.SelectIdentityColorCubes(true);
                SetScore(selectable, StartDeley);
            }
        }
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
        Destroy(selectable.gameObject);
        _spawner.SpawnCubes();
        _score += selectable.GetScore();
        onComplite?.Invoke();
    }
}
