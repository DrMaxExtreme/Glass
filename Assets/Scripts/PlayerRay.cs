using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;

    private SelecterColor _currentSecectable;

    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _layerMask))
        {
            SelecterColor selectable = hit.collider.gameObject.GetComponent<SelecterColor>();

            if (selectable)
            {
                if (_currentSecectable && _currentSecectable != selectable)
                    _currentSecectable.Deselect();

                _currentSecectable = selectable;
                selectable.Select(true);
                selectable.SelectIdentityColorCubes();
            }

            if (Input.GetMouseButtonDown(0))
            {
                //hit.collider.gameObject.GetComponent<DestroyIndetityColorCubes>().Destroy(_layerMask);
            }
        }
    }
}
