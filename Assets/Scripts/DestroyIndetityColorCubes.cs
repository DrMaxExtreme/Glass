using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIndetityColorCubes : MonoBehaviour
{
    /*public void Destroy(LayerMask layerMask) //������� ��������� ����� �� �������� �����
    {
        float rayDisnatce = 20f;

        Ray rayUp = new Ray(transform.position, transform.up); //�� ���
        Ray rayDown = new Ray(transform.position, -transform.up);
        Ray rayRigth = new Ray(transform.position, transform.right);
        Ray rayLeft = new Ray(transform.position, -transform.right);//�� ���

        SelectInRay(Physics.RaycastAll(rayUp, rayDisnatce, layerMask));//������� � ������ ����� � select color
        SelectInRay(Physics.RaycastAll(rayDown, rayDisnatce, layerMask));
        SelectInRay(Physics.RaycastAll(rayRigth, rayDisnatce, layerMask));
        SelectInRay(Physics.RaycastAll(rayLeft, rayDisnatce, layerMask));
    }*/

    /*private void SelectInRay(RaycastHit[] collisions)
    {
        foreach (var collision in collisions)
        {
            int collisionColorIndex = collision.collider.gameObject.GetComponent<SelecterColor>().ColorIndex;

            if (collisionColorIndex == _colorIndex)
            {
                collision.collider.gameObject.GetComponent<SelecterColor>().Select(true);
            }
        }
    }*/
}
