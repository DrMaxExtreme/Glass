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

    public void SelectIdentityColorCubes()
    {
        float rayDisnatce = 20f;

        Ray rayUp = new Ray(transform.position, transform.up);
        Ray rayDown = new Ray(transform.position, -transform.up);
        Ray rayRigth = new Ray(transform.position, transform.right);
        Ray rayLeft = new Ray(transform.position, -transform.right);

        SelectInRay(Physics.RaycastAll(rayUp, rayDisnatce, _layerMask));
        SelectInRay(Physics.RaycastAll(rayDown, rayDisnatce, _layerMask));
        SelectInRay(Physics.RaycastAll(rayRigth, rayDisnatce, _layerMask));
        SelectInRay(Physics.RaycastAll(rayLeft, rayDisnatce, _layerMask));
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
