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

    private List<Tile> startTiles;

    [HideInInspector] public bool hasGameFinished;

    [Header("Timer")]
    public float gameFinishedTimer = 2f;

    private float checkDelayTimer = 0.1f;

    private void Awake()
    {
        Instance = this;
        SpawnLevel();
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
        _tileData = new Tile[_level.Row, _level.Column];
        startTiles = new List<Tile>();
        float yPos = 0f;
        float xPos = 0f;

        for (int y = 0; y < _level.Row; y++)
        {
            for (int x = 0; x < _level.Column; x++)
            {
                // 부모 타일 오브젝트 생성하기
                Vector3 newPos = new Vector3(yPos, 0, xPos);
                Tile newTile = Instantiate(_tilePrefab).GetComponent<Tile>();
                // 부모 타일 위치 정의하기
                newTile.transform.position = newPos;
                TileType tempType = _level.Data[y * _level.Column + x];
                // 부모 타일 속성 초기화 하기 (자식 오브젝트에 타일 하위 생성)
                newTile.Initialize(tempType, y, x);

                // 배열에 타일 데이터 집어넣기
                _tileData[y, x] = newTile;

                // 시작 타일이라면 startTiles 리스트에 넣기
                if (tempType == TileType.Start)
                    startTiles.Add(newTile);

                xPos += defaultSpacing + spawnSpacing;
            }
            yPos += defaultSpacing + spawnSpacing;
            xPos = 0;
        }

        Camera.main.orthographicSize = (Mathf.Max(_level.Row, _level.Column) / 2f) + 1f;
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.x = _level.Column;
        cameraPos.y = _level.Row + 1;
        Camera.main.transform.position = cameraPos;

        StartCoroutine(CheckDelay(checkDelayTimer));
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

    //public void PrintTileNumberArray()
    //{
    //    string strs = null;
    //    for (int y = 0; y < _tileData.GetLength(0); y++)
    //    {
    //        for (int x = 0; x < _tileData.GetLength(1); x++)
    //        {
    //            strs += $"{(int)_tileData[y, x].TileType}\t";
    //        }
    //        strs += "\n";
    //    }
    //    Debug.Log(strs);
    //}

    public void RunCoroutine(int y, int x)
    {
        Shift(y, x);
        StartCoroutine(CheckDelay(checkDelayTimer));
    }

    public IEnumerator CheckDelay(float sec)
    {
        yield return new WaitForSeconds(sec);

        // 타일 연결 검사하고 해당 타일에 불리언 값 할당
        CheckTile();
        // 불리언 값 검사로 연결이 정상적으로 되었는지 확인
        CheckWin();
    }

    private void CheckTile()
    {
        // 타일이 연결되었던 흔적 변경
        for (int i = 1; i < _level.Row - 2; i++)
        {
            for (int j = 1; j < _level.Column - 2; j++)
            {
                Tile tempTile = _tileData[i, j];
                if (tempTile.TileType != TileType.Empty || tempTile.TileType != TileType.Button)
                {
                    tempTile.IsClosed = false;
                }
            }
        }

        // 확인할 타일을 큐에 넣기
        Queue<Tile> check = new Queue<Tile>();
        HashSet<Tile> finished = new HashSet<Tile>();
        foreach (Tile tile in startTiles)
        {
            check.Enqueue(tile);
        }

        // 큐에 넣은 타일 하나씩 빼와서 레이캐스트 체크해서 연결 검사하기
        while (check.Count > 0)
        {
            Tile tile = check.Dequeue();
            finished.Add(tile);
            List<Tile> connected = tile.ConnectedTiles();
            foreach (Tile connectTile in connected)
            {
                if (!finished.Contains(connectTile))
                {
                    check.Enqueue(connectTile);
                }
            }
        }

        // 검사 완료된 타일은 연결된 것으로 간주
        foreach (Tile filled in finished)
        {
            filled.IsClosed = true;
        }
    }

    private void CheckWin()
    {
        // 끝 지점까지 연결되었는지 체크
        int y = _level.Row - 1;
        int x = _level.Column - 1;
        if (!_tileData[y - 1, x].IsClosed)
            return;

        hasGameFinished = true;
        StartCoroutine(GameFinishedLoadScene(gameFinishedTimer));
    }

    private IEnumerator GameFinishedLoadScene(float sec)
    {
        yield return new WaitForSeconds(sec);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        PrintTileNumberArray();
    //    }
    //}
}
