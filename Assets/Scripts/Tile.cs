using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
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
            if (y == level.Column - 1)
                transform.eulerAngles = new Vector3(0, -1 * rotationMultipler, 0);
    }

    public List<Tile> TilesButtonLine()
    {
        // 해당되는 타일들을 전부 담기 위한 타일 리스트
        List<Tile> result = new List<Tile>();
        // 버튼 타일을 기준으로 정면에 레이캐스트를 쏘았을 때
        // "Tile" 레이어를 가진 Collider 오브젝트들 만 리스트에 넣기
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Tile");
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            result.Add(hits[i].collider.transform.GetComponent<Tile>());
        }

        return result;
    }
}
