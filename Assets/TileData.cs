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
    
// Ÿ���� ������ ����
public class TileData : MonoBehaviour
{
    [Header("Direction")]
    [SerializeField] TileType tileType;
    [SerializeField] Dir[] dirs = new Dir[4];

    // Ÿ���� Ÿ��
    public TileType Tile { get; set; }
    // Ÿ���� �ٶ󺸴� ����� (�ּ� 2�� �̻�)
    public Dir Dirs { get; set; }
    // �ʵ忡 ��ġ�� ������Ʈ�� �� ����
    public GameObject go;

}
