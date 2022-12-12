using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoDown : MonoBehaviour
{
    private float _distance = 0.58f;
    private float _speed;
    
    void Update()
    {
        float speed = 3;

        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, _distance))
            _speed = 0;
        else
            _speed = speed;

        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }
}
