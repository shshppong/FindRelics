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
    public Tile[,] Tiles { get { return tiles; } set { tiles = value; } }

    [SerializeField]
    private List<Tile> startTiles;
    
    [Header("Spawn Button Value")]
    [SerializeField] private TileButton _buttonPrefab;
    private TileButton[,] btnArr;

    [Header("Block Spacing")]
    private float defaultSpacing = 1f;
    [SerializeField] private float spawnSpacing = 0.5f;
    public float DefaultSpacing { get { return defaultSpacing; } }
    public float SpawnSpacing { get { return spawnSpacing; } }
    float yPos;
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
        yPos = 0;
        xPos = 0;
        for (int y = 0; y < _level.Row; y++)
        {
            // y값, x값을 저장하는데
            // x값이 루프 끝나면 y에서 x값 초기화
            for (int x = 0; x < _level.Column; x++)
            {
                Vector3 spawnPos = new Vector3(xPos, 0, yPos);
                Tile tempTile = Instantiate(_tilePrefab);
                tempTile.transform.position = spawnPos;
                tempTile.Initialize(_level.Data[y * _level.Column + x], y, x, spawnPos);
                tiles[y, x] = tempTile;
                tiles[y, x].tileObject = tempTile.gameObject;
                if (tempTile.TileType == (int)TileData.Start)
                {
                    startTiles.Add(tempTile);
                }
                xPos += spawnSpacing + defaultSpacing;
            }
            yPos += spawnSpacing + defaultSpacing;
            xPos = 0;
        }

        // Camera.main.orthographicSize = Mathf.Max(_level.Row, _level.Column);
        // Vector3 cameraPos = Camera.main.transform.position;
        // cameraPos.x = _level.Column * 0.5f;
        // cameraPos.y = _level.Row * 0.5f;
    }

    public void ReplaceTiles()
    {
        Tile[,] tempTiles = new Tile[_level.Row, _level.Column];
        tempTiles = (Tile[,])tiles.Clone();
        yPos = 0;
        xPos = 0;
        for (int y = 0; y < _level.Row; y++)
        {
            for (int x = 0; x < _level.Column; x++)
            {
                Vector3 spawnPos = new Vector3(xPos, 0, yPos);
                tempTiles[y, x].transform.position = spawnPos;
                tiles[y, x] = tempTiles[y, x];
            }
            xPos += spawnSpacing + defaultSpacing;
        }
        yPos += spawnSpacing + defaultSpacing;
        xPos = 0;
    }
}
