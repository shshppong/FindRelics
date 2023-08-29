using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;
using System.Linq;
using DG.Tweening;

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

    Queue<Tile> tileQueue = new Queue<Tile>();

    [Header("Tile Spawned List")]
    List<Tile> spawnedTiles = new List<Tile>();

    // 게임 종료 불리언 변수
    public bool hasGameEnded;

    // 트윈 실행 큐
    public Queue<Tween> tweenQueue = new Queue<Tween>();
    
    // 우선 now 부터 방문
    // now에 있는 콜라이더를 이용하여 레이캐스트를 쏘고 아직 미방문 상태라면 방문한다.
    void DFS(Tile now, ref bool isSearchDone)
    {
        // 우선 now 부터 방문하고
        now.isVisited = true;

        // 목적지에 도달한 경우
        if (now.isDestination)
        {
            isSearchDone = true;
            tileQueue.Enqueue(now);
            // 게임 종료 변수 활성화 시키기
            hasGameEnded = true;
            // 캐릭터 움직이기
            Player.MoveStart(tileQueue);
            return;
        }

        // 계속 호출 되지 않도록 미리 생성
        Transform[] trs = now.colls.ToArray();
        // 레이어에서 타일로 등록된 레이어를 시프트 연산시킨다
        LayerMask layerNameTile = 1 << LayerMask.NameToLayer("Connect");
        
        // 레이캐스트 쏠 콜라이더 갯수를 가져온다.
        for (int next = 0; next < trs.Length; next++)
        {
            // 이미 목적지에 도달한 경우 중단
            if (isSearchDone)
                return;

            // 만약 나와 상대방으로 이어지는 통로가 있으면 진행하고,
            // 없으면 스킵
            Ray ray = new Ray(trs[next].transform.position, trs[next].forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 0.2f, layerNameTile))
            {
                Tile checkTile = hit.collider.GetComponentInParent<Tile>();
                // 만약 타일이 존재하지 않으면 스킵
                if (checkTile == null)
                    continue;
                // 만약 통로가 있는데 이미 지나온 곳이라면 스킵
                if (checkTile.isVisited)
                    continue;

                // 만약 통로가 존재하면 큐에 넣고 다음 진행하기
                tileQueue.Enqueue(checkTile);
                DFS(checkTile, ref isSearchDone);
            }
        }
    }

    public void DFSStart()
    {
        // 모든 타일의 방문 여부를 초기화 한다.
        DisableVisitedTiles();
        // 타일 스택을 초기화 시킨다.
        tileQueue.Clear();
        // 레퍼런스 불리언으로 DFS의 중단점을 만들었기 때문에 불리언 변수를 선언했다.
        bool isSearchDone = false;
        // DFS 알고리즘 실행
        DFS(Player.Tile, ref isSearchDone);
    }
    
    // 모든 타일의 방문 여부를 초기화하는 함수
    private void DisableVisitedTiles()
    {
        foreach (Tile tile in spawnedTiles)
        {
            tile.isVisited = false;
        }
    }

    void Awake()
    {
        Instance = this;

        SpawnStage();
        CameraSetting();

        hasGameEnded = false;
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
                    Player.Initialize(temp, newTile);
                }
                
                // 생성된 타일 적재하기
                spawnedTiles.Add(newTile);

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
}
