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

    // 콜라이더 오브젝트를 저장하는 리스트
    public List<Transform> colls = new List<Transform>();
    // 평지 위치 값을 저장하는 변수
    public Vector3 floor = Vector3.zero;

    // 방문 했는지 체크하는 변수
    public bool isVisited;

    // 마지막 타일인지 확인하는 변수
    public bool isDestination;

    public void Initialize(TileType type, int rot, int x, int y, LevelData level)
    {
        Type = type;
        rotation = rot;
        X = x;
        Y = y;
        // 타일 방문 여부
        isVisited = false;

        // 자식 오브젝트 가져오기
        if (Type >= TileType.Start)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Floor"))
                {
                    floor = child.position;
                }
                else if (child.CompareTag("Connect"))
                {
                    colls.Add(child);
                }
            }
        }

        transform.eulerAngles = new Vector3(0, rotation * rotationMultipler, 0);

        if (Type == TileType.Button)
        {
            if (y == level.Column - 1)
            {
                transform.eulerAngles = new Vector3(0, -1 * rotationMultipler, 0);
            }
        }

        if (Type == TileType.End)
            isDestination = true;
    }
}
