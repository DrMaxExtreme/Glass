using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MovementDown : MonoBehaviour
{
    private float _speed = 0.1f;
    private float _distance = 1;

    private void Update()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - _distance, transform.position.z);

        Ray ray = new Ray(transform.position, -transform.up);

        if(Physics.Raycast(ray, _distance))
            StartCoroutine(MoveDown(targetPosition));
        else
            StopCoroutine(MoveDown(targetPosition));

    }

    private IEnumerator MoveDown(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed);

        yield return null;
    }
}
