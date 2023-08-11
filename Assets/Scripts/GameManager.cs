using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn Level Value")]
    [SerializeField] private LevelData _level;
    [SerializeField] private Tile _tilePrefab;

    private Tile[,] tiles;
    
    [Header("Block Spacing")]
    public float spawnSpacing = 0.5f;
    public float defaultSpacing = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnLevel();
        //PrintTiles();
    }

    void SpawnLevel()
    {
        tiles = new Tile[_level.Row, _level.Column];
        float yPos = 0;
        float xPos = 0;
        for (int y = 0; y < _level.Row; y++)
        {
            for (int x = 0; x < _level.Column; x++)
            {
                Vector3 spawnPos = new Vector3(yPos, 0, xPos);
                Tile newTile = Instantiate(_tilePrefab);
                newTile.transform.position = spawnPos;
                newTile.Initialize(_level.Data[y * _level.Column + x]);
                tiles[y, x] = newTile;

                xPos += spawnSpacing + defaultSpacing;
            }
            yPos += spawnSpacing + defaultSpacing;
            xPos = 0f;
        }
    }

    void PrintTiles()
    {
        for (int y = 0; y < _level.Row; y++)
        {
            for (int x = 0; x < _level.Column; x++)
            {
                Debug.Log(tiles[y, x]);
            }
        }
    }
}
