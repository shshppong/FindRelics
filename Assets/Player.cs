using System.Collections;
using System.Collections.Generic;

public class Player
{
    // 플레이어의 위치 값을 저장하는 클래스
    public int PosX { get; private set; }
    public int PosY { get; private set; }

    Board _board; // 여기서 사용하는 변수

    // 바라보는 위치를 정해준다.
    //int dir = (int)Dir.Up;

    // 플레이어의 위치와 보드 데이터를 참조한다
    public void Initialize(int posY, int posX, Board board)
    {
        PosY = posY;
        PosX = posX;
        _board = board;
    }
}
