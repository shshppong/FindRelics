using System.Collections;
using System.Collections.Generic;

public class Player
{
    // �÷��̾��� ��ġ ���� �����ϴ� Ŭ����
    public int PosX { get; private set; }
    public int PosY { get; private set; }

    Board _board; // ���⼭ ����ϴ� ����

    // �ٶ󺸴� ��ġ�� �����ش�.
    //int dir = (int)Dir.Up;

    // �÷��̾��� ��ġ�� ���� �����͸� �����Ѵ�
    public void Initialize(int posY, int posX, Board board)
    {
        PosY = posY;
        PosX = posX;
        _board = board;
    }
}
