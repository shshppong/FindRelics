using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static Util;

public class TileButton : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] Animator anim;

    // 타일의 YPos, XPos를 가져오기 위한 클래스 불러오기
    Tile tile;

    [Header("Level Data")]
    // Row, Column 값 가져오기 위한 클래스 불러오기
    [SerializeField] LevelData levelData;

    GameManager gameManager;
    // 버튼 타일의 열 위치
    int col;
    // 버튼 타일의 행 위치
    int row;

    void Start()
    {
        gameManager = GameManager.Instance;
        anim.GetComponent<Animator>();
        tile = transform.parent.GetComponent<Tile>();
        col = tile.Column;
        row = tile.Row;
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
        if (row == 0)
            ChangeRowLine(row);
        if (col == levelData.Row - 1)
            ChangeColumnLine(col);
    }

    private void ChangeRowLine(int row)
    {
        Debug.Log($"{row}, {col}, {levelData.Row - 2}");
        Tile tempTile = gameManager.Tiles[levelData.Column - 2, 1];
        Debug.Log(tempTile.transform.GetChild(0).name);
        for (int i = levelData.Row - 2; i > 2; i--)
        {
            gameManager.Tiles[i, 1] = gameManager.Tiles[i - 1, 1];
            gameManager.Tiles[i, 1].tileObject = gameManager.Tiles[i - 1, 1].tileObject;
            gameManager.Tiles[i - 1, 1] = gameManager.Tiles[i - 2, 1];
            gameManager.Tiles[i - 1, 1].tileObject = gameManager.Tiles[i - 2, 1].tileObject;
        }
        gameManager.Tiles[1, 1] = tempTile;
        gameManager.Tiles[1, 1].tileObject = tempTile.tileObject;
    }
    private void ChangeColumnLine(int column)
    {
        Debug.Log($"Row");
    }
}
