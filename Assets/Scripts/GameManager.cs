using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Spawn Level Value")]
    public LevelData Level;

    [Header("Block Spacing")]
    public float spawnSpacing = 0.25f;
    public float defaultSpacing = 1f;

    [Header("Start Tile")]
    public Tile startTile;

    [Header("Character")]
    public GameObject CharacterPrefab;
    public Player Player;

    [Header("Animations")]
    public float PlayingAnimSpeed = 0.3f;

    void Awake()
    {
        Instance = this;

        SpawnStage();
        CameraSetting();
    }

    // 1 프레임 이후 처리할 코드
    private bool hasRunOnce = false;
    void LateUpdate()
    {
        if (hasRunOnce)
            return;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject go in gos)
        {
            TileButton tileButton = go.GetComponent<TileButton>();
            tileButton.Initialize();
        }
        hasRunOnce = true;
    }

    void SpawnStage()
    {
        int row = Level.Row;
        int col = Level.Column;

        float yPos = 0f;
        float xPos = 0f;

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                // 타일 위치 미리 저장하기
                Vector3 newPos = new Vector3(yPos, 0, xPos);
                // 레벨 데이터 안에 타일 프리팹 불러오기
                TileData tileData = Level.Data[y * col + x];

                // 불러온 타일 생성
                Tile newTile = Instantiate(tileData.TilePrefab).GetOrAddComponent<Tile>();
                // 미리 저장한 타일 위치 정의하기
                newTile.transform.position = newPos;
                TileType type = tileData.TileType;
                // 타일 초기화하기
                newTile.Initialize(type, tileData.Rotation, x, y, Level);

                // 시작 타일이면 startTiles 리스트에 넣기
                if (type == TileType.Start)
                {
                    startTile = newTile;
                    // 캐릭터 생성하기
                    Player = Instantiate(CharacterPrefab).GetComponent<Player>();
                    Transform temp = newTile.transform.GetChild(1).transform;
                    Player.Initialize(temp, Level, newTile);
                }

                xPos += defaultSpacing + spawnSpacing;
            }
            xPos = 0;
            yPos += defaultSpacing + spawnSpacing;
        }
        yPos = 0;
    }

    void CameraSetting()
    {
        Camera.main.orthographicSize = (Mathf.Max(Level.Row, Level.Column) / 2f) + 1f;
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = Level.Column;
        cameraPos.y = Level.Row + 1;
        Camera.main.transform.position = cameraPos;
    }

    void BFS()
    {
        Vector3[] deltaPos = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

        bool[,] found = new bool[Level.Row, Level.Column];

        Queue<Pos> q = new Queue<Pos>();
        // 첫 위치는 캐릭터가 서 있는 위치로 등록한다.
        q.Enqueue(new Pos(Player.PosY, Player.PosX));

    }
}
