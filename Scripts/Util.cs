using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            return go.AddComponent<T>();
        return component;
    }

    public static void Shift<T>(T[,] array, Vector2 dir) where T : Tile
    {
        int row = array.GetLength(0);
        int col = array.GetLength(1);
        if (dir == Vector2.right)
        {
            int lastIndex = col - 2;
            T tempTile = array[lastIndex, 1];
            Vector3 tempPos = array[lastIndex, 1].Pos;
            for (int i = col - 2; i > 1; i--)
            {
                Debug.Log($"변경 전 arr {array[i, 1].Pos}");
                array[i, 1] = array[i - 1, 1];
                Debug.Log($"변경 후 arr {array[i, 1].Pos}");
                array[i, 1].Pos = array[i - 1, 1].Pos;
            }
            array[1, 1] = tempTile;
            array[1, 1] = tempPos as T;
        }
    }
}