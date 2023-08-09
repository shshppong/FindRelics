using System.Collections;
using System.Collections.Generic;

/* 3 x 3 ���� ���� ����
 * 0, ��, ��, ��, ��, ȸ�� 0, 90, 180, 270
 * 1, ��, ��, ��, ��, 
 * 2, ��
 * 
 * ��   ���� ��ġ (-1, -1) 
 * �ۡۡ� ��¥ ���� ��ġ (0, 0)
 * �ۡۡ�
 * �ۡۡ� ���� ��ġ (2, 2) size - 1
 *   �� 
 */

public class Board
{
    // Ÿ�� ������
    public TileData[,] Tiles { get; private set; }
    // Ÿ�� ũ��
    public int Size { get; private set; }
    // ���� ���� Destination
    public int DestY { get; private set; }
    public int DestX { get; private set; }
    // �÷��̾� ������
    Player _player;

    public void Initialize(int size, Player player)
    {
        // ���� ����� ¦������ ��
        if (size % 2 == 0)
            return;
        // �÷��̾� ������ ����
        _player = player;
        // Ÿ�� �����͵��� �����ŭ �����Ѵ�
        Tiles = new TileData[size, size];
        // �������� �����͸� ����ȭ
        Size = size;

        // ������ ��ġ
        // �� ������ Ÿ���� ������ �������� ���ϰ� ������ Ż�� �� ������ ģ��
        DestX = size;
        DestY = size;

        // ��ã�� ��ã�� �˰��� ȣ�� ����
    }
}
