using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;

    private SelecterColor _currentSecectable;

    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerMask))
        {
            SelecterColor selectable = hit.collider.gameObject.GetComponent<SelecterColor>();

            if (selectable)
            {
                if(_currentSecectable && _currentSecectable != selectable)
                    _currentSecectable.Deselect();

                _currentSecectable = selectable;
                selectable.Select();
            }
            else
            {
                if (_currentSecectable)
                {
                    _currentSecectable.Deselect();
                    _currentSecectable = null;
                }
            }
        }
        else
        {
            if (_currentSecectable)
            {
                _currentSecectable.Deselect();
                _currentSecectable = null;
            }
        }
    }
}
