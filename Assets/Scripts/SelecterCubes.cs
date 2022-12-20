using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SelecterCubes : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] private Color[] _colors;
    [SerializeField] private float _differenceIntensityColor;
    [SerializeField] private float _differenceIntensityScale;
    [SerializeField] private LayerMask _layerMask;

    private Color _startColor;
    private Color _selectColor;
    private Vector3 _startScale;
    private Vector3 _selectScale;
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
        _startScale = _cube.transform.localScale;

        Color currentColor = _startColor;
        Transform currentTransform = _cube.transform;

        _selectColor = new Color(currentColor.r - _differenceIntensityColor, currentColor.g - _differenceIntensityColor, currentColor.b - _differenceIntensityColor);
        _selectScale = new Vector3(currentTransform.localScale.x - _differenceIntensityScale, currentTransform.localScale.y - _differenceIntensityScale, currentTransform.localScale.z - _differenceIntensityScale);
    }

    private void Update()
    {
        Deselect();
    }

    public void Select(bool isSelected)
    {
        if (isSelected)
            Highlight(_selectColor, _selectScale);
        else
            Deselect();
    }

    public void Deselect()
    {
        Highlight(_startColor, _startScale);
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
        float cubePrice = 10;
        float countScore = 0;

        for (int i = 0; i < _countSelected; i++)
        {
            countScore += cubePrice + i;
        }

        return Mathf.Round(countScore);
    }

    private void Highlight(Color targetColor, Vector3 targetScale)
    {
        _cube.GetComponent<Renderer>().material.color = targetColor;
        _cube.transform.localScale = targetScale;
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
