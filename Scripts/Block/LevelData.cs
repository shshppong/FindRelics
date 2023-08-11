using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileData
{
    Empty = 0,
    Button = 1,
    Start = 2,
    End = 3,
    Straight = 4,
    Up_Left_Down_Right = 5,
    Up_Left_Down = 6,
    Up_Left = 7
}

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class LevelData : ScriptableObject
{
    public int Row;
    public int Column;
    public List<TileData> Data;
}
