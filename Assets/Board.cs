using System.Collections;
using System.Collections.Generic;

/* 3 x 3 기준 보드 세팅
 * 0, ┌, ┐, └, ┘, 회전 0, 90, 180, 270
 * 1, ┤, ┬, ├, ┴, 
 * 2, ┼
 * 
 * ●   시작 위치 (-1, -1) 
 * ○○○ 진짜 시작 위치 (0, 0)
 * ○○○
 * ○○○ 종료 위치 (2, 2) size - 1
 *   ● 
 */

public class Board
{
    // 타일 데이터
    public TileData[,] Tiles { get; private set; }
    // 타일 크기
    public int Size { get; private set; }
    // 골인 지점 Destination
    public int DestY { get; private set; }
    public int DestX { get; private set; }
    // 플레이어 데이터
    Player _player;

    public void Initialize(int size, Player player)
    {
        // 보드 사이즈가 짝수여야 함
        if (size % 2 == 0)
            return;
        // 플레이어 데이터 참조
        _player = player;
        // 타일 데이터들을 사이즈만큼 생성한다
        Tiles = new TileData[size, size];
        // 사이즈의 데이터를 동기화
        Size = size;

        // 마지막 위치
        // 는 마지막 타일의 방향이 오른쪽을 향하고 있으면 탈출 한 것으로 친다
        DestX = size;
        DestY = size;

        // 길찾기 길찾기 알고리즘 호출 ㄱㄱ
    }
}
