using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyerLine : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    public void Destroy()
    {
        float rayDisnatce = 10f;

        Ray ray = new Ray(transform.position, transform.right);
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDisnatce, _layerMask);

        foreach (var hit in hits)
            Destroy(hit.collider.gameObject);
    }
}
