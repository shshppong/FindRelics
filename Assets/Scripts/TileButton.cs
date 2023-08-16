using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] Animator anim;

    private Tile _tile;
    
    public int Y { get; set; }
    public int X { get; set; }

    void Start()
    {
        anim.GetComponent<Animator>();
        _tile = GetComponent<Tile>();
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
        anim.SetTrigger("Clicked");
        GameManager.Instance.Shift(Y, X);
    }
}
