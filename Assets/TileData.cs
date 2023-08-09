using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Dir
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
}

public enum TileType
{
    Empty,
    Wall,
}
    
// 타일의 데이터 집합
public class TileData : MonoBehaviour
{
    [Header("Direction")]
    [SerializeField] TileType tileType;
    [SerializeField] Dir[] dirs = new Dir[4];

    // 타일의 타입
    public TileType Tile { get; set; }
    // 타일의 바라보는 방향들 (최소 2개 이상)
    public Dir Dirs { get; set; }
    // 필드에 배치될 오브젝트가 들어갈 변수
    public GameObject go;

}
