using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static PublicLibrary;

public class Tile : MonoBehaviour
{
    [SerializeField] private Transform[] _tilePrefabs;

    // 타일(자식) 데이터를 가질 변수
    public TileType TileType = new TileType();

    public int minMoveTile = 4;
    public int maxMoveTile = 7;

    // 회전 상수
    private const int minRotation = 0;
    private const int maxRotation = 3;
    private const int rotationMultiplier = 90;

    private int rotation;

    private TileButton btn;

    Transform currentTile;

    public List<Transform> currentTileList = new List<Transform>();

    public void InsertValue(Tile newTile)
    {
        TileType = newTile.TileType;
        rotation = newTile.rotation;
        currentTile = newTile.currentTile;
        currentTileList.AddRange(newTile.currentTileList);
    }

    public void Initialize(TileType tileType, int y, int x)
    {
        TileType = tileType;
        int tileTypeNum = (int)TileType % (maxMoveTile + 1);
        if (tileTypeNum >= minMoveTile && tileTypeNum <= maxMoveTile)
        {
            for (int i = minMoveTile; i <= maxMoveTile; i++)
            {
                currentTile = Instantiate(_tilePrefabs[i], transform);
                currentTile.transform.localPosition = Vector3.zero;
        
                rotation = Random.Range(minRotation, maxRotation);
                
                //currentTile.transform.eulerAngles = new Vector3(0, rotation * rotationMultiplier, 0);
                currentTile.transform.rotation = Quaternion.Euler(new Vector3(0, rotation * rotationMultiplier, 0));

                currentTileList.Add(currentTile);

                currentTile.gameObject.SetActive(false);
            }

            currentTile = transform.GetChild(tileTypeNum % minMoveTile);
            currentTile.gameObject.SetActive(true);
        }
        else
        {
            Transform currentTile = Instantiate(_tilePrefabs[tileTypeNum], transform);
            currentTile.transform.localPosition = Vector3.zero;
            
            if (TileType == TileType.Button)
            {
                btn = transform.GetComponentInChildren<TileButton>();
                btn.Y = y;
                btn.X = x;

                currentTile = transform.GetChild(0);
            }
        }
    }

    public void ChangeTile(Tile tile)
    {
        TileType tileType = tile.TileType;

        // 이 오브젝트에 활성화 되어있는 타일을 비활성화 시킨다.
        Transform beforeTr = currentTileList[(int)TileType % minMoveTile];
        beforeTr.gameObject.SetActive(false);

        // 바꿀 대상 타일의 회전 값 y을 가져온다.
        Transform afterTr = tile.currentTileList[(int)tileType % minMoveTile];
        Vector3 afterRot = afterTr.transform.eulerAngles;

        // 이 오브젝트에 활성화 시킬 대상의 타일을 가져온다.
        beforeTr = currentTileList[(int)tileType % minMoveTile];

        // 대상 오브젝트와 바꿀 타일의 회전 값을 덮어씌운다.
        beforeTr.transform.eulerAngles = afterRot;
        
        // 대상 오브젝트를 활성화 시킨다.
        beforeTr.transform.gameObject.SetActive(true);

        TileType = tileType;
        currentTile = tile.currentTile;
    }
}
