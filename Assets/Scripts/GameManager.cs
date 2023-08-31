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

    [Header("Tile Spawned List")]
    List<Tile> spawnedTiles = new List<Tile>();

    // 게임 종료 불리언 변수
    public bool hasGameEnded;

    // 트윈 실행 큐
    public Queue<Tween> tweenQueue = new Queue<Tween>();

    // 레이어에서 "Connect"로 등록된 레이어를 시프트 연산시킨다
    LayerMask layerConncet;

    public GameObject UI_Clear_Prefab;

    // BFS의 결과 순서를 저장할 큐를 선언해준다.
    Queue<Tile> resultQueue = new Queue<Tile>();
    void BFS(Tile now)
    {
        Queue<Tile> q = new Queue<Tile>();
        // 큐에 시작점을 넣어준다.
        q.Enqueue(now);

        // 큐에 들어간 첫 번째 타일을 방문한 것으로 처리한다.
        // 큐가 빌 때까지 반복적으로 수행한다.
        while (q.Count > 0)
        {
            // 큐에서 하나를 꺼내온다.
            Tile current = q.Dequeue();

            // 인식한 타일이 마지막 타일이라면
            // 큐에 넣고 루프 종료
            if (current.isDestination)
            {
                resultQueue.Enqueue(current);
                // 게임 종료 변수 활성화 시키기
                hasGameEnded = true;
                // 캐릭터 움직이기
                Player.MoveStart(resultQueue);
                break;
            }

            // 꺼내온 타일에 들어있는 콜라이더를 가져온다.
            Transform[] trs = current.colls.ToArray();
            // 큐에서 꺼낸 원소가 가진 콜라이더의 방향으로 
            // 연결되어있는지 하나씩 방문하면서 확인한다.
            for (int i = 0; i < trs.Length; i++)
            {
                // 상 하 좌 우 검색
                Ray ray = new Ray(trs[i].position, trs[i].forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.2f, layerConncet))
                {
                    // 통로가 연결이 되어있을 때 처리.
                    Tile connectTile = hit.collider.GetComponentInParent<Tile>();

                    // 연결된 통로에 타일이 존재하지 않으면 스킵
                    if (connectTile.isVisited == false)
                    {
                        q.Enqueue(connectTile);

                        // 연결된 통로에 타일이 존재 한다면
                        // 방문 여부 활성화 및 연결된 타일 선언한 큐에 넣기
                        if (current.isVisited == false)
                        {
                            current.isVisited = true;
                            resultQueue.Enqueue(current);
                        }
                        continue;
                    }
                }
            }
        }
    }

    public void BFSStart()
    {
        // 모든 타일의 방문 여부를 초기화 한다.
        DisableVisitedTiles();
        // 타일 스택을 초기화 시킨다.
        resultQueue.Clear();

        // BFS 알고리즘 실행
        BFS(Player.Tile);
    }
    
    GameObject music;

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

        hasGameEnded = false;
        layerConncet = 1 << LayerMask.NameToLayer("Connect");
        UI_Clear_Prefab = Resources.Load<GameObject>("UI/ClearCanvas");
        music = Resources.Load<GameObject>("Sound/Music");
    }

    void Start()
    {
        SpawnStage();
        CameraSetting();

        Instantiate(music);
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
        // 카메라 사이즈를 가져와서 Row, Col 둘 중에 큰 값을 반환 하고 절반을 나눈 후 보정 값을 1 준다.
        Camera.main.orthographicSize = (Mathf.Max(Level.Row, Level.Column) / 2f) + 1f;
        // 카메라 위치 값을 저장
        Vector3 cameraPos = Camera.main.transform.position;
        // 카메라 위치를 레벨 데이터의 행과 열의 최대 값으로
        cameraPos.x = Level.Column;
        cameraPos.y = Level.Row + 1;
        // 카메라 위치를 갱신하기
        Camera.main.transform.position = cameraPos;
    }
}
