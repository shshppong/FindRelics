using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static PublicLibrary;

public class Tile : MonoBehaviour
{
    [SerializeField] private Transform[] _tilePrefabs;

    // 타일(자식) 데이터를 가질 변수
    [HideInInspector] public TileType TileType = new TileType();
    public bool IsClosed;

    public int minMoveTile = 4;
    public int maxMoveTile = 7;

    // 회전 상수
    private const int minRotation = 0;
    private const int maxRotation = 3;
    private const int rotationMultiplier = 90;

    private int rotation;

    private TileButton btn;

    public Transform currentTile;

    public List<Transform> currentTileList = new List<Transform>();

    [SerializeField] private List<Transform> connectTiles;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void InsertValue(Tile newTile)
    {
        TileType = newTile.TileType;
        rotation = newTile.rotation;
        for (int i = 0; i < currentTileList.Count; i++)
        {
            if (currentTileList[i].gameObject.activeSelf == true)
            {
                currentTile = currentTileList[i];
                break;
            }
        }

        if (currentTileList != null)
            currentTileList.Clear();
        currentTileList.AddRange(newTile.currentTileList);
    }

    public void Initialize(TileType tileType, LevelData level, int y, int x)
    {
        TileType = tileType;
        int tileTypeNum = (int)TileType % (maxMoveTile + 1);

        // 비어있는 타일이라면 생성하지 않기
        if (TileType == TileType.Empty)
            return;

        if (tileTypeNum >= minMoveTile && tileTypeNum <= maxMoveTile)
        {
            rotation = Random.Range(minRotation, maxRotation);
            for (int i = minMoveTile; i <= maxMoveTile; i++)
            {
                currentTile = Instantiate(_tilePrefabs[i], transform);
                currentTile.transform.localPosition = Vector3.zero;
        
                currentTile.transform.rotation = Quaternion.Euler(new Vector3(0, rotation * rotationMultiplier, 0));

                currentTileList.Add(currentTile);

                currentTile.gameObject.SetActive(false);
            }

            currentTile = transform.GetChild(tileTypeNum % minMoveTile);
            currentTile.gameObject.SetActive(true);
        }
        else
        {
            Transform currentTile = Instantiate(_tilePrefabs[tileTypeNum], transform);
            currentTile.transform.localPosition = Vector3.zero;
            currentTile = transform.GetChild(0);

            // 버튼 타일이라면
            if (TileType == TileType.Button)
            {
                btn = transform.GetComponentInChildren<TileButton>();
                btn.Y = y;
                btn.X = x;

                if (y == level.Column - 1)
                {
                    btn.transform.eulerAngles = new Vector3(0, -1 * rotationMultiplier, 0);
                }
            }

            if (tileType == TileType.Start || tileType == TileType.End)
            {
                Transform child = transform.GetChild(0);
                currentTileList.Add(child);
                for (int i = 1; i < child.childCount; i++)
                {
                    connectTiles.Add(child.GetChild(i));
                }
            }
        }

        // 지정한 타일은 
        if (TileType == TileType.Empty || TileType == TileType.Start)
        {
            IsClosed = true;
        }
    }

    public void ChangeTile(Tile tile)
    {
        TileType tileType = tile.TileType;

        // 이 오브젝트에 활성화 되어있는 타일을 비활성화 시킨다.
        Transform beforeTr = currentTileList[(int)TileType % minMoveTile];
        beforeTr.gameObject.SetActive(false);
        
        // 바꿀 대상 타일의 회전 값 y을 가져온다.
        Vector3 afterRot = new Vector3(0, tile.rotation * rotationMultiplier, 0);

        // 이 오브젝트에 활성화 시킬 자식의 타일을 가져온다.
        beforeTr = currentTileList[(int)tileType % minMoveTile];

        // 대상 오브젝트와 바꿀 타일의 회전 값을 덮어씌운다.
        beforeTr.transform.localEulerAngles = afterRot;
        
        // 대상 오브젝트를 활성화 시킨다.
        beforeTr.transform.gameObject.SetActive(true);

        TileType = tileType;
        for (int i = 0; i < currentTileList.Count; i++)
        {
            if (currentTileList[i].gameObject.activeSelf == true)
            {
                currentTile = currentTileList[i];
                break;
            }
        }
        rotation = tile.rotation;
    }

    public List<Tile> ConnectedTiles()
    {
        List<Tile> result = new List<Tile>();

        connectTiles.Clear();
        for (int i = 0; i < currentTileList.Count; i++)
        {
            if (currentTileList[i].gameObject.activeSelf == true)
            {
                Transform child = currentTileList[i];
                for (int j = 1; j < child.childCount; j++)
                {
                    connectTiles.Add(child.GetChild(j));
                }
                break;
            }
        }

        foreach (Transform tile in connectTiles)
        {
            RaycastHit[] hit = Physics.RaycastAll(tile.transform.position, tile.forward, 0.2f);
            for (int i = 0; i < hit.Length; i++)
            {
                result.Add(hit[i].collider.transform.GetComponentInParent<Tile>());
            }
        }

        return result;
    }

    //private void OnDrawGizmos()
    //{
    //    if (transform.childCount <= 0) return;
    //    if (currentTileList.Count <= 0) return;

    //    foreach (Transform tile in transform)
    //    {
    //        if (tile.gameObject.activeSelf == true)
    //        {
    //            for (int i = 1; i < tile.childCount; i++)
    //            {
    //                //Debug.DrawLine(tile.GetChild(i).position, , Color.red, 0.1f);
    //                Gizmos.color = Color.green;
    //                Gizmos.DrawLine(tile.GetChild(i).position, tile.GetChild(i).position + tile.GetChild(i).forward * 0.2f);
    //            }
    //        }
    //    }
    //}
}
