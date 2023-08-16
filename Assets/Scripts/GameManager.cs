using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Spawn Level Value")]
    [SerializeField] private LevelData _level;
    [SerializeField] private GameObject _tilePrefab;

    [Header("Block Spacing")]
    public float spawnSpacing = 0.25f;
    public float defaultSpacing = 1f;

    private Tile[,] _tileData;

    private GameObject tempGameObject;
    private Tile tempTile;

    private void Awake()
    {
        Instance = this;
        SpawnLevel();
    }

    private void Start()
    {
        //PrintTileNumberArray();
        tempGameObject = Instance.gameObject;
        tempGameObject.transform.position = Vector3.up * -100f;
        tempTile = tempGameObject.AddComponent<Tile>();
    }

    public void SpawnLevel()
    {
        _tileData = new Tile[_level.Column, _level.Row];
        float yPos = 0f;
        float xPos = 0f;

        for (int y = 0; y < _level.Column; y++)
        {
            for (int x = 0; x < _level.Row; x++)
            {
                // 부모 타일 오브젝트 생성하기
                Vector3 newPos = new Vector3(yPos, 0, xPos);
                Tile newTile = Instantiate(_tilePrefab).GetComponent<Tile>();
                // 부모 타일 위치 정의하기
                newTile.transform.position = newPos;
                // 부모 타일 속성 초기화 하기 (자식 오브젝트에 타일 하위 생성)
                newTile.Initialize(_level.Data[y * _level.Column + x], y, x);

                // 배열에 타일 데이터 집어넣기
                _tileData[y, x] = newTile;

                xPos += defaultSpacing + spawnSpacing;
            }
            yPos += defaultSpacing + spawnSpacing;
            xPos = 0;
        }
    }

    public void Shift(int y, int x)
    {
        if (x == 0)
        {
            int length = _tileData.GetLength(0);
            // Shift 로직
            tempTile.InsertValue(_tileData[y, length - 2]);
            for (int i = length - 3; i >= 1; i--)
            {
                _tileData[y, i + 1].ChangeTile(_tileData[y, i]);
            }
            _tileData[y, 1].ChangeTile(tempTile);
        }
        else if (y == _tileData.GetLength(1) - 1) // 4
        {
            int length = _tileData.GetLength(1) - 2; // 3
            tempTile.InsertValue(_tileData[1, x]);
            for (int i = 1; i <= length - 1; i++)
            {
                _tileData[i, x].ChangeTile(_tileData[i + 1, x]);
            }
            _tileData[length, x].ChangeTile(tempTile);
        }
    }

    public void PrintTileNumberArray()
    {
        string strs = null;
        for (int y = 0; y < _tileData.GetLength(0); y++)
        {
            for (int x = 0; x < _tileData.GetLength(1); x++)
            {
                strs += $"{(int)_tileData[y, x].TileType}\t";
            }
            strs += "\n";
        }
        Debug.Log(strs);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PrintTileNumberArray();
        }
    }
}
