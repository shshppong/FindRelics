using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.PackageManager;

public class TileButton : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] Animator anim;

    [HideInInspector] public Tile Tile;

    GameManager gameManager;

    void Awake() => gameManager = GameManager.Instance;

    public int X { get; private set; }
    public int Y { get; private set; }

    [HideInInspector] public int TweenCount;

    // 초기 위치 저장
    [SerializeField] List<Vector3> tempVec;

    void Start()
    {
        anim.GetComponent<Animator>();
        Tile = GetComponent<Tile>();

        tempVec = new List<Vector3>();

        TweenCount = 0;
    }

    public void Initialize()
    {
        // 초기 위치 저장하기
        List<Transform> tileTransforms = TilesButtonLine();
        foreach (Transform tile in tileTransforms)
        {
            tempVec.Add(tile.position);
        }
    }

    // 마우스 가까이 가져다 대었을 때
    void OnMouseEnter()
    {
        anim.SetBool("isEntered", true);
    }

    void OnMouseExit()
    {
        anim.SetBool("isEntered", false);
    }

    void OnMouseDown()
    {
        anim.SetTrigger("Clicked");

        // 만약 애니메이션이 실행되고 있는 중 이라면, 리턴 시킨다.
        int totalPlayingTweens = TweenCount;
        if (totalPlayingTweens > 0)
            return;

        // 타일 스왑 시킨다.
        ObjectSwap(TilesButtonLine());
    }

    public List<Transform> TilesButtonLine()
    {
        // 해당되는 타일들을 전부 담기 위한 타일 리스트
        List<Transform> result = new List<Transform>();
        // 버튼 타일을 기준으로 정면에 레이캐스트를 쏘았을 때
        // "Tile" 레이어를 가진 Collider 오브젝트들 만 리스트에 넣기
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Tile");
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity, layerMask);
        // 거리에 따라 결과를 정렬
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        result = hits.Select((hit) => hit.collider.transform).ToList();

        return result;
    }

    public void ObjectSwap(List<Transform> newList)
    {
        for (int i = 0; i < newList.Count - 1; i++)
        {
            MoveObjectToTarget(newList[i].transform, tempVec[i + 1], gameManager.PlayingAnimSpeed);
        }
        MoveObjectWithCurve(newList[newList.Count - 1].transform, tempVec[0], 0.1f, gameManager.PlayingAnimSpeed);
    }

    private void MoveObjectToTarget(Transform obj, Vector3 targetPosition, float duration)
    {
        // 트윈 실행
        obj.DOMove(targetPosition, duration).OnComplete(() =>
        {
            TweenCount--;
        });

        TweenCount++;
    }

    private void MoveObjectWithCurve(Transform obj, Vector3 targetPos, float curveStrength, float duration)
    {
        Vector3 middlePoint = (obj.position + targetPos) * 0.5f;
        // 아래로 -1만큼 위치 수정
        middlePoint.y = -1f;
        Vector3 direction = (middlePoint - obj.position).normalized;
        Vector3 controlPoint = middlePoint + direction * curveStrength;

        obj.DOLocalPath(new Vector3[] { obj.localPosition, controlPoint, targetPos }, duration).OnComplete(() =>
        {
            TweenCount--;
        });

        TweenCount++;
    }
}
