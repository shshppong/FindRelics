using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //[HideInInspector] public bool IsFilled;
    [HideInInspector] public int TileType;

    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform[] _tilePrefabs;

    private Transform currentTile;
    private int rotation;

    // SpriteRenderer
    // SpriteRenderer
    private List<Transform> connectBoxes;

    private const int minRotation = 0;
    private const int maxRotation = 3;
    private const int rotationMultiplier = 90;

    public void Initialize(int tile)
    {
        // 타일 데이터 기입된 숫자가 십의 자리가 넘어가도 0~3으로 고정되게
        TileType = tile % levelData.Column;
        // 생성된 타일 프리팹의 트랜스 정보를 가져옴
        currentTile = Instantiate(_tilePrefabs[TileType], transform);
        // 생성된 타일에 트랜스 정보의 위치를 0, 0, 0으로 함
        currentTile.transform.localPosition = Vector3.zero;
        // 타일 타입이 시작 또는 끝 이라면
        if (TileType == 1 || TileType == 2)
        {
            // 시작과 끝 타일
            // rotation = tile / 10;
        }
        else
        {
            // 그 외 타일에 대한 회전 값 랜덤 적용
            rotation = Random.Range(minRotation, maxRotation + 1);
        }
        currentTile.transform.eulerAngles = new Vector3(0, rotation * rotationMultiplier, 0);

        if (TileType == 0 || TileType == 1)
        {
            //IsFilled = true;
        }

        if (TileType == 0)
        {
            return;
        }

        // emptySprite = currentTile.GetChild(0).GetComponent<SpriteRenderer>();
        // emptySprite.gameObject.SetActive(!IsFilled);
        // filledSprite = currentTile.GetChild(1).GetComponent<SpriteRenderer>();
        // filledSprite.gameObject.SetActive(IsFilled);ㄷ

        connectBoxes = new List<Transform>();
        for (int i = 2; i < currentTile.childCount; i++)
        {
            connectBoxes.Add(currentTile.GetChild(i));
        }
    }
}
