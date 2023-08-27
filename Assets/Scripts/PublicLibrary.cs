public class PublicLibrary
{
    public enum TileType
    {
        Empty = 0,
        Button = 1,
        Start = 2,
        End = 3,
        Straight = 4,
        Up_Left_Down_Right = 5,
        Up_Left_Down = 6,
        Up_Left = 7
    }
    
    [System.Serializable]
    public struct TileData
    {
        public TileType tileType;
        public int rotation;
    }
}
