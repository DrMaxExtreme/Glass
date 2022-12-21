using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementDown : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private float _speed = 0.5f;
    private float _distance = 1f;
    private bool _isMove = false;
    private Vector3 _targetPosition;

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        if(Physics.Raycast(ray, _distance, _layerMask) == false && _isMove == false)
        {
            _targetPosition = new Vector3(transform.position.x, transform.position.y - _distance, transform.position.z);
            _isMove = true;
        }
            
        if(_isMove)
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed);

        if (transform.position == _targetPosition)
            _isMove = false;
    }
}
