using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelecterCubes : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] Color[] _colors;
    [SerializeField] float _differenceIntensityColor;
    [SerializeField] LayerMask _layerMask;

    private Color _startColor;
    private Color _selectColor;
    private int _colorIndex;
    private int _countSelected;

    public int ColorIndex => _colorIndex;
    public int CountSelected => _countSelected;

    public Color[] Colors => _colors;

    private void Start()
    {
        _colorIndex = Random.Range(0, _colors.Length);
        _cube.GetComponent<Renderer>().material.color = _colors[_colorIndex];

        _startColor = _cube.GetComponent<Renderer>().material.color;
        Color currentColor = _startColor;
        _selectColor = new Color(currentColor.r - _differenceIntensityColor, currentColor.g - _differenceIntensityColor, currentColor.b - _differenceIntensityColor);
    }

    private void Update()
    {
        Deselect();
    }

    public void Select(bool isSelected)
    {
        if (isSelected)
            ChangeColor(_selectColor);
        else
            Deselect();
    }

    public void Deselect()
    {
        ChangeColor(_startColor);
    }

    public List<Ray> SetRays()
    {
        List<Ray> rays = new List<Ray>
        {
            new Ray(transform.position, transform.up),
            new Ray(transform.position, -transform.up),
            new Ray(transform.position, transform.right),
            new Ray(transform.position, -transform.right)
        };

        return rays;
    }

    public void SelectIdentityColorCubes(bool isDestroyed)
    {
        float rayDisnatce = 20f;
        _countSelected = 1;

        List<Ray> rays = SetRays();

        foreach (var ray in rays)
        {
            SelectInRay(Physics.RaycastAll(ray, rayDisnatce, _layerMask), isDestroyed);
        }
    }

    public float GetScore()
    {
        float countScore = 0;

        for (int i = 1; i <= _countSelected; i++)
        {
            countScore += i;
        }

        return Mathf.Round(countScore);
    }

    private void ChangeColor(Color targetColor)
    {
        _cube.GetComponent<Renderer>().material.color = targetColor;
    }

    private void SelectInRay(RaycastHit[] collisions, bool isDestroyed)
    {
        foreach (var collision in collisions)
        {
            int collisionColorIndex = collision.collider.gameObject.GetComponent<SelecterCubes>().ColorIndex;

            if (collisionColorIndex == _colorIndex)
            {
                _countSelected++;

                if (isDestroyed)
                    DectroyColorInRay(collision);
                else
                    SelectColorInRay(collision);
            }
        }
    }

    private void SelectColorInRay(RaycastHit collision)
    {
        collision.collider.gameObject.GetComponent<SelecterCubes>().Select(true);
    }

    private void DectroyColorInRay(RaycastHit collision)
    {
        Destroy(collision.collider.gameObject);
    }
}
