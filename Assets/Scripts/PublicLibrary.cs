public static class PublicLibrary
{
    // 타일 확인을 위한 열거형
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
    
    // 기획자가 편하게 맵 데이터를 추가할 수 있도록 한 데이터 집합
    [System.Serializable]
    public struct TileData
    {
        public TileType TileType;
        public UnityEngine.GameObject TilePrefab;
        public int Rotation;
    }

    // 컴포넌트가 존재하면 반환하고 아니라면 새로 추가하기
    public static T GetOrAddComponent<T> (this UnityEngine.GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }
}
