using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] Animator anim;
    


    void Start()
    {
        anim.GetComponent<Animator>();
    }

    // 마우스 가까이 가져다 대었을 때
    void OnMouseEnter()
    {
        anim.SetBool("isEntered", true);
    }

    void OnMouseExit()
    {
        anim.SetBool("isEntered", false);
    }

    void OnMouseDown()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            // TODO
            anim.SetTrigger("Clicked");
        }
    }
}
