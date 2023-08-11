using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    int x;
    int y;

    [SerializeField] private Transform[] _tilePrefabs;

    // 회전 상수
    public int minMoveTile = 4;
    public int maxMoveTile = 7;
    private const int minRotation = 0;
    private const int maxRotation = 3;
    private const int rotationMultiplier = 90;

    private Transform currentTile;
    private int rotation;

    public void Initialize(TileData tileData)
    {
        int tileType = (int)tileData % (maxMoveTile + 1);
        Debug.Log(tileType);

        currentTile = Instantiate(_tilePrefabs[tileType], transform);
        currentTile.transform.localPosition = Vector3.zero;
        
        // 생성된 타일 오브젝트에 회전 값 넣기
        if (tileType >= minMoveTile && tileType <= maxMoveTile)
        {
            rotation = Random.Range(minRotation, maxRotation + 1);
        }
        currentTile.transform.eulerAngles = new Vector3(0, rotation * rotationMultiplier, 0);
    }
}
