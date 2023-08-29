using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [HideInInspector] public Tile Tile;

    // 이동하는데 걸리는 시간
    public float moveDuration = 1.0f;

    public void Initialize(Transform tr, Tile tile)
    {
        Tile = tile;
        
        transform.position = tr.position;
    }

    public void MoveStart(Queue<Tile> q)
    {
        if (q.Count == 0)
        {
            Debug.Log("All positions reached.");
            GameManager.Instance.tweenQueue.Clear();
            return;
        }

        // 첫 번째 위치로 이동하고 이동이 끝나면 다음 위치로 이동하는 콜백 및 재귀함수 설정
        Transform target = q.Dequeue().transform.Find("Floor");
        // 타겟 방향으로 캐릭터 바라보기
        Vector3 fwd = target.position - transform.position;
        transform.forward = fwd;
        Tween move = transform.DOMove(target.position, moveDuration)
            .OnComplete(() =>
            {
                // 다음 위치로 이동
                GameManager.Instance.tweenQueue.Dequeue();
                MoveStart(q);
            });

        GameManager.Instance.tweenQueue.Enqueue(move);
    }
}
