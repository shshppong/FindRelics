using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn Level Value")]
    [SerializeField] private LevelData _level;
    [SerializeField] private Tile _tilePrefab;

    private bool hasGameFinished;
    private Tile[,] tiles;
    [SerializeField]
    private List<Tile> startTiles;
    
    [Header("Spawn Button Value")]
    [SerializeField] private TileButton _buttonPrefab;
    private TileButton[,] btnArr;

    [Header("Block Spacing")]
    private float defaultSpacing = 1f;
    [SerializeField] private float spawnSpacing = 0.5f;
    float zPos;
    float xPos;

    private void Awake()
    {
        Instance = this;

        SpawnLevel();
        //SpawnButton();
    }

    private void SpawnLevel()
    {
        tiles = new Tile[_level.Row, _level.Column];
        startTiles = new List<Tile>();
        zPos = 0;
        xPos = 0;
        for (int z = 0; z < _level.Row; z++)
        {
            // y값, x값을 저장하는데
            // x값이 루프 끝나면 y에서 x값 초기화
            for (int x = 0; x < _level.Column; x++)
            {
                Vector3 spawnPos = new Vector3(xPos, 0, zPos);
                Tile tempTile = Instantiate(_tilePrefab);
                tempTile.transform.position = spawnPos;
                tempTile.Initialize((int)_level.Data[z * _level.Column + x]);
                tiles[z, x] = tempTile;
                if (tempTile.TileType == 1)
                {
                    startTiles.Add(tempTile);
                }
                xPos += spawnSpacing + defaultSpacing;
            }
            zPos += spawnSpacing + defaultSpacing;
            xPos = 0;
        }
        
        // 2차원 배열 가져와서 tiles[z, x] 검색해서
        // 안에 있는 Transform의 위치를 조정해서 간격을 띄우는 방법으로 ㄱㄱ


        // Camera.main.orthographicSize = Mathf.Max(_level.Row, _level.Column);
        // Vector3 cameraPos = Camera.main.transform.position;
        // cameraPos.x = _level.Column * 0.5f;
        // cameraPos.y = _level.Row * 0.5f;
    }

    private void SpawnButton()
    {
        // 타일 데이터 가져와서 끝 지점을 가져와야함.
        // 3 x 3 이면
        // 5 x 5 로 구성
        btnArr = new TileButton[_level.Row + 2, _level.Column + 2];
        for (int z = 0; z < _level.Row; z++)
        {
            for (int x = 0; x < _level.Column; x++)
            {
                if (x == 0 || z == 0)
                {
                    Vector3 spawnPos = new Vector3(x + spawnSpacing, 0, z + spawnSpacing);
                    TileButton tempBtn = Instantiate(_buttonPrefab);
                    tempBtn.transform.position = spawnPos;
                    btnArr[z, x] = tempBtn;
                }
            }
        }
    }
}
