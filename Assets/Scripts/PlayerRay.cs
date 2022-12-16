using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    [SerializeField] LayerMask _layerMask;

    private SelecterCubes _currentSecectable;

    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _layerMask))
        {
            SelecterCubes selectable = hit.collider.gameObject.GetComponent<SelecterCubes>();

            if (selectable)
            {
                if (_currentSecectable && _currentSecectable != selectable)
                    _currentSecectable.Deselect();

                _currentSecectable = selectable;
                selectable.Select(true);
                selectable.SelectIdentityColorCubes(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                selectable.SelectIdentityColorCubes(true);
                Destroy(selectable.gameObject);
                spawner.SpawnCubes();
            }
        }
    }
}
