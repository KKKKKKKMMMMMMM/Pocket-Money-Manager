using System.Diagnostics;
using UnityEngine;

public static class LogManager
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(string str)
    {
        UnityEngine.Debug.Log(str);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string str)
    {
        UnityEngine.Debug.LogWarning(str);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(string str)
    {
        UnityEngine.Debug.LogError(str);
    }
}
