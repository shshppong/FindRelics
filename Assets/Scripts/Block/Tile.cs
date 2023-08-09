using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //[HideInInspector] public bool IsFilled;
    [HideInInspector] public int TileType;

    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform[] _tilePrefabs;

    private Transform currentTile;
    private int rotation;

    // SpriteRenderer
    // SpriteRenderer
    private List<Transform> connectBoxes;

    private const int minRotation = 0;
    private const int maxRotation = 3;
    private const int rotationMultiplier = 90;

    public void Initialize(int tile)
    {
        // Ÿ�� ������ ���Ե� ���ڰ� ���� �ڸ��� �Ѿ�� 0~3���� �����ǰ�
        TileType = tile % levelData.Column;
        // ������ Ÿ�� �������� Ʈ���� ������ ������
        currentTile = Instantiate(_tilePrefabs[TileType], transform);
        // ������ Ÿ�Ͽ� Ʈ���� ������ ��ġ�� 0, 0, 0���� ��
        currentTile.transform.localPosition = Vector3.zero;
        // Ÿ�� Ÿ���� ���� �Ǵ� �� �̶��
        if (TileType == 1 || TileType == 2)
        {
            // ���۰� �� Ÿ��
            // rotation = tile / 10;
        }
        else
        {
            // �� �� Ÿ�Ͽ� ���� ȸ�� �� ���� ����
            rotation = Random.Range(minRotation, maxRotation + 1);
        }
        currentTile.transform.eulerAngles = new Vector3(0, rotation * rotationMultiplier, 0);

        if (TileType == 0 || TileType == 1)
        {
            //IsFilled = true;
        }

        if (TileType == 0)
        {
            return;
        }

        // emptySprite = currentTile.GetChild(0).GetComponent<SpriteRenderer>();
        // emptySprite.gameObject.SetActive(!IsFilled);
        // filledSprite = currentTile.GetChild(1).GetComponent<SpriteRenderer>();
        // filledSprite.gameObject.SetActive(IsFilled);��

        connectBoxes = new List<Transform>();
        for (int i = 2; i < currentTile.childCount; i++)
        {
            connectBoxes.Add(currentTile.GetChild(i));
        }
    }
}
