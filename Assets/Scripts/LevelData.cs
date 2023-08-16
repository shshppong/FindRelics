using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class LevelData : ScriptableObject
{
    public int Row;
    public int Column;
    public List<TileType> Data;

    private void OnValidate()
    {
        Column = Row;
    }
}
