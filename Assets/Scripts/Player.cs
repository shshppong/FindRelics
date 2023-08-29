using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    LevelData _level;

    public float PosX { get; private set; }
    public float PosY { get; private set; }
    public float PosZ { get; private set; }

    Tile _tile;

    public void Initialize(Transform tr, LevelData level, Tile tile)
    {
        PosX = tr.position.x;
        PosY = tr.position.y;
        PosZ = tr.position.z;
        _level = level;
        _tile = tile;
        
        transform.position = new Vector3(PosX, PosY, PosZ);
    }

    
}
