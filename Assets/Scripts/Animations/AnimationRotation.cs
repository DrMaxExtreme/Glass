using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRotation : MonoBehaviour
{
    private Vector3 _angle;

    private void Start()
    {
        float maxAngle = 1.5f;

        transform.rotation = Random.rotation;
        _angle = new Vector3(Random.Range(0, maxAngle), Random.Range(0, maxAngle), Random.Range(0, maxAngle));
    }

    private void FixedUpdate()
    {
        transform.Rotate(_angle);
    }
}
