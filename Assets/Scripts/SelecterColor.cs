using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelecterColor : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] Color[] _colors;

    public Color[] Colors => _colors;

    void Start()
    {
        int colorIndex = Random.Range(0, _colors.Length);

        _cube.GetComponent<Renderer>().material.color = _colors[colorIndex];
    }
}
