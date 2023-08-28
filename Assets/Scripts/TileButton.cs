using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileButton : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] Animator anim;

    private Tile _tile;

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
        // 버튼을 눌렀을 때 타일을 이동시켜 보자.

        // 타일을 리스트에 넣는다.
        List<Tile> newTileList = _tile.TilesButtonLine();
        
        // 마지막 위치의 타일 위치를 저장합니다.
        Vector3 tempPos = newTileList[newTileList.Count].transform.position;
        for (int i = 0; i < newTileList.Count; i++)
        {
            // 
        }
    }
}
