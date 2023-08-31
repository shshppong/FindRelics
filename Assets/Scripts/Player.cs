using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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

            // 스테이지 클리어 프리팹 생성하기
            Instantiate(GameManager.Instance.UI_Clear_Prefab);

            return;
        }

        // 첫 번째 위치로 이동하고 이동이 끝나면 다음 위치로 이동하는 콜백 및 재귀함수 설정
        Transform target = q.Dequeue().transform.Find("Floor");

        // 타겟 방향으로 캐릭터 바라보기
        Vector3 fwd = target.position - transform.position;
        transform.forward = fwd;

        // 이동 트윈 실행하기
        Tween move = transform.DOMove(target.position, moveDuration)
            .OnComplete(() =>
            {
                // 다음 위치로 이동
                GameManager.Instance.tweenQueue.Dequeue();
                // 재귀 실행
                MoveStart(q);
            });

        // 실행 중인 트윈 큐에 넣기
        GameManager.Instance.tweenQueue.Enqueue(move);
    }
}
