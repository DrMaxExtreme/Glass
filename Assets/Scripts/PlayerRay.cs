using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRay : MonoBehaviour
{
    private SelecterColor _currentSecectable;

    private void LateUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, transform.forward * 100f, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
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
