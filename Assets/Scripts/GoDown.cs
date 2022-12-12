using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDown : MonoBehaviour
{
    private float _speed = 0.1f;
    private Transform _target;

    private void Start()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
            _target.position = hit.point; //в _target.position ничего не сохраняется
    }

    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed);

        Debug.DrawRay(transform.position, -transform.up * 10, Color.yellow);
        Debug.DrawRay(transform.position, _target.position * 10, Color.red);
    }
}
