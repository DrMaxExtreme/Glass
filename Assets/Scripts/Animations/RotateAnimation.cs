using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    private float _animationSpeed;
    private Quaternion originalRotation;
    private float angle;

    private void Start()
    {
        float minSpeedAnimation = 0.5f;
        float maxSpeedAnimation = 1.5f;

        transform.rotation = Random.rotation;
        _animationSpeed = Random.Range(minSpeedAnimation, maxSpeedAnimation);
        originalRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        angle++;

        Quaternion quaternionY = Quaternion.AngleAxis(angle * _animationSpeed, Vector3.up);
        Quaternion quaternionX = Quaternion.AngleAxis(angle * _animationSpeed, Vector3.right);
        Quaternion quaternionZ = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = originalRotation * quaternionY * quaternionX * quaternionZ;
    }
}
