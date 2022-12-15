using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    private float _animationSpeed;
    private Quaternion _originalRotation;
    private float _angle;

    private void Start()
    {
        float minSpeedAnimation = 0.5f;
        float maxSpeedAnimation = 1.5f;

        transform.rotation = Random.rotation;
        _animationSpeed = Random.Range(minSpeedAnimation, maxSpeedAnimation);
        _originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        _angle++;

        Quaternion quaternionY = Quaternion.AngleAxis(_angle * _animationSpeed, Vector3.up);
        Quaternion quaternionX = Quaternion.AngleAxis(_angle * _animationSpeed, Vector3.right);
        Quaternion quaternionZ = Quaternion.AngleAxis(_angle, Vector3.forward);

        transform.rotation = _originalRotation * quaternionY * quaternionX * quaternionZ;
    }
}
