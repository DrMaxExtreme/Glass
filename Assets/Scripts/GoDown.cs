using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDown : MonoBehaviour
{
    private float _speed = 0.0001f;
    private Transform _target;

    private void Start()
    {
        _target = transform;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) //if hit == null => Vector3.MoveTowards(transform.position, transform.position.y - 1, _speed);
            _target.position = hit.point;

        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed);

        Debug.DrawRay(transform.position, _target.position * 12, Color.red);
    }
}
