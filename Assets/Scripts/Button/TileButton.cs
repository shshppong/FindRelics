using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButton : MonoBehaviour
{

    // ���콺 ������ ������ ����� ��
    private void OnMouseEnter()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // TODO
            Debug.Log($"[Obj:{hit.transform.name}] On Mouse Enter");
        }
    }

    private void OnMouseExit()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // TODO
            Debug.Log($"[Obj:{hit.transform.name}] On Mouse Exit");
        }
    }

    private void OnMouseDown()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // TODO
            Debug.Log($"[Obj:{hit.transform.name}] On Mouse Down");
        }
    }

    private void OnMouseUp()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // TODO
            Debug.Log($"[Obj:{hit.transform.name}] On Mouse Up");
        }
    }
}
