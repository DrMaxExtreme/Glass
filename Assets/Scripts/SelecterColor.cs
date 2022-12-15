using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelecterColor : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] Color[] _colors;
    [SerializeField] float _differenceIntensityColor;
    [SerializeField] LayerMask _layerMask;

    private Color _startColor;
    private Color _selectColor;
    private int _colorIndex;

    public int ColorIndex => _colorIndex;

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

    public void SelectIdentityColorCubes()
    {
        float rayDisnatce = 20f;

        List<Ray> rays = SetRays();

        foreach (var ray in rays)
        {
            SelectInRay(Physics.RaycastAll(ray, rayDisnatce, _layerMask));
        }
    }

    private void ChangeColor(Color targetColor)
    {
        _cube.GetComponent<Renderer>().material.color = targetColor;
    }

    private void SelectInRay(RaycastHit[] collisions)
    {
        foreach (var collision in collisions)
        {
            int collisionColorIndex = collision.collider.gameObject.GetComponent<SelecterColor>().ColorIndex;

            if (collisionColorIndex == _colorIndex)
            {
                collision.collider.gameObject.GetComponent<SelecterColor>().Select(true);
            }
        }
    }
}
