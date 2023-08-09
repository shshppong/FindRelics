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
}