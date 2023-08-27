using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Spawn Level Value")]
    //[SerializeField] private LevelData _level;
    public int Row = 3;
    public int Column = 3;
    [HideInInspector] public int row = 5;
    [HideInInspector] public int column = 5;
    [SerializeField] private GameObject _tilePrefab;

    [Header("Block Spacing")]
    public float spawnSpacing = 0.25f;
    public float defaultSpacing = 1f;

    private Tile[,] _tileData;

    private GameObject tempGameObject;
    private Tile tempTile;

    private List<Tile> startTiles;

    [HideInInspector] public bool hasGameFinished;

    [Header("Timer")]
    public float gameFinishedTimer = 2f;

    private TileType[,] map;

    public bool isGenerate;
    public bool isRelocation;

    private List<Transform> trs;

    public int Straight_Tile = 0;
    public int Up_Left_Down_Right = 0;
    public int Up_Left_Down = 0;
    public int Up_Left = 0;

    private void Awake()
    {
        Instance = this;

        row = Row + 2;
        column = Column + 2;

        // 기본 위치 정의
        // 1,0 2,0 3,0      x = 1       x - (row - 1)
        // 3,1 3,2 3,3      y = 3       y - (col - 1)
        // StartTile        (0, 1)
        // EndTile          (3, 4)
        map = new TileType[row, column];
        map[0, 1] = TileType.Start;
        map[row - 2, column - 1] = TileType.End;
        for (int y = 1; y < row - 1; y++)
            map[y, 0] = TileType.Button;
        for (int x = 1; x < column - 1; x++)
            map[row - 1, x] = TileType.Button;
        for (int y = 1; y < row - 1; y++)
            for (int x = 1; x < column - 1; x++)
                map[y, x] = TileType.Straight;

        isGenerate = false;
        isRelocation = false;

        GenerateMapAndCheckTile();
    }

    private void Start()
    {
        //PrintTileNumberArray();
        tempGameObject = new GameObject("TempGameObject");
        tempGameObject.transform.position = Vector3.up * -100f;
        tempTile = tempGameObject.AddComponent<Tile>();
        hasGameFinished = false;
    }

    public void SpawnLevel()
    {
        if (trs != null)
            if (trs.Count > 0)
                foreach (Transform tr in trs)
                    Destroy(tr.gameObject);

        _tileData = new Tile[row, column];
        startTiles = new List<Tile>();
        trs = new List<Transform>();
        float yPos = 0f;
        float xPos = 0f;

        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < column; x++)
            {
                // 부모 타일 오브젝트 생성하기
                Vector3 newPos = new Vector3(yPos, 0, xPos);
                Tile newTile = Instantiate(_tilePrefab).GetComponent<Tile>();
                // 부모 타일 위치 정의하기
                newTile.transform.position = newPos;

                //TileType tempType = _level.Data[y * _level.Column + x];
                // 부모 타일 속성 초기화 하기 (자식 오브젝트에 타일 하위 생성)
                newTile.Initialize((int)map[y, x], y, x);

                // 배열에 타일 데이터 집어넣기
                _tileData[y, x] = newTile;

                // 시작 타일이라면 startTiles 리스트에 넣기
                if (newTile.TileType == TileType.Start)
                    startTiles.Add(newTile);

                trs.Add(newTile.transform);

                xPos += defaultSpacing + spawnSpacing;
            }
            yPos += defaultSpacing + spawnSpacing;
            xPos = 0;
        }

        Camera.main.orthographicSize = (Mathf.Max(row, column) / 2f) + 1f;
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = column;
        cameraPos.y = row + 1;
        Camera.main.transform.position = cameraPos;

        Check();
    }

    private void Shift(int y, int x)
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

    public void RunCoroutine(int y, int x)
    {
        Shift(y, x);
        Check();
    }

    public void Check()
    {
        // 타일 연결 검사하고 해당 타일에 불리언 값 할당
        CheckTile();
        // 불리언 값 검사로 연결이 정상적으로 되었는지 확인
        CheckWin();
    }

    private void CheckTile()
    {
        // 타일이 연결되었던 흔적 변경
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Tile tempTile = _tileData[i, j];
                tempTile.IsClosed = false;
            }
        }

        // 확인할 타일을 큐에 넣기
        Queue<Tile> check = new Queue<Tile>();
        HashSet<Tile> finished = new HashSet<Tile>();
        foreach (Tile tile in startTiles)
            check.Enqueue(tile);

        // 큐에 넣은 타일 하나씩 빼와서 레이캐스트 체크해서 연결 검사하기
        while (check.Count > 0)
        {
            Tile tile = check.Dequeue();
            finished.Add(tile);
            List<Tile> connected = tile.ConnectedTiles();
            foreach (Tile connectTile in connected)
                if (!finished.Contains(connectTile))
                    check.Enqueue(connectTile);
        }

        // 검사 완료된 타일은 연결된 것으로 간주
        foreach (Tile filled in finished)
        {
            filled.IsClosed = true;
        }
    }

    private bool CheckWin()
    {
        // 끝 지점까지 연결되었는지 체크
        int y = row - 2;
        int x = column - 1;

        // 생성이 제대로 되지 않았으면 승리하지 않기
        if (isGenerate == false)
        {
            isGenerate = true;
            return false;
        }

        // 생성이 제대로 되었고, 마지막 타일까지 연결되어있다면 승리하기
        if (isRelocation == true)
        {
            if (!_tileData[y, x].IsClosed)
                return false;

            hasGameFinished = true;
            StartCoroutine(GameFinishedLoadScene(gameFinishedTimer));
            return true;
        }

        return true;
    }

    private IEnumerator GameFinishedLoadScene(float sec)
    {
        yield return new WaitForSeconds(sec);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
    private void GenerateMapAndCheckTile()
    {
        if (isGenerate == false)
            SpawnLevel();

        if (isGenerate == true)
        {
            // 타일이 목적지까지 연결이 되어있다면,
            // 배치된 타일들을 1,1 ~ 3,3 까지 List에 넣고
            // List에 넣어져있는 요소들을 랜덤으로 섞은 후 큐에 넣는다.
            // 0,0 부터 큐에 들어간 타일들을 배치시켜야한다.
            // 배치하고 다시 연결되어있는지 체크해야 한다.

            // 배치된 타일들을 1,1 ~ 3,3 까지 List에 넣고
            List<Tile> tiles = new List<Tile>();
            for (int y = 1; y < Row + 1; y++)
                for (int x = 1; x < Column + 1; x++)
                    tiles.Add(_tileData[x, y]);

            // List에 넣어져 있는 요소들을 랜덤으로 섞은 후 큐에 넣는다.
            ShuffleList(tiles);

            Queue<Tile> tileQueue = new Queue<Tile>(tiles);
            for (int y = 1; y < Row + 1; y++)
                for (int x = 1; x < Column + 1; x++)
                    _tileData[y, x].ChangeTile(tileQueue.Dequeue());

            Check();

            isRelocation = true;
        }
        else
        {
            isGenerate = false;
            isRelocation = false;
            GenerateMapAndCheckTile();
        }
    }

    // 무작위로 리스트를 섞는 함수
    public static void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random();

        // Fisher-Yates 알고리즘을 사용하여 리스트를 섞음
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
