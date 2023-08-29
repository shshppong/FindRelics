using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;

public class Tile : MonoBehaviour
{
    public TileType Type;

    private int rotation;
    private int rotationMultipler = 90;

    public int X { get; private set; }
    public int Y { get; private set; }

    LevelData levelData;

    public void Initialize(TileType type, int rot, int x, int y, LevelData level)
    {
        Type = type;
        rotation = rot;
        X = x;
        Y = y;

        transform.eulerAngles = new Vector3(0, rotation * rotationMultipler, 0);

        if (Type == TileType.Button)
        {
            if (y == level.Column - 1)
            {
                transform.eulerAngles = new Vector3(0, -1 * rotationMultipler, 0);
            }
        }
    }
}
