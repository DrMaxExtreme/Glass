using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementDown : MonoBehaviour
{
    private float _speed = 0.1f;
    private float _distance = 1f;
    private bool _isMove = false;
    private Vector3 _targetPosition;

    private void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        Debug.DrawRay(transform.position, -transform.up);

        if(Physics.Raycast(ray, _distance) == false && _isMove == false)
        {
            _targetPosition = new Vector3(transform.position.x, transform.position.y - _distance, transform.position.z);
            _isMove = true;
            Debug.Log("True");
        }
            
        if(_isMove)
            MoveDown(_targetPosition);

        if (transform.position == _targetPosition)
            _isMove = false;
    }

    private void MoveDown(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed);
        Debug.Log("Move");
    }
}
