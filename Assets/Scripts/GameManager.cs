using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn Level Value")]
    [SerializeField] private LevelData _level;

    [Header("Block Spacing")]
    public float spawnSpacing = 0.25f;
    public float defaultSpacing = 1f;

    [Header("Start Tile")]
    public Tile startTile;

    // LevelData의 Data를 읽어와서 타일 배치한다.
    // 그 전에 3x3 이면 x + 2, y + 2 해서 부속 타일부터 배치시킨다.


    private void Awake()
    {
        Instance = this;

        SpawnStage();
        CameraSetting();
    }

    void SpawnStage()
    {
        int row = _level.Row;
        int col = _level.Column;

        float yPos = 0f;
        float xPos = 0f;

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                // 타일 위치 미리 저장하기
                Vector3 newPos = new Vector3(yPos, 0, xPos);
                // 레벨 데이터 안에 타일 프리팹 불러오기
                TileData tileData = _level.Data[y * col + x];

                // 불러온 타일 생성
                Tile newTile = Instantiate(tileData.TilePrefab).GetOrAddComponent<Tile>();
                // 미리 저장한 타일 위치 정의하기
                newTile.transform.position = newPos;
                TileType type = tileData.TileType;
                // 타일 초기화하기
                newTile.Initialize(type, tileData.Rotation, x, y, _level);

                // 시작 타일이면 startTiles 리스트에 넣기
                if (type == TileType.Start)
                    startTile = newTile;

                xPos += defaultSpacing + spawnSpacing;
            }
            xPos = 0;
            yPos += defaultSpacing + spawnSpacing;
        }
        yPos = 0;
    }

    void CameraSetting()
    {
        Camera.main.orthographicSize = (Mathf.Max(_level.Row, _level.Column) / 2f) + 1f;
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = _level.Column;
        cameraPos.y = _level.Row + 1;
        Camera.main.transform.position = cameraPos;
    }
}
