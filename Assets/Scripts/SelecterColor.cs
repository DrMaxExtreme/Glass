using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecterColor : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] Color[] _colors;
    [SerializeField] float _differenceIntensityColor;

    private Color _startColor;
    private Color _selectColor;

    public Color[] Colors => _colors;

    private void Start()
    {
        int colorIndex = Random.Range(0, _colors.Length);
        _cube.GetComponent<Renderer>().material.color = _colors[colorIndex];

        _startColor = _cube.GetComponent<Renderer>().material.color;
        Color currentColor = _startColor;
        _selectColor = new Color(currentColor.r - _differenceIntensityColor, currentColor.g - _differenceIntensityColor, currentColor.b - _differenceIntensityColor);
    }

    public void Select() 
    { 
        ChangeColor(_selectColor);
    }

    public void Deselect()
    {
        ChangeColor(_startColor);
    }

    private void ChangeColor(Color targetColor)
    {

        _cube.GetComponent<Renderer>().material.color = targetColor;
    }
}
