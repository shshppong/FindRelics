using System.Collections;
using System.Collections.Generic;
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
        if (GameManager.Instance.isRelocation == false || GameManager.Instance.hasGameFinished == true)
            return;
        anim.SetTrigger("Clicked");
        GameManager.Instance.RunCoroutine(Y, X);
    }
}
